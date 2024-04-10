/*
 * Description: Player Loss and Reset
 * Authors: mcmyers4, blopezro, dnguye99asu
 * Version: 20240410
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player death script to handle hazard interactions and respawn attempts.
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    private PlayerController _playerController;
    private System.DateTime LastActivation;

    public PlayerHealth PlayerHealth;                            // Initial player health
    public SolarArrayManager SolarArrayManager;                  // Initial solar array
    public HashSet<int> ReachedCheckpoints = new HashSet<int>(); // Stores unique IDs of checkpoints

    public void Initialize(PlayerController PlayerController)
    {
        _playerController = PlayerController;
        GameController.Instance.GameStateManager.StartPoint = _playerController.transform.position;
        GameController.Instance.GameStateManager.RespawnPoint = _playerController.transform.position;
    }

    /// <summary>
    /// Initialize respawn point and set starting location.
    /// </summary>
    private void Start()
    {
        GameController.Instance.GameStateManager.StartPoint = _playerController.transform.position;
        GameController.Instance.GameStateManager.RespawnPoint = _playerController.transform.position;
    }

    /// <summary>
    /// Player touches hazard object.
    /// </summary>
    public void Hazard()
    {
        ApplyKickback();
        GameController.Instance.AudioManager.PlayerHurt.Play();
        GetHurt(1);
    }

    /// <summary>
    /// When player touches spikes
    /// </summary>
    public void Spikes()
    {
        GameController.Instance.AudioManager.PlayerHurt.Play();
        GetHurt(PlayerHealth.playerHealth);
    }

    /// <summary>
    /// Applies a kickback force to the player character
    /// </summary>
    private void ApplyKickback()
    {
        // calculate kickback direction
        // left up or right up
        Vector2 kickbackDirection;
        if (PlayerController.Instance.playerMovement.IsFacingRight)
        {
            kickbackDirection = new Vector2(-5f, 5f).normalized;
        }
        else
        {
            kickbackDirection = new Vector2(5f, 5f).normalized;
        }

        // set force and apply
        float kickbackForce = 5f;
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.playerCharacter.AddForce(kickbackDirection * kickbackForce, ForceMode2D.Impulse);
        PlayerController.Instance.inputBlocked = false;
    }

    /// <summary>
    /// Updates respawn location once a Checkpoint has been touched.
    /// Additionally recharges health and battery.
    /// </summary>
    /// <param name="Collision"></param>
    public void Checkpoint(Collider2D Collision)
    {
        if (!Collision.gameObject.CompareTag(_playerController.playerCollisionManager.MatchTag(PlayerCollisionManager.CollisionTag.Checkpoint)))
        {
            return;
        }
        //ID of the Checkpoint
        GameController.Instance.GameStateManager.Checkpoint = Collision.gameObject.GetInstanceID();
        GameController.Instance.GameStateManager.RespawnPoint = _playerController.transform.position;

        //Recharge health and battery
        PlayerHealth.HealthUp(100);
        gameObject.GetComponent<SolarArrayManager>().Activate();

        //Play audio if you hit a Checkpoint with default layer
        if (Collision.gameObject.layer.Equals(0))
        {
            if (LastActivation == null)
            {
                GameController.Instance.AudioManager.Checkpoint.Play();
                LastActivation = System.DateTime.Now;
            }
            else
            {
                System.TimeSpan diff = System.DateTime.Now - LastActivation;
                if (diff.TotalSeconds > 5)
                {
                    LastActivation = System.DateTime.Now;
                    GameController.Instance.AudioManager.Checkpoint.Play();
                }
            }
        }
    }

    /// <summary>
    /// Lose health on contacts with hazards.
    /// Respawn character if no health remains.
    /// </summary>
    public void GetHurt(int dmg)
    {
        StartCoroutine(_playerController.interruptMagnet());
        PlayerHealth.HealthDown(dmg);
        if (PlayerHealth.playerHealth <= 0)
        {
            //start the warping animation & reset player's heath & battery
            StartCoroutine(GameController.Instance.GameStateManager.Warp());
        }
    }
}
