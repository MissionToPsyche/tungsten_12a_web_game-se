/** 
Description: player respawn script
Author: mcmyers4
Version: 20231004
**/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator animat;
    private Rigidbody2D rb2d;
    private Vector3 respawnPoint;

    private void Start()
    {
        animat = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            respawnPoint = transform.position;
        }
    }

    private void Die()
    {
        animat.SetTrigger("death");
    }

    private void RestartLevel()
    {
        animat.SetTrigger("respawn");
        transform.position = respawnPoint;
    }
}
