/** 
Description: player movement script
Author: blopezro
Version: 20230930
**/

using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    public bool isFacingRight = true;

    // movement
    public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // audio
    [SerializeField] private AudioSource jumpSound;

    // update is called once per frame
    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpSound.Play();
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    // checks if the player is touching the ground
    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // changes the direction the player is facing 
    private void Flip() {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}
