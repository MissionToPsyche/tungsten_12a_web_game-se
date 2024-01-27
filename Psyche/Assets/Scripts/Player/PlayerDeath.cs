/*
 * Description: Player Loss and Reset
 * Authors: mcmyers4, blopezro, dnguye99asu
 * Version: 20240119
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player death script to handle hazard interactions and respawn attempts.
/// </summary>
public class PlayerDeath : MonoBehaviour {
    
    private Vector3 respawnPoint;         //Respawn location
    private Vector3 startPoint;           //Initial character location when level first begins
    public PlayerHealth playerHealth;     //Initial player health
    public BatteryManager batteryManager; //Initial battery
    public HashSet<int> reachedCheckpoints = new HashSet<int>(); //Stores unique IDs of checkpoints

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
    private void OnCollisionEnter2D(Collision2D collision) {
            //regular hazards do 1 damage
        if (collision.gameObject.CompareTag("Hazard")) {
            GetHurt(1);
        }

        //spikes do full damage
        else if (collision.gameObject.CompareTag("Spikes"))
        {
            GetHurt(playerHealth.playerHealth);
        }
    }

    /// <summary>
    /// Updates respawn location once a checkpoint has been touched.
    /// Additionally recharges health and battery.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) {
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

            //start the warping animation
            StartCoroutine(Warp());
            //reset the player's health to 5
            playerHealth.HealthUp(5);
        }
    }

    /// <summary>
    /// This co-routine forces the game to wait for the player's warping
    /// animation to complete before continuing on.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Warp()
    {
        //block player controls
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.beingWarped = true;

        //wait for the animation to be completed
        yield return new WaitForSeconds(1.2f);

        //check if the player is at the starting point
        if (startPoint.Equals(respawnPoint))
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            //changed so that camera bounds would load on player repawn
            ArrayList args = new ArrayList {
                "Game", "SceneManager", "PlayerController", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            };
            PlayerController.Instance.SendMessage(args);
        }

        //move the player to their respawn point
        transform.position = respawnPoint;

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
        PlayerController.Instance.beingWarped = false;
    }
}
