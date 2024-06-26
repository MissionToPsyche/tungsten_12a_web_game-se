/**
 * description: checkpoint script to ensure animation
 * Author: dnguye99, JoshBenn
 * version 20240410
 */

using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Variables for animation
    private Animator Animator;
    public bool IsSpinning = false;
    const string STATIC = "checkpoint-solarpanel-static";
    const string SPIN = "checkpoint-solarpanel-spin";
    System.DateTime LastActivation;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Animator = GetComponent<Animator>();
        if (!IsSpinning)
        {
            Animator.Play(STATIC);
        }
        else
        {
            Spin();
        }
    }

    /// <summary>
    /// What to do when the player collides with the checkpoint
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Checks if the checkpoint is not spinning
            if (!IsSpinning)
            {
                IsSpinning = true;
                Spin();
            }
        }
    }

    /// <summary>
    /// Activates the animation for the checkpoint
    /// </summary>
    public void Spin()
    {
        Animator.Play(SPIN);
    }

}