using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the behavior of the Thruster tool
/// </summary>
public class Thruster : MonoBehaviour
{
    public float thrusterForce = 0.9f;
    public void activateThruster(Rigidbody2D playerCharacter)
    {
        if (Input.GetButton("Jump"))
            playerCharacter.velocity += new Vector2(0f, thrusterForce * Time.deltaTime * 10f); 
    }
}
 