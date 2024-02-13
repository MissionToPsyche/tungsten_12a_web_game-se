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
    public event Action<Collision2D> OnCollisionEnter;
    public event Action<Collision2D> OnCollisionExit;
    public event Action<Collision2D> OnCollisionStay;


    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enter stuff Stuff
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Stuff
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Enter the rest of the stuff
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger enter stuff
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Trigger Exit Stuff
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Trigger stay stuff
    }



    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
