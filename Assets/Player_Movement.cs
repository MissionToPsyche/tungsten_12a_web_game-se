using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D character;

    public Transform groundCheck;  
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private bool isGrounded;  

    // Start is called before the first frame update
    private void Start()
    {
        character = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        // Horizontal Movement
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        character.velocity = new Vector2(horizontalDirection * 7f, character.velocity.y);

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            character.velocity = new Vector2(character.velocity.x, 10f);
        }//Activate Thruster
        else if (!isGrounded && Input.GetButton("Jump"))
        {
            character.velocity += new Vector2(0f, 2f * Time.deltaTime * 10f);
        }
    }
}
