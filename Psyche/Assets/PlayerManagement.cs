using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Player Management script controls how the player interacts with the system and various components.
/// </summary>
public class PlayerManagement : MonoBehaviour
{
    //Create the playercharacter assignment
    private Rigidbody2D playerCharacter;

    //Set up environmental checks
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    //Booleans for environmental checks
    private bool isGrounded;

    //Booleans for the various tools
    private bool hasThrusters;
    private bool hasImager;
    private bool hasMagnetometer;
    private bool hasSpectrometer;

    // Start is called before the first frame update
    void Start()
    {
        //Assign the playerCharacter to its in-game object
        playerCharacter = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //Horizontal Movement
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        playerCharacter.velocity = new Vector2(horizontalDirection * 7f, playerCharacter.velocity.y);

        //Vertical "jump" only if player is on the ground
        if (isGrounded && Input.GetButtonDown("Jump"))
            playerCharacter.velocity = new Vector2(playerCharacter.velocity.x, 7f);

        //Call the requisite tool scripts here:
        //Thruster
        if(hasThrusters && !isGrounded && Input.GetButton("Jump"))
            return; //Thruster script call
        //Imager
        if (hasImager)
            return; //Imager script call
        //Spectrometer
        if (hasSpectrometer)
            return; //Spectrometer script call
        //Magnometer
        if (hasMagnetometer)
            return; //Magnometer script call

    }
}
