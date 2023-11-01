using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the behavior of the Thruster tool
/// </summary>
public class Thruster : MonoBehaviour
{
    //Private Variables
    private PlayerManagement _playerManagement;

    //Public Variables
    public float thrusterForce;

    /// <summary>
    /// Initializes the script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        _playerManagement = playerManagement;
        thrusterForce = 0.9f;
    }
    
    /// <summary>
    /// Activates the thruster applying a vertical force
    /// </summary>
    /// <param name="playerCharacter"></param>
    public void ActivateThruster()
    {
        _playerManagement.playerCharacter.velocity += new Vector2(0f, thrusterForce * Time.deltaTime * 10f); 
    }
}
