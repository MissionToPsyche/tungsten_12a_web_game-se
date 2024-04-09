/*
 * Description: Character Movement
 * Authors: joshbenn, blopezro, mcmyers4, jmolive8, dnguye99asu
 * Version: 20240404
 */

using UnityEngine;

/// <summary>
/// PlayerMovement class for handling the physical movements and related animations of the player character
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private variables
    private PlayerController PlayerManagement;
    private Animator Animator;
    private SpriteRenderer SpriteRenderer;
    private float XAxis;
    private string CurrentAnimation;
    private string NewAnimation;
    private bool FlipSprite;
    private Vector2 WalkVelocity;

    // Public variables
    public bool IsFacingRight;

    // Animation states
    const string PLAYER_IDLE = "player_idle";
    const string PLAYER_WALK = "player_walk";
    const string PLAYER_JUMP = "player_jump";
    const string PLAYER_THRUSTER = "player_thruster";
    const string PLAYER_WARP = "player_warp";
    const string ENTERING_CAVE = "player_entering_cave";

    /// <summary>
    /// Initializes the script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerController playerManagement)
    {
        PlayerManagement = playerManagement;
        Animator = GetComponentInChildren<Animator>();
        SpriteRenderer = Animator.gameObject.GetComponent<SpriteRenderer>();
    }

    //=========================================== Movement and Animation =========================================

    /// <summary>
    /// Handles the movement and animations of the player.
    /// </summary>
    public void HandleMovement(bool usingThruster, bool beingWarped, bool enteringCave, bool exitingCave)
    {
        FlipSprite = true;

        // warping takes precedent over every other animation, and blocks players movement
        if (beingWarped)
        {
            PlayerManagement.playerCharacter.velocity = new Vector2(0,0);
            NewAnimation = PLAYER_WARP;
        }
        else if (enteringCave)
        {
            PlayerManagement.playerCharacter.velocity = new Vector2(0, 0);
            NewAnimation = ENTERING_CAVE;
        }
        else if (exitingCave)
        {
            PlayerManagement.playerCharacter.velocity = new Vector2(0, 0);
            NewAnimation = PLAYER_WALK;
        }
        // checks for the player's behavior to determine which animation to play
        else
        {
            // horizontal movement
            if (!PlayerManagement.inputBlocked)
            {
                XAxis = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                XAxis = 0;
            }

            WalkVelocity = new Vector2(XAxis * 7f, PlayerManagement.playerCharacter.velocity.y);
            PlayerManagement.playerCharacter.velocity = WalkVelocity;

            // if the player is grounded, then either the walk animation or idle animation will play
            if (PlayerManagement.isGrounded && !PlayerManagement.beingPulled)
            {
                // if the player is moving left or right
                if (XAxis != 0)
                {
                    NewAnimation = PLAYER_WALK;
                }
                // if the player is not moving
                else
                {
                    NewAnimation = PLAYER_IDLE;
                }
            }
            // if the player is in the air
            else
            {
                if (usingThruster)
                {
                    NewAnimation = PLAYER_THRUSTER;
                }
                else
                {
                    NewAnimation = PLAYER_JUMP;
                }

            }

            // vertical "jump" only if player is on the ground
            if (!PlayerManagement.inputBlocked && !PlayerManagement.beingPulled
                && PlayerManagement.isGrounded && Input.GetButtonDown("Jump"))
            {
                PlayerManagement.playerCharacter.velocity = 
                    new Vector2(PlayerManagement.playerCharacter.velocity.x, 7f);
                GameController.Instance.AudioManager.playerJump.Play();
            }
        }

        // checks the direction the player is moving
        if (FlipSprite)
        {
            if (XAxis < 0) // if moving right
            {
                IsFacingRight = false;
            }
            else if (XAxis > 0) // if moving left
            {
                IsFacingRight = true;
            }
        }

        // flips the sprite based on direction the character is facing
        if (IsFacingRight)
        {
            SpriteRenderer.flipX = false;
        }
        else
        {
            SpriteRenderer.flipX = true;
        }

        // if the currentAnimation that is playing is the same animation that would be playing,
        // does nothing to ensure animation does not restart
        if (CurrentAnimation == NewAnimation)
        {
            return;
        }
        else
        {
            // play the new animation
            Animator.Play(NewAnimation);
            if (NewAnimation.Equals(PLAYER_THRUSTER))
            {
                GameController.Instance.AudioManager.toolThrusters.Play();
            }
            else
            {
                GameController.Instance.AudioManager.toolThrusters.Stop();
            }

            // set the current animation state
            CurrentAnimation = NewAnimation;
        }

    }
}

