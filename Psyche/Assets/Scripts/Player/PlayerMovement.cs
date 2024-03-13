/*
 * Description: Character Movement
 * Authors: joshbenn, blopezro, mcmyers4, jmolive8, dnguye99asu
 * Version: 20240119
 */

using Cinemachine;
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
    public bool _isFacingRight;
    private bool _flipSprite;
    private Vector2 _walkVelocity;
    /*//A Camera to bias right when player is looking right
    [SerializeField] private CinemachineVirtualCamera _virtualCameraR;
    //A Camera to bias left when player is looking left
    [SerializeField] private CinemachineVirtualCamera _virtualCameraL;
    //A Camera to bias lower right when player is in the air and looking right
    [SerializeField] private CinemachineVirtualCamera _virtualCameraLowR;
    //A Camera to bias lower left when player is in the air and looking left
    [SerializeField] private CinemachineVirtualCamera _virtualCameraLowL;*/

    //Animation states
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
        _playerManagement = playerManagement;
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = _animator.gameObject.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Handles the movement and animations of the player.
    /// </summary>
    public void handleMovement(bool usingThruster, bool beingWarped, bool enteringCave, bool exitingCave)
    {
        _flipSprite = true;

        // warping takes precedent over every other animation, and blocks players movement
        if (beingWarped)
        {
            _playerManagement.playerCharacter.velocity = new Vector2(0,0);
            _newAnimation = PLAYER_WARP;
        }

        else if (enteringCave)
        {
            _playerManagement.playerCharacter.velocity = new Vector2(0, 0);
            _newAnimation = ENTERING_CAVE;
        }

        else if (exitingCave)
        {
            _playerManagement.playerCharacter.velocity = new Vector2(0, 0);
            _newAnimation = PLAYER_WALK;
        }
        //checks for the player's behavior to determine which animation to play
        else
        {
            //Horizontal movement
            if (!_playerManagement.inputBlocked)
                _xAxis = Input.GetAxisRaw("Horizontal");
            else
                _xAxis = 0;
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
            if (!_playerManagement.inputBlocked && !_playerManagement.beingPulled && _playerManagement.isGrounded && Input.GetButtonDown("Jump"))
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
            //TODO need to set alpha faders for entering and exiting cave -Dhalia

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

        //Trying out camera bias solely on mouse aim

        /*/// <summary>
        /// Bias the camera in the direction the player is facing (left or right).
        /// Show more below the player if the player is in the air.
        /// </summary>
        if (_isFacingRight)
        {
            //Player is looking right and in the air.
            if (!_playerManagement.isGrounded)
            {
                _virtualCameraLowR.Priority = 10;
                _virtualCameraLowL.Priority = 0;
                _virtualCameraL.Priority = 0;
                _virtualCameraR.Priority = 0;
            }
            //Player is looking right and on the ground.
            else
            {
                _virtualCameraR.Priority = 10;
                _virtualCameraLowR.Priority = 0;
                _virtualCameraLowL.Priority = 0;
                _virtualCameraL.Priority = 0;
            }
        }
        else
        {
            //Player is looking left and in the air.
            if (!_playerManagement.isGrounded)
            {
                _virtualCameraLowL.Priority = 10;
                _virtualCameraLowR.Priority = 0;
                _virtualCameraL.Priority = 0;
                _virtualCameraR.Priority = 0;
            }
            //Player is looking right and on the ground.
            else
            {
                _virtualCameraL.Priority = 10;
                _virtualCameraLowR.Priority = 0;
                _virtualCameraLowL.Priority = 0;
                _virtualCameraR.Priority = 0;
            }
        }*/

    }
}

