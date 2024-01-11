using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test integration for a gamestate class to manage the state of a scene
/// </summary>
public class Tool_Intro_eMagnetState : BaseState
{
    /// <summary>
    /// Constructor for the gamestate
    /// </summary>
    public Tool_Intro_eMagnetState()
    {
        _objectStates = new Dictionary<string, object>()
        {
            { "Electromagnet Pickup Variant", false }, //Should it be 'false' for not picked up or 'true' for available?
            { "Checkpoint", false },  //'false' for not activated or 'true' for activated?
            { "Checkpoint (1)", false },
            { "Iron Movable", new Vector3(55.07f, -3.05f, -1.0f) },
        };
    }

    /// <summary>
    /// Holds the default game state for the scene, allowing for a state reset
    /// </summary>
    public void LoadDefaultGameState()
    {
        _objectStates["Electromagnet Pickup Variant"] = false;
        _objectStates["Checkpoint"] = false;
        _objectStates["Checkpoint (1)"] = false;
        _objectStates["Iron Movable"] = new Vector3(55.07f, -3.05f, -1.0f);
    }

    /// <summary>
    /// Sets the state of an object in the scene
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetState(string key, object value)
    {
        _objectStates[key] = value;
    }
}
