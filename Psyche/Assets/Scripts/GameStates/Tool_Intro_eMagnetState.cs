using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


/// <summary>
/// Test integration for a gamestate class to manage the state of a scene
/// </summary>
public class Tool_Intro_eMagnetState : BaseState
{
    /// <summary>
    /// Defines the tracked objects in the scene
    /// -- Purpose is to reduce the potential for errors in object identification
    /// -- Identified as `stort` (i16) for ease of use -- will use `-1` for errorneous values
    /// </summary>
    public enum SceneObject : short
    {
        ELECTRO_MAGNET_PICKUP_VARIANT = 0,
        CHECKPOINT_0 = 1,
        CHECKPOINT_1 = 2,
        IRON_MOVEABLE = 3,
        BASIC_MOVING_HAZARD = 4,
    }

    /// <summary>
    /// Matches the input object string with its enum variant -- useful for consistency
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected override short Match(string obj)
    {
        switch (obj)
        {
            case "Electromagnet Pickup Variant": return (short)SceneObject.ELECTRO_MAGNET_PICKUP_VARIANT;
            case "Checkpoint 0": return (short)SceneObject.CHECKPOINT_0;
            case "Checkpoint 1": return (short)SceneObject.CHECKPOINT_1;
            case "Iron Moveable": return (short)SceneObject.IRON_MOVEABLE;
            case "Basic Moving Hazard": return (short)SceneObject.BASIC_MOVING_HAZARD;
        }
        return -1;
    }

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Tool_Intro_eMagnetState()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        _defaultState = new Dictionary<short, object>
        {
                { (short)SceneObject.ELECTRO_MAGNET_PICKUP_VARIANT, true }, // 'true' for available
                { (short)SceneObject.CHECKPOINT_0, true },  // `true` for activate-able
                { (short)SceneObject.CHECKPOINT_1, true },
                { (short)SceneObject.IRON_MOVEABLE, new Vector3(56.69f, -3.1f, -1.0f) },
                { (short)SceneObject.BASIC_MOVING_HAZARD, new Vector3(29.24f, 3.48f, 0f) }
        };
        LoadDefaultState();
        SaveState();
    }
}
