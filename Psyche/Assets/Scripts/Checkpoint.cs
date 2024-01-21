/**
 * description: checkpoint script to ensure animation
 * Author: dnguye99
 * version 20231125
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //Private variables for animation
    private Animator animator;
    private bool isSpinning;
    const string STATIC = "checkpoint-solarpanel-static";
    const string SPIN = "checkpoint-solarpanel-spin";

    //PlayerDeath script related to checkpoints
    private PlayerDeath playerDeath;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(STATIC);
        isSpinning = false;
        playerDeath = GetComponent<PlayerDeath>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //checks if the checkpoint is not spinning
            if (!isSpinning)
            {
                animator.Play(SPIN);
                isSpinning = true;
            }
        }
    }
}
