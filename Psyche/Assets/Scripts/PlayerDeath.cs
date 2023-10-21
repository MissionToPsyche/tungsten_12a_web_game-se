/** 
Description: player death script
Author: mcmyers4
Version: 20231018
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Player death script to handle hazard interactions and respawn attempts.
/// </summary>
public class PlayerDeath : MonoBehaviour
{
    //Respawn location
    private Vector3 respawnPoint;

    //Initial character location when level first begins
    private Vector3 startPoint;

    //Initialize respawn point and set starting location
    private void Start()
    {
        startPoint = transform.position;
        respawnPoint = transform.position;
    }

    //Player touches hazard object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    //Updates respawn location once a checkpoint has been touched
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            respawnPoint = transform.position;
        }
    }

    //Respawn character to last checkpoint when player dies
    private void Die()
    {
        if (startPoint.Equals(respawnPoint))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        transform.position = respawnPoint;
    }

}