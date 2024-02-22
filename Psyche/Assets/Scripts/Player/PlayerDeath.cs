/*
 * Description: Player Loss and Reset
 * Authors: mcmyers4, blopezro, dnguye99asu
 * Version: 20240130
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player death script to handle hazard interactions and respawn attempts.
/// </summary>
public class PlayerDeath : MonoBehaviour {
    private PlayerController _playerController;
    private Vector3 respawnPoint;         //Respawn location
    private Vector3 startPoint;           //Initial character location when level first begins
    public PlayerHealth playerHealth;     //Initial player health
    public BatteryManager batteryManager; //Initial battery
    public HashSet<int> reachedCheckpoints = new HashSet<int>(); //Stores unique IDs of checkpoints


    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        startPoint = transform.position;
        respawnPoint = transform.position;
    }

    /// <summary>
    /// Initialize respawn point and set starting location.
    /// </summary>
    private void Start() {
        startPoint = transform.position;
        respawnPoint = transform.position;
    }

    /// <summary>
    /// Player touches hazard object.
    /// </summary>
    /// <param name="collision"></param>
    public void Hazard(Collision2D collision)
    {
        ApplyKickback(collision);

        GetHurt(1);
        GameController.Instance.audioManager.playerHurt.Play();
    }
    
    /// <summary>
    /// When player touches spikes
    /// </summary>
    public void Spikes()
    {
        GetHurt(playerHealth.playerHealth);
    }

    /// <summary>
    /// Applies a kickback force to the player character
    /// </summary>
    private void ApplyKickback(Collision2D collision) {
        // calculate kickback direction
        // left up or right up, TODO: base it off collision
        Vector2 kickbackDirection;
        if (PlayerController.Instance.playerMovement._isFacingRight) {
            kickbackDirection = new Vector2(-5f, 5f).normalized;
        } else {
            kickbackDirection = new Vector2(5f, 5f).normalized;
        }

        // set force and apply
        //Debug.Log($"Before kickback, velocity: {PlayerController.Instance.playerCharacter.velocity}");
        PlayerController.Instance.inputBlocked = true;
        float kickbackForce = 5f;        
        PlayerController.Instance.playerCharacter.AddForce(kickbackDirection * kickbackForce, ForceMode2D.Impulse);
        // TODO: Simulate horizontal key press here as horizontal force is not being applied   
        PlayerController.Instance.inputBlocked = false;
        //Debug.Log($"After kickback, velocity: {PlayerController.Instance.playerCharacter.velocity}");
    }

    /// <summary>
    /// Updates respawn location once a checkpoint has been touched.
    /// Additionally recharges health and battery.
    /// </summary>
    /// <param name="collision"></param>
    public void Checkpoint(Collider2D collision) {
        if (collision.gameObject.CompareTag("Checkpoint")) {
            //ID of the checkpoint
            int checkpointID = collision.gameObject.GetInstanceID();
            //Check if this checkpoint has been reached before
            if (reachedCheckpoints.Contains(checkpointID)) {
                //Debug.Log("Already reached this checkpoint!");
            } else {
                //Add the checkpoint ID to the HashSet
                reachedCheckpoints.Add(checkpointID);
                respawnPoint = transform.position;
                //Debug.Log("#### Checkpoint captured: " + respawnPoint + " ####");
                //Debug.Log("Checkpoint ID: " + checkpointID);
                //Recharge health and battery
                playerHealth.HealthUp(100);
                gameObject.GetComponent<BatteryManager>().Activate();
            }
        }
    }

    /// <summary>
    /// Lose health on contacts with hazards.
    /// Respawn character if no health remains.
    /// </summary>
    public void GetHurt(int dmg) {
        //Debug.Log("Ouch!");
        playerHealth.HealthDown(dmg);
        if (playerHealth.playerHealth <= 0) {
            //Debug.Log("Game should rest to checkpoint here.....");

            //start the warping animation & reset player's heath & battery
            StartCoroutine(Warp());
        }
    }

    /// <summary>
    /// This co-routine forces the game to wait for the player's warping
    /// animation to complete before continuing on.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Warp() {
        //block player controls
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.beingWarped = true;

        //wait for the animation to be completed
        yield return new WaitForSeconds(1.2f);

        //check if the player is at the starting point
        if (startPoint.Equals(respawnPoint)) {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            //changed so that camera bounds would load on player repawn
            StartCoroutine(GameController.Instance.sceneTransitionManager.CheckTransition(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
        }

        //move the player to their respawn point
        transform.position = respawnPoint;

        playerHealth.HealthUp(100);
        gameObject.GetComponent<BatteryManager>().Activate();

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
        PlayerController.Instance.beingWarped = false;
    }
}
