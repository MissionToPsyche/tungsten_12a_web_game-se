using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Integration for eMagnet state management for the scene
/// </summary>
public class eMagnet_State : BaseState
{
    /// <summary>
    /// Defines the tracked objects in the scene
    /// -- Purpose is to reduce the potential for errors in object identification
    /// -- Identified as `stort` (i16) for ease of use -- will use `-1` for errorneous values
    /// </summary>
    public enum SceneObject : short
    {
        ELECTROMAGNET   = 0,
        CHECKPOINT_0    = 1,
        CHECKPOINT_1    = 2,
        GOLD_1          = 3,
        GOLD_2          = 4,
        TUNGSTEN_1      = 5,
        FIRST_VEINS     = 6,

        CHECKPOINT      = 99,
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
            "electromagnet"         => (short)SceneObject.ELECTROMAGNET,
            "checkpoint 0"          => (short)SceneObject.CHECKPOINT_0,
            "checkpoint 1"          => (short)SceneObject.CHECKPOINT_1,
            "element_gold 1"        => (short)SceneObject.GOLD_1,
            "element_gold 2"        => (short)SceneObject.GOLD_2,
            "element_tungsten 1"    => (short)SceneObject.TUNGSTEN_1,
            "first_veins"           => (short)SceneObject.FIRST_VEINS,

            "checkpoint"            => (short)SceneObject.CHECKPOINT,
            _                       => -1,
        };
    }

    /// <summary>
    /// Matches the input object short (enum) with its string variant
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override string Match(short obj) 
    {
        return obj switch
        {
            (short)SceneObject.ELECTROMAGNET    => "ElectroMagnet",
            (short)SceneObject.CHECKPOINT_0     => "Checkpoint 0",
            (short)SceneObject.CHECKPOINT_1     => "Checkpoint 1",
            (short)SceneObject.GOLD_1           => "Element_Gold 1",
            (short)SceneObject.GOLD_2           => "Element_Gold 2",
            (short)SceneObject.TUNGSTEN_1       => "Element_Tungsten 1",
            (short)SceneObject.FIRST_VEINS      => "First_Veins",

            (short)SceneObject.CHECKPOINT       => "Checkpoint",
            _                                   => null,
        };
    }

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public eMagnet_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        _defaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.ELECTROMAGNET, true }, // 'true' for available
            { (short)SceneObject.CHECKPOINT_0,  false }, // `true` for activate-able
            { (short)SceneObject.CHECKPOINT_1,  false },
            { (short)SceneObject.GOLD_1,        true },
            { (short)SceneObject.GOLD_2,        true },
            { (short)SceneObject.TUNGSTEN_1,    true },
            { (short)SceneObject.FIRST_VEINS,   false },
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
                case (short)SceneObject.ELECTROMAGNET:
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
                        if (value)
                        {
                            var targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.CHECKPOINT_1:
                    {
                        var value = (bool)pair.Value;
                        if (value)
                        {
                            var targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.GOLD_1:
                    {
                        var value = (bool)pair.Value;
                        if (!value)
                        {
                            var targetObject = GameObject.Find(objectName);
                            if(targetObject == null)
                            {
                                Debug.LogError($"Object {objectName} does not exist");
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.GOLD_2:
                    {
                        var value = (bool)pair.Value;
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
                    {
                        var value = (bool)pair.Value;
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

                case (short)SceneObject.FIRST_VEINS:
                    {
                        var value = (bool)pair.Value;
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