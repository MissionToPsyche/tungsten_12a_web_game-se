using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public void handleMovement(Rigidbody2D playerCharacter, bool isGrounded)
    {        
        //Horizontal Movement
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        playerCharacter.velocity = new Vector2(horizontalDirection * 7f, playerCharacter.velocity.y);

        //Vertical "jump" only if player is on the ground
        if (isGrounded && Input.GetButtonDown("Jump"))
            playerCharacter.velocity = new Vector2(playerCharacter.velocity.x, 7f);
    }
}
