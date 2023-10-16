using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the behavior of the Thruster tool
/// </summary>
public class Thruster : MonoBehaviour
{
    public void activateThruster(Rigidbody2D playerCharacter)
    {
        if (Input.GetButton("Jump"))
            return;
        //TODO
    }
}
