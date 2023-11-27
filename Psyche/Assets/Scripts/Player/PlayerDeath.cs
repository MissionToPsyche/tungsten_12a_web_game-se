/** 
Description: player death script
Author: mcmyers4, blopezro, dnguye99
Version: 20231109
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player death script to handle hazard interactions and respawn attempts.
/// </summary>
public class PlayerDeath : MonoBehaviour {
    
    private Vector3 respawnPoint;         //Respawn location
    private Vector3 startPoint;           //Initial character location when level first begins
    public PlayerHealth playerHealth;     //Initial player health
    public BatteryManager batteryManager; //Initial battery
    public HashSet<int> reachedCheckpoints = new HashSet<int>(); //Stores unique IDs of checkpoints

    //Initialize respawn point and set starting location
    private void Start() {
        startPoint = transform.position;
        respawnPoint = transform.position;
    }

    //Player touches hazard object
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Hazard")) {
            GetHurt();
        }
    }

    //Updates respawn location once a checkpoint has been touched
    //Additionally recharges health and battery
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

    //Respawn character to last checkpoint when player gets hurt
    public void GetHurt() {
        //Debug.Log("Ouch!");
        playerHealth.HealthDown(1);
        if (playerHealth.playerHealth == 0) {
            //Debug.Log("Game should rest to checkpoint here.....");
        }

        //start the warping animation
        StartCoroutine(Warp());
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

        //tell the PlayerMovement script the player should be warping
        gameObject.GetComponent<PlayerMovement>().setWarp();

        //wait for the animation to be completed
        yield return new WaitForSeconds(1.2f);

        //check if the player is at the starting point
        if (startPoint.Equals(respawnPoint))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //move the player to their respawn point
        transform.position = respawnPoint;

        //tell the PlayerMovement script the player is no longer warping
        gameObject.GetComponent<PlayerMovement>().setWarp();

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
    }
}
