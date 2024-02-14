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

    public enum CollisionName
    {
        Health,

        None,
    }

    public CollisionName MatchName(string name)
    {
        return name.ToLower() switch
        {
            "health" or "health pickup variant" => CollisionName.Health,
            _ => CollisionName.None,
        };
    }

    public string MatchName(CollisionName name)
    {
        return name switch
        {
            CollisionName.Health => "Health",
            _ => null,
        };
    }

    public enum CollisionTag
    {
        // Pickupable
        Element,
        Tool,

        // Transition Object Directions
        TransitionObjectIn,
        TransitionObjectOut,

        Checkpoint,

        None,
    }

    public CollisionTag MatchTag(string tag)
    {
        return tag.ToLower() switch
        {
            "element"               => CollisionTag.Element,
            "tool"                  => CollisionTag.Tool,
            "transitionobjectin"    => CollisionTag.TransitionObjectIn,
            "transitionobjectout"   => CollisionTag.TransitionObjectOut,
            "checkpoint"            => CollisionTag.Checkpoint,
            _                       => CollisionTag.None,
        };
    }

    public string MatchTag(CollisionTag tag)
    {
        return tag switch
        {
            CollisionTag.Element                => "Element",
            CollisionTag.Tool                   => "Tool",
            CollisionTag.TransitionObjectIn     => "TransitionObjectIn",
            CollisionTag.TransitionObjectOut    => "TransitionObjectOut",
            CollisionTag.Checkpoint             => "Checkpoint",
            _                                   => null,
        };
    }


    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Enter stuff Stuff
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Stuff
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // Enter the rest of the stuff
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (MatchName(other.name))
        {
            case CollisionName.Health:                                        // Should we do away with the health pickup now?
                _playerController.playerHealth.HealthUp(1); 
                Destroy(other.gameObject);
                //return; // Return out early for the time being
                break;

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
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Trigger Exit Stuff
    }

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
