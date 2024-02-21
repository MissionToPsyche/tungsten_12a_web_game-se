using System;
using System.Collections.Generic;
using UnityEngine;


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
    public override short Match(string obj)
    {
        switch (obj)
        {
            case "ElectroMagnet": return (short)SceneObject.ELECTRO_MAGNET_PICKUP_VARIANT;
            case "Checkpoint 0": return (short)SceneObject.CHECKPOINT_0;
            case "Checkpoint 1": return (short)SceneObject.CHECKPOINT_1;
            case "Iron Moveable": return (short)SceneObject.IRON_MOVEABLE;
            case "Basic Moving Hazard": return (short)SceneObject.BASIC_MOVING_HAZARD;
        }
        return -1;
    }

    public override string Match(short obj) {
        switch (obj)
        {
            case (short)SceneObject.ELECTRO_MAGNET_PICKUP_VARIANT: return "ElectroMagnet";
            case (short)SceneObject.CHECKPOINT_0: return "Checkpoint 0";               
            case (short)SceneObject.CHECKPOINT_1: return "Checkpoint 1";               
            case (short)SceneObject.IRON_MOVEABLE: return "Iron Moveable";              
            case (short)SceneObject.BASIC_MOVING_HAZARD: return "Basic Moving Hazard";
        }
        return "";
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

    /// <summary>
    /// Loads the specific state for this scene
    /// </summary>
    public override void LoadState()
    {
        
        foreach (var pair in _savedState)
        {
            string objectName = Match(pair.Key);
            switch (pair.Key)
            {
                case (short)SceneObject.ELECTRO_MAGNET_PICKUP_VARIANT:
                    { // Specifying scope for use of `var value` && `var targetObject`
                        var value = (bool)pair.Value;
                        
                        // Remove the object if it's already been picked up
                        if (!value)
                        {
                            var targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                Debug.LogError($"Object {objectName} does not exist");
                                return;
                            }

                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.CHECKPOINT_0:
                    {
                        var value = (bool)pair.Value;
                        var targetObject = GameObject.Find(objectName);
                    }
                    break;
                case (short)SceneObject.CHECKPOINT_1:
                    {
                        var value = (bool)pair.Value;
                        var targetObject = GameObject.Find(objectName);
                    }
                    break;
                case (short)SceneObject.IRON_MOVEABLE:
                    {
                        var value = (Vector3)pair.Value;
                        var targetObject = GameObject.Find(objectName);
                        targetObject.transform.position = value;
                    }
                    break;
                case (short)SceneObject.BASIC_MOVING_HAZARD:
                    {
                        var value = (Vector3)pair.Value;
                        var targetObject = GameObject.Find(objectName);
                        targetObject.transform.position = value;
                    }
                    break;
            }
        }
    }
}
