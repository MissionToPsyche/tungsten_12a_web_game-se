using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

///<summary>
///PlayerMovement is a script which makes changes to the player's current position, velocity, and behavior
///</summary>
public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float xAxis;
    private float yAxis; //unused for now. may use for jumping later
    private string currentAnimation;
    private string newAnimation;
    private bool isFacingRight;
    private bool flipSprite;
    private Vector2 walkVelocity;

    //Animation states
    const string PLAYER_IDLE = "player_idle";
    const string PLAYER_WALK = "player_walk";
    const string PLAYER_JUMP = "player_jump";
    const string PLAYER_THRUSTER = "player_thruster"; //unused for now. will add later

    /// <summary>
    /// Handles the movement and animations of the player.
    /// </summary>
    /// <param name="playerCharacter"></param>
    /// <param name="isGrounded"></param>
    public void handleMovement(Rigidbody2D playerCharacter, bool isGrounded, AudioManager audioManager, bool usingThruster)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flipSprite = true;

        //Horizontal Movement
        xAxis = Input.GetAxisRaw("Horizontal");
        walkVelocity = new Vector2(xAxis * 7f, playerCharacter.velocity.y);
        playerCharacter.velocity = walkVelocity;

        //if the player is grounded, then either the walk animation or idle animation will play
        if (isGrounded)
        {
            //if the player is moving left or right
            if (xAxis != 0) 
            {
                newAnimation = PLAYER_WALK;
            }

            //if the player is not moving
            else
            {
                newAnimation = PLAYER_IDLE;
            }
        }

        //if the player is in the air; will be edited later to add thruster option
        else
        {
            if (usingThruster)
            {
                newAnimation = PLAYER_THRUSTER;
            }

            else
            {
                newAnimation = PLAYER_JUMP;
            }
            
        }

        //Vertical "jump" only if player is on the ground
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerCharacter.velocity = new Vector2(playerCharacter.velocity.x, 7f);
            audioManager.PlayPlayerJump(); // play jump sound
        }

        //checks the direction the player is moving
        if (flipSprite)
        {
            if (xAxis < 0) //if moving right
            {
                isFacingRight = false;
            }
            else if (xAxis > 0) //if moving left
            {
                isFacingRight = true;
            }
            else
            { 
                //nothing. whichever way it's currently facing will be the same
            }
        }

        //flips the sprite based on direction the character is facing
        if (isFacingRight)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        //if the currentAnimation that is playing is the same animation that would be playing, does nothing
        //to ensure animation does not restart
        if (currentAnimation == newAnimation)
        {
            return;
        }
        else
        {
            //play the new animation
            animator.Play(newAnimation);

            //set the current animation state
            currentAnimation = newAnimation;
        }

    }
}
