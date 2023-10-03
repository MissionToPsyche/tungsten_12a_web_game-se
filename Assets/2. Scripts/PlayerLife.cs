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

    // Start is called before the first frame update
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
            //rb2d.bodyType = RigidbodyType2D.Static;
            //animat.SetTrigger("death");
            //rb2d.bodyType = RigidbodyType2D.Static;
            //transform.position = respawnPoint;
            //Die();
        }
        //transform.position = respawnPoint;
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
        //rb2d.bodyType = RigidbodyType2D.Static;
        //transform.position = respawnPoint;
    }

    private void RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        animat.SetTrigger("respawn");
        transform.position = respawnPoint;
        //animat.SetTrigger("respawn");
        //rb2d.bodyType = RigidbodyType2D.Kinematic;
    }
}
