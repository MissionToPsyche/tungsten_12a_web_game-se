/*
 * Description: Player loss and reset
 * Authors: mcmyers4, blopezro, dnguye99asu
 * Version: 20240326
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player death script to handle hazard interactions and respawn attempts.
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    private PlayerController _playerController;
    public PlayerHealth playerHealth;                               // Initial player health
    public BatteryManager batteryManager;                           // Initial battery
    public HashSet<int> reachedCheckpoints = new HashSet<int>();    // Stores unique IDs of checkpoints

    /// <summary>
    /// Initialize respawn point and set starting location.
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        GameController.Instance.gameStateManager.startPoint = _playerController.transform.position;
        GameController.Instance.gameStateManager.respawnPoint = _playerController.transform.position;
    }

    /// <summary>
    /// Start respawn point and set starting location.
    /// </summary>
    private void Start()
    {
        GameController.Instance.gameStateManager.startPoint = _playerController.transform.position;
        GameController.Instance.gameStateManager.respawnPoint = _playerController.transform.position;
    }

    /// <summary>
    /// Player touches hazard object.
    /// </summary>
    /// <param name="collision"></param>
    public void Hazard(Collision2D collision)
    {
        ApplyKickback(collision);
        GameController.Instance.audioManager.playerHurt.Play();
        GetHurt(1);
    }

    /// <summary>
    /// When player touches spikes.
    /// </summary>
    public void Spikes()
    {
        GameController.Instance.audioManager.playerHurt.Play();
        GetHurt(playerHealth.playerHealth);
    }

    /// <summary>
    /// Applies a kickback force to the player character.
    /// </summary>
    private void ApplyKickback(Collision2D collision)
    {
        // calculate kickback direction
        Vector2 kickbackDirection;
        if (PlayerController.Instance.playerMovement._isFacingRight)
        {
            kickbackDirection = new Vector2(-5f, 5f).normalized;
        }
        else
        {
            kickbackDirection = new Vector2(5f, 5f).normalized;
        }

        // set force and apply
        PlayerController.Instance.inputBlocked = true;
        float kickbackForce = 5f;
        PlayerController.Instance.playerCharacter.AddForce(kickbackDirection * kickbackForce, ForceMode2D.Impulse);
        PlayerController.Instance.inputBlocked = false;
    }

    /// <summary>
    /// Updates respawn location once a checkpoint has been touched.
    /// Additionally recharges health and battery.
    /// </summary>
    /// <param name="collision"></param>
    public void Checkpoint(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(_playerController.playerCollisionManager.MatchTag(PlayerCollisionManager.CollisionTag.Checkpoint)))
        {
            return;
        }
        // ID of the checkpoint
        GameController.Instance.gameStateManager.checkpoint = collision.gameObject.GetInstanceID();
        GameController.Instance.gameStateManager.respawnPoint = _playerController.transform.position;

        // Recharge health and battery
        playerHealth.HealthUp(100);
        gameObject.GetComponent<BatteryManager>().Activate();

        // Play audio if you hit a checkpoint with default layer
        if (collision.gameObject.layer.Equals(0))
        {
            GameController.Instance.audioManager.checkpoint.Play();
        }
    }

    /// <summary>
    /// Lose health on contacts with hazards.
    /// Respawn character if no health remains.
    /// </summary>
    public void GetHurt(int dmg)
    {
        StartCoroutine(_playerController.interruptMagnet());
        playerHealth.HealthDown(dmg);
        if (playerHealth.playerHealth <= 0)
        {
            // start the warping animation & reset player's heath & battery
            StartCoroutine(GameController.Instance.gameStateManager.Warp());
        }
    }
}
