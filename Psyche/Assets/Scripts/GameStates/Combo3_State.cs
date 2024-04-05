using System.Collections.Generic;
using UnityEngine;

public class Combo3_State : BaseState
{
    /// <summary>
    /// Defines the tracked objects in the scene
    /// -- Purpose is to reduce the potential for errors in object identification
    /// -- Identified as `stort` (i16) for ease of use -- will use `-1` for errorneous values
    /// </summary>
    public enum SceneObject : short
    {
        CHECKPOINT_1    = 0,
        CHECKPOINT_2    = 1,
        CHECKPOINT_3    = 2,
        TUNGSTEN_1      = 3,
        IRON_1          = 4,
        IRON_2          = 5,
        IRON_3          = 6,
        IRON_4          = 7,

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
            "checkpoint 1"          => (short)SceneObject.CHECKPOINT_1,
            "checkpoint 2"          => (short)SceneObject.CHECKPOINT_2,
            "checkpoint 3"          => (short)SceneObject.CHECKPOINT_3,
            "element_tungsten 1"    => (short)SceneObject.TUNGSTEN_1,
            "element_iron 1"        => (short)SceneObject.IRON_1,
            "element_iron 2"        => (short)SceneObject.IRON_2,
            "element_iron 3"        => (short)SceneObject.IRON_3,
            "element_iron 4"        => (short)SceneObject.IRON_4,

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
            (short)SceneObject.CHECKPOINT_1 => "Checkpoint 1",
            (short)SceneObject.CHECKPOINT_2 => "Checkpoint 2",
            (short)SceneObject.CHECKPOINT_3 => "Checkpoint 3",
            (short)SceneObject.TUNGSTEN_1   => "Element_Tungsten 1",
            (short)SceneObject.IRON_1       => "Element_Iron 1",
            (short)SceneObject.IRON_2       => "Element_Iron 2",
            (short)SceneObject.IRON_3       => "Element_Iron 3",
            (short)SceneObject.IRON_4       => "Element_Iron 4",

            (short)SceneObject.CHECKPOINT   => "Checkpoint",
            _ => null,
        };
    }

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Combo3_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.CHECKPOINT_1, false }, // 'true' for available
            { (short)SceneObject.CHECKPOINT_2, false }, // `true` for activate-able
            { (short)SceneObject.CHECKPOINT_3, false },
            { (short)SceneObject.TUNGSTEN_1,   true },
            { (short)SceneObject.IRON_1,       true },
            { (short)SceneObject.IRON_2,       true },
            { (short)SceneObject.IRON_3,       true },
            { (short)SceneObject.IRON_4,       true },
        };
        LoadDefaultState();
        SaveState();
    }

    /// <summary>
    /// Loads the specific state for this scene
    /// </summary>
    public override void LoadState()
    {
        foreach (var pair in SavedState)
        {
            string objectName = Match(pair.Key);
            switch (pair.Key)
            {
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

                case (short)SceneObject.CHECKPOINT_2:
                    {
                        var value = (bool)pair.Value;
                        if (value)
                        {
                            var targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.CHECKPOINT_3:
                    {
                        var value = (bool)pair.Value;
                        if (value)
                        {
                            var targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
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

                case (short)SceneObject.IRON_2:
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

                case (short)SceneObject.IRON_3:
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

                case (short)SceneObject.IRON_4:
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
