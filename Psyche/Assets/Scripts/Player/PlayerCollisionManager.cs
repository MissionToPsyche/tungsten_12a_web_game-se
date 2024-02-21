using System;
using UnityEngine;

/// <summary>
/// Script which handles all collisions made by the player
/// </summary>
public class PlayerCollisionManager : MonoBehaviour
{
    // Private
    private PlayerController _playerController;

    // Events
    public event Action<string> InitiateTransition;
    public event Action SaveSceneState;
    public event Action<string, object> SetObjectState;
    //public event Action<Collision2D> OnCollisionEnter;
    //public event Action<Collision2D> OnCollisionExit;
    //public event Action<Collision2D> OnCollisionStay;

    /// <summary>
    /// Enum for tracking object names upon collision
    /// </summary>
    public enum CollisionName
    {
        None,
    }

    /// <summary>
    /// Matches the name value (string) with its respective CollisionName enum
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public CollisionName MatchName(string name)
    {
        return name.ToLower() switch
        {
            _ => CollisionName.None,
        };
    }

    /// <summary>
    /// Matches the CollisionName enum value with its respective string
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string MatchName(CollisionName name)
    {
        return name switch
        {
            _ => null,
        };
    }

    /// <summary>
    /// For tracking the various CollisionTags
    /// </summary>
    public enum CollisionTag
    {
        // Pickupable
        Element,
        Tool,

        // Transition Object Directions
        TransitionObjectIn,
        TransitionObjectOut,

        // Hazards
        Hazard,
        Spikes,

        // Checkpoint
        Checkpoint,

        None,
    }

    /// <summary>
    /// Matches a given tag (string) with its enum variant
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
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
    /// Matches a given tag (enum) with its string variant
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Initializes the script and assigns a reference to PlayerController
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    /// <summary>
    /// Object collision entrance
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (MatchTag(other.gameObject.tag))
        {
            case CollisionTag.Hazard:
                _playerController.playerDeath.Hazard(other);
                break;
            case CollisionTag.Spikes:
                _playerController.playerDeath.Spikes();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Object collision exit
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionExit2D(Collision2D other)
    {
        // Stuff
    }

    /// <summary>
    /// Object collision stay
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay2D(Collision2D other)
    {
        // Enter the rest of the stuff
    }

    /// <summary>
    /// Object collision - trigger enter
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (MatchName(other.name))
        {
            default:
                break;
        }

        switch (MatchTag(other.tag))
        {
            case CollisionTag.Element:
                _playerController.inventoryManager.AddElement(other.name, 1);
                Destroy(other.gameObject);
                break;

            case CollisionTag.Tool:
                _playerController.inventoryManager.ToolPickUp(other.name);
                Destroy(other.gameObject);
                break;

            case CollisionTag.Checkpoint:
                SetObjectState?.Invoke(other.name, true);
                SaveSceneState?.Invoke();
                _playerController.playerDeath.Checkpoint(other);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Object collision - trigger exit
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // Trigger Exit Stuff
    }

    /// <summary>
    /// Object collision - trigger stay
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.name)
        {
            default:
                break;
        }

        switch (MatchTag(other.tag))
        {
            case CollisionTag.TransitionObjectIn or CollisionTag.TransitionObjectOut:
                InitiateTransition?.Invoke(other.name);
                break;

            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
