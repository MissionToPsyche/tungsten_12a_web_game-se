using System.Collections;
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
        CHECKPOINT_0    = 0,
        CHECKPOINT_1    = 1,
        CHECKPOINT_2    = 2,
        TUNGSTEN_1      = 4,
        GOLD_1          = 5,
        GOLD_2          = 6,
        GOLD_3          = 7,
        GOLD_4          = 8,
        NICKEL_1        = 9,

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
            "checkpoint 0"          => (short)SceneObject.CHECKPOINT_0,
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
            (short)SceneObject.CHECKPOINT_0 => "Checkpoint 0",
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
        _defaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.CHECKPOINT_0, true },
            { (short)SceneObject.CHECKPOINT_1, true }, // 'true' for available
            { (short)SceneObject.CHECKPOINT_2, true }, // `true` for activate-able
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
        foreach (var pair in _savedState)
        {
            string objectName = Match(pair.Key);
            switch (pair.Key)
            {


                default:
                    break;
            }
        }
    }
}
