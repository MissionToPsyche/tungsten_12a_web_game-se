using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the behavior of the Thruster tool
/// </summary>
public class ThrusterManager : ToolManager
{
    //Private Variables
    private float _thrusterForce;

    public void Initialize(PlayerManagement playerManagement)
    {
        toolName = "Thruster";
        _playerManagement = playerManagement;
        _thrusterForce = 0.9f;
    }
    
    /// <summary>
    /// Activates the thruster applying a vertical force
    /// </summary>
    /// <param name="playerCharacter"></param>
    public void ActivateThruster()
    {
        _playerManagement.playerCharacter.velocity += new Vector2(0f, _thrusterForce * Time.deltaTime * 10f); 
    }

    /// <summary>
    /// Increases the thruster force when called
    /// </summary>
    public override void Modify()
    {
        _thrusterForce += 1.0f;
    }
}