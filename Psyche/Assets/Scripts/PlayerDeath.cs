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
    //Player touches hazard object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Die();
        }
    }

    //Restart level when player dies
    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
