using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test integration for a gamestate class to manage the state of a scene
/// </summary>
public class Landing_SceneState : BaseState
{
    /// <summary>
    /// Defines the tracked objects in the scene
    /// -- Purpose is to reduce the potential for errors in object identification
    /// -- Identified as `stort` (i16) for ease of use -- will use `-1` for errorneous values
    /// </summary>
    public enum SceneObject : short
    {
        SOLARPANEL = 0,
        CHECKPOINT_1 = 1,
        COPPER_1 = 2,
        GOLD_1 = 3,
        IRON_1 = 4,
        NICKEL_1 = 5,
        TUNGSTEN_1 = 6,

        CHECKPOINT = 99,
    }

    /// <summary>
    /// Matches the input object string with its enum variant -- useful for consistency
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "solarpanel"            => (short)SceneObject.SOLARPANEL,
            "checkpoint 1"          => (short)SceneObject.CHECKPOINT_1,
            "element_copper 1"      => (short)SceneObject.COPPER_1,
            "element_gold 1"        => (short)SceneObject.GOLD_1,
            "element_iron 1"        => (short)SceneObject.IRON_1,
            "element_nickel 1"      => (short)SceneObject.NICKEL_1,
            "element_tungsten 1"    => (short)SceneObject.TUNGSTEN_1,
            "checkpoint"            => (short)SceneObject.CHECKPOINT,
            _                       => -1,
        };
    }

    public override string Match(short obj)
    {
        return obj switch
        {
            (short)SceneObject.SOLARPANEL   => "SolarPanel",
            (short)SceneObject.CHECKPOINT_1 => "Checkpoint 1",
            (short)SceneObject.COPPER_1     => "Element_Copper 1",
            (short)SceneObject.GOLD_1       => "Element_Gold 1",
            (short)SceneObject.IRON_1       => "Element_Iron 1",
            (short)SceneObject.NICKEL_1     => "Element_Nickel 1",
            (short)SceneObject.TUNGSTEN_1   => "Element_Tungsten 1",
            (short)SceneObject.CHECKPOINT   => "Checkpoint",
            _ => null,
        };
    }

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Landing_SceneState()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        _defaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.SOLARPANEL, true }, // 'true' for available
            { (short)SceneObject.CHECKPOINT_1, true }, // `false` for not activated
            { (short)SceneObject.COPPER_1, true },
            { (short)SceneObject.GOLD_1, true },
            { (short)SceneObject.IRON_1, true },
            { (short)SceneObject.NICKEL_1, true },
            { (short)SceneObject.TUNGSTEN_1, true },
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
                case (short)SceneObject.SOLARPANEL:
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


                case (short)SceneObject.CHECKPOINT_1:
                    {
                        var value = (bool)pair.Value;
                        var targetObject = GameObject.Find(objectName);
                    }
                    break;


                case (short)SceneObject.COPPER_1:
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


                case (short)SceneObject.GOLD_1:
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


                case (short)SceneObject.IRON_1:
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


                case (short)SceneObject.NICKEL_1:
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


                case (short)SceneObject.TUNGSTEN_1:
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

                default:
                    break;
            }
        }
    }
}

