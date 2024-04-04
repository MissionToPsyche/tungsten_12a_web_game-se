using System.Collections.Generic;
using UnityEngine;

public class Combo2_State : BaseState
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
        TUNGSTEN_1      = 2,
        GOLD_1          = 3,
        GOLD_2          = 4,
        GOLD_3          = 5,
        GOLD_4          = 6,
        NICKEL_1        = 7,

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
            "element_tungsten 1"    => (short)SceneObject.TUNGSTEN_1,
            "element_gold 1"        => (short)SceneObject.GOLD_1,
            "element_gold 2"        => (short)SceneObject.GOLD_2,
            "element_gold 3"        => (short)SceneObject.GOLD_3,
            "element_gold 4"        => (short)SceneObject.GOLD_4,
            "element_nickel 1"      => (short)SceneObject.NICKEL_1,

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
            (short)SceneObject.TUNGSTEN_1   => "Element_Tungsten 1",
            (short)SceneObject.GOLD_1       => "Element_Gold 1",
            (short)SceneObject.GOLD_2       => "Element_Gold 2",
            (short)SceneObject.GOLD_3       => "Element_Gold 3",
            (short)SceneObject.GOLD_4       => "Element_Gold 4",
            (short)SceneObject.NICKEL_1     => "Element_Nickel 1",

            (short)SceneObject.CHECKPOINT   => "Checkpoint",
            _                               => null,
        };
    }

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Combo2_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.CHECKPOINT_1, false }, // 'true' for available
            { (short)SceneObject.CHECKPOINT_2, false }, // `true` for activate-able
            { (short)SceneObject.TUNGSTEN_1,   true },
            { (short)SceneObject.GOLD_1,       true },
            { (short)SceneObject.GOLD_2,       true },
            { (short)SceneObject.GOLD_3,       true },
            { (short)SceneObject.GOLD_4,       true },
            { (short)SceneObject.NICKEL_1,     true },
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

                case (short)SceneObject.GOLD_2:
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

                case (short)SceneObject.GOLD_3:
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
                case (short)SceneObject.GOLD_4:
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

                default:
                    break;
            }
        }
    }
}
