using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster_State : BaseState
{
    /// <summary>
    /// Defines the tracked objects in the scene
    /// -- Purpose is to reduce the potential for errors in object identification
    /// -- Identified as `stort` (i16) for ease of use -- will use `-1` for errorneous values
    /// </summary>
    public enum SceneObject : short
    {
        THRUSTER        = 0,
        CHECKPOINT_1    = 1,
        CHECKPOINT_2    = 2,
        CHECKPOINT_3    = 3,
        COPPER_1        = 4,
        GOLD_1          = 5,
        GOLD_2          = 6,
        GOLD_3          = 7,
        TUNGSTEN_1      = 8,

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
            "thruster"              => (short)SceneObject.THRUSTER,
            "checkpoint 1"          => (short)SceneObject.CHECKPOINT_1,
            "checkpoint 2"          => (short)SceneObject.CHECKPOINT_2,
            "checkpoint 3"          => (short)SceneObject.CHECKPOINT_3,
            "element_copper 1"      => (short)SceneObject.COPPER_1,
            "element_gold 1"        => (short)SceneObject.GOLD_1,
            "element_gold 2"        => (short)SceneObject.GOLD_2,
            "element_gold 3"        => (short)SceneObject.GOLD_3,
            "element_tungsten 1"    => (short)SceneObject.TUNGSTEN_1,

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
            (short)SceneObject.THRUSTER     => "Thruster",
            (short)SceneObject.CHECKPOINT_1 => "Checkpoint 1",
            (short)SceneObject.CHECKPOINT_2 => "Checkpoint 2",
            (short)SceneObject.CHECKPOINT_3 => "Checkpoint 3",
            (short)SceneObject.COPPER_1     => "Element_Copper 1",
            (short)SceneObject.GOLD_1       => "Element_Gold 1",
            (short)SceneObject.GOLD_2       => "Element_Gold 2",
            (short)SceneObject.GOLD_3       => "Element_Gold 3",
            (short)SceneObject.TUNGSTEN_1   => "Element_Tungsten 1",

            (short)SceneObject.CHECKPOINT   => "Checkpoint",
            _                               => null,
        };
    }

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Thruster_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        _defaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.THRUSTER,      true }, // 'true' for available
            { (short)SceneObject.CHECKPOINT_1,  true }, // `false` for not activated
            { (short)SceneObject.CHECKPOINT_2,  true },
            { (short)SceneObject.CHECKPOINT_3,  true },
            { (short)SceneObject.COPPER_1,      true },
            { (short)SceneObject.GOLD_1,        true },
            { (short)SceneObject.GOLD_2,        true },
            { (short)SceneObject.GOLD_3,        true },
            { (short)SceneObject.TUNGSTEN_1,    true },
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
