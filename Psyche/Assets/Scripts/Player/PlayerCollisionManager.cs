/*
 * Authors: JoshBenn
 */
using System;
using UnityEngine;

/// <summary>
/// Handles all collisions made by the player character.
/// </summary>
public class PlayerCollisionManager : MonoBehaviour
{
    //======================================== Initialize/Updates/Destroy ========================================
    // Private
    private PlayerController PlayerController;

    /// <summary>
    /// Initializes the script and assigns a reference to PlayerController.
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        PlayerController = playerController;
    }

    //================================================== Events ==================================================
    public event Action<string> InitiateTransition;
    public event Action SaveSceneState;
    public event Action<string, object> SetObjectState;

    //=================================================== Enum ===================================================
    /// <summary>
    /// All names to be tracked upon collision
    /// </summary>
    public enum CollisionName
    {
        None,
    }

    /// <summary>
    /// Matches the <see cref="string"/> name with the <see cref="CollisionName"/> variant.
    /// </summary>
    /// <param name="name"><see cref="string"/> object name</param>
    /// <returns></returns>
    public CollisionName MatchName(string name)
    {
        return name.ToLower() switch
        {
            _ => CollisionName.None,
        };
    }

    /// <summary>
    /// Matches the <see cref="CollisionName"/> variant with its respective <see cref="string"/> value.
    /// </summary>
    /// <param name="name"><see cref="CollisionName"/> variant</param>
    /// <returns><see cref="string"/> value or <see cref="null"/> if none exist</returns>
    public string MatchName(CollisionName name)
    {
        return name switch
        {
            _ => null,
        };
    }

    /// <summary>
    /// All tags to be tracked upon collision.
    /// </summary>
    public enum CollisionTag
    {
        // ===== Pickupable Objects =====
        /// <summary>
        /// Element items as defined by <see cref="InventoryManager.Element"/>.
        /// </summary>
        Element,
        
        /// <summary>
        /// Tool items as defined by <see cref="InventoryManager.Tool"/>.
        /// </summary>
        Tool,

        // ===== Transition Objects =====
        /// <summary>
        /// Transition objects into a <see cref="GameStateManager.Scene"/>.
        /// </summary>
        TransitionObjectIn,
        
        /// <summary>
        /// Transition objects out of a <see cref="GameStateManager.Scene"/>.
        /// </summary>
        TransitionObjectOut,

        // ===== Hazardous Objects =====
        /// <summary>
        /// Hazards used by <see cref="PlayerDeath"/>.
        /// </summary>
        Hazard,

        /// <summary>
        /// Spikes used by <see cref="PlayerDeath"/>.
        /// </summary>
        Spikes,

        // ===== Checkpoint Objects =====
        /// <summary>
        /// All <see cref="Checkpoint"/> objects throughout the game.
        /// </summary>
        Checkpoint,

        // ===== All Other Objects =====
        /// <summary>
        /// An object that is not tracked
        /// </summary>
        None,
    }

    /// <summary>
    /// Matches a given <see cref="string"/> tag with a <see cref="CollisionTag"/> variant.
    /// </summary>
    /// <param name="tag"><see cref="string"/> tag of the object</param>
    /// <returns><see cref="CollisionTag"/> variant</returns>
    public CollisionTag MatchTag(string tag)
    {
        return tag.ToLower() switch
        {
            "element"               => CollisionTag.Element,
            "tool"                  => CollisionTag.Tool,
            "transitionobjectin"    => CollisionTag.TransitionObjectIn,
            "transitionobjectout"   => CollisionTag.TransitionObjectOut,
            "checkpoint"            => CollisionTag.Checkpoint,
            "hazard"                => CollisionTag.Hazard,
            "spikes"                => CollisionTag.Spikes,
            _                       => CollisionTag.None,
        };
    }

    /// <summary>
    /// Matches a given <see cref="CollisionTag"/> variant with its <see cref="string"/> value.
    /// </summary>
    /// <param name="tag"><see cref="CollisionTag"/> variant</param>
    /// <returns><see cref="string"/> value or <see cref="null"/> if no matches are found</returns>
    public string MatchTag(CollisionTag tag)
    {
        return tag switch
        {
            CollisionTag.Element                => "Element",
            CollisionTag.Tool                   => "Tool",
            CollisionTag.TransitionObjectIn     => "TransitionObjectIn",
            CollisionTag.TransitionObjectOut    => "TransitionObjectOut",
            CollisionTag.Checkpoint             => "Checkpoint",
            CollisionTag.Hazard                 => "Hazard",
            CollisionTag.Spikes                 => "Spikes",
            _                                   => null,
        };
    }

    //=================================================== Collisions ===================================================
    /// <summary>
    /// Object collision entrance.
    /// </summary>
    /// <param name="other"><see cref="Collision2D"/></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (MatchTag(other.gameObject.tag))
        {
            case CollisionTag.Hazard:
                PlayerController.playerDeath.Hazard(other);
                break;
            case CollisionTag.Spikes:
                PlayerController.playerDeath.Spikes();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Object collision when entering a trigger.
    /// </summary>
    /// <param name="other"><see cref="Collision2D"/></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // All tag matches
        switch (MatchTag(other.tag))
        {
            case CollisionTag.Element:
                PlayerController.inventoryManager.AddElement(other.name, 1);
                Destroy(other.gameObject);
                GameController.Instance.AudioManager.pickupElement.Play();
                break;

            case CollisionTag.Tool:
                PlayerController.inventoryManager.ToolPickUp(other.name);
                Destroy(other.gameObject);
                GameController.Instance.AudioManager.pickupTool.Play();
                break;

            case CollisionTag.Checkpoint:
                SetObjectState?.Invoke(other.name, true);
                SaveSceneState?.Invoke();
                PlayerController.playerDeath.Checkpoint(other);
                break;

            case CollisionTag.TransitionObjectIn or CollisionTag.TransitionObjectOut:
                // Popup over the player
                PlayerController.pressUpPopup.SetActive(true);

                // If ship (tungsten: layer 12) and fewer than 8 tungsten available - disable the BoxCollider
                if (other.gameObject.layer == 12 && PlayerController.inventoryManager.CheckElement(InventoryManager.Element.Tungsten) < 8)
                {
                    other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Object collision when exiting a trigger.
    /// </summary>
    /// <param name="other"><see cref="Collision2D"/></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // All tag matches
        switch (MatchTag(other.tag))
        {
            case CollisionTag.TransitionObjectIn or CollisionTag.TransitionObjectOut:
                // Popup over the player
                PlayerController.pressUpPopup.SetActive(false);

                // If ship (tungsten: layer 12) - enable the BoxCollider
                if (other.gameObject.layer == 12)
                {
                    other.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Object collision when staying in a trigger.
    /// </summary>
    /// <param name="other"><see cref="Collision2D"/></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        // All tag matches
        switch (MatchTag(other.tag))
        {
            case CollisionTag.TransitionObjectIn or CollisionTag.TransitionObjectOut:
                InitiateTransition?.Invoke(other.name);
                break;

            default:
                break;
        }
    }
}