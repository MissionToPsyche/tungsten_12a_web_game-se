using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Test integration for a gamestate class to manage the state of a scene
/// </summary>
public class Tool_Intro_eMagnetState : BaseState
{
    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Tool_Intro_eMagnetState()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        _defaultState = new Dictionary<string, object>
        {
                { "Electromagnet Pickup Variant", true }, // 'true' for available?
                { "Checkpoint 0", false },  // 'false' for not activated
                { "Checkpoint 1", false },
                { "Iron Moveable", new Vector3(56.69f, -3.1f, -1.0f) },
                { "Basic Moving Hazard", new Vector3(29.24f, 3.48f, 0f) }
        };
        LoadDefaultState();
        SaveState();
    }


    public override void LoadState()
    {
        // Do stuff to load the state here
    }
}
