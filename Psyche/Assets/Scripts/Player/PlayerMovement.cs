/**
 * Description: makes changes to the player's current position, velocity, behavior, and animations.
 * Authors:
 * Version: 20231106
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

///<summary>
///PlayerMovement is a script which 
///</summary>
public class PlayerMovement : MonoBehaviour
{
    //Private Variables
    private PlayerController _playerManagement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private float _xAxis;
    private string _currentAnimation;
    private string _newAnimation;
    private bool _isFacingRight;
    private bool _flipSprite;
    private Vector2 _walkVelocity;

    //Animation states
    const string PLAYER_IDLE = "player_idle";
    const string PLAYER_WALK = "player_walk";
    const string PLAYER_JUMP = "player_jump";
    const string PLAYER_THRUSTER = "player_thruster";
    const string PLAYER_WARP = "player_warp";

    /// <summary>
    /// Initializes the script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerController playerManagement)
    {
        _playerManagement = playerManagement;
    }

    /// <summary>
    /// Handles the movement and animations of the player.
    /// </summary>
    /// <param name="playerCharacter"></param>
    /// <param name="isGrounded"></param>
    public void handleMovement(bool usingThruster, bool beingWarped)
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _flipSprite = true;

        // warping takes precedent over every other animation, and blocks players movement
        if (beingWarped)
        {
            _playerManagement.playerCharacter.velocity = new Vector2(0,0);
            _newAnimation = PLAYER_WARP;
        }

        //checks for the player's behavior to determine which animation to play
        else
        {
            //Horizontal movement
            _xAxis = Input.GetAxisRaw("Horizontal");
            _walkVelocity = new Vector2(_xAxis * 7f, _playerManagement.playerCharacter.velocity.y);
            _playerManagement.playerCharacter.velocity = _walkVelocity;

            //if the player is grounded, then either the walk animation or idle animation will play
            if (_playerManagement.isGrounded && !_playerManagement.beingPulled)
            {
                //if the player is moving left or right
                if (_xAxis != 0)
                {
                    _newAnimation = PLAYER_WALK;
                }

                //if the player is not moving
                else
                {
                    _newAnimation = PLAYER_IDLE;
                }
            }

            //if the player is in the air
            else
            {
                if (usingThruster)
                {
                    _newAnimation = PLAYER_THRUSTER;
                }

                else
                {
                    _newAnimation = PLAYER_JUMP;
                }

            }

            //Vertical "jump" only if player is on the ground
            if (_playerManagement.isGrounded && !_playerManagement.beingPulled && Input.GetButtonDown("Jump"))
            {
                _playerManagement.playerCharacter.velocity = new Vector2(_playerManagement.playerCharacter.velocity.x, 7f);
                GameController.Instance.audioManager.playerJump.Play();
            }

        }

        //checks the direction the player is moving
        if (_flipSprite)
        {
            if (_xAxis < 0) //if moving right
            {
                _isFacingRight = false;
            }
            else if (_xAxis > 0) //if moving left
            {
                _isFacingRight = true;
            }
            else
            { 
                //nothing. whichever way it's currently facing will be the same
            }
        }

        //flips the sprite based on direction the character is facing
        if (_isFacingRight)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }

        //if the currentAnimation that is playing is the same animation that would be playing, does nothing
        //to ensure animation does not restart
        if (_currentAnimation == _newAnimation)
        {
            return;
        }
        else
        {
            //play the new animation
            _animator.Play(_newAnimation);
            if (_newAnimation.Equals(PLAYER_THRUSTER)) {
                GameController.Instance.audioManager.toolThrusters.Play();
            } else {
                GameController.Instance.audioManager.toolThrusters.Stop();
            }

            //set the current animation state
            _currentAnimation = _newAnimation;
        }
    }
}
