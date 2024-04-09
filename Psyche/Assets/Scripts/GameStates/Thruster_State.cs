/*
 * Authors: JoshBenn
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives the <see cref="BaseState"/> class and implements it for Combo1 <see cref="GameStateManager.Scene.Thruster" />.
/// </summary>
public class Thruster_State : BaseState
{
    //========================================= Initialize/Updates/Destroy =========================================

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Thruster_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.Thruster,      true }, // 'true' for available
            { (short)SceneObject.Checkpoint1,  false }, // `false` for not activated
            { (short)SceneObject.Checkpoint2,  false },
            { (short)SceneObject.Checkpoint3,  false },
            { (short)SceneObject.Copper1,      true },
            { (short)SceneObject.Gold1,        true },
            { (short)SceneObject.Gold2,        true },
            { (short)SceneObject.Gold3,        true },
            { (short)SceneObject.Tungsten1,    true },
            { (short)SceneObject.Nickel1,      true },
        };
        LoadDefaultState();
        SaveState();
    }

    //==================================================== Enums =====================================================

    /// <summary>
    /// Defines the tracked objects in the <see cref="GameStateManager.Scene"/>.
    /// </summary>
    public enum SceneObject : short
    {
        Thruster        = 0,
        Checkpoint1     = 1,
        Checkpoint2     = 2,
        Checkpoint3     = 3,
        Copper1         = 4,
        Gold1           = 5,
        Gold2           = 6,
        Gold3           = 7,
        Tungsten1       = 8,
        Nickel1         = 9,

        Checkpoint      = 99,
    }

    /// <inheritdoc />
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "thruster"              => (short)SceneObject.Thruster,
            "checkpoint 1"          => (short)SceneObject.Checkpoint1,
            "checkpoint 2"          => (short)SceneObject.Checkpoint2,
            "checkpoint 3"          => (short)SceneObject.Checkpoint3,
            "element_copper 1"      => (short)SceneObject.Copper1,
            "element_gold 1"        => (short)SceneObject.Gold1,
            "element_gold 2"        => (short)SceneObject.Gold2,
            "element_gold 3"        => (short)SceneObject.Gold3,
            "element_tungsten 1"    => (short)SceneObject.Tungsten1,
            "element_nickel 1"      => (short)SceneObject.Nickel1,

            "checkpoint"            => (short)SceneObject.Checkpoint,
            _                       => -1,
        };
    }

    /// <inheritdoc />
    public override string Match(short obj)
    {
        return obj switch
        {
            (short)SceneObject.Thruster     => "Thruster",
            (short)SceneObject.Checkpoint1 => "Checkpoint 1",
            (short)SceneObject.Checkpoint2 => "Checkpoint 2",
            (short)SceneObject.Checkpoint3 => "Checkpoint 3",
            (short)SceneObject.Copper1     => "Element_Copper 1",
            (short)SceneObject.Gold1       => "Element_Gold 1",
            (short)SceneObject.Gold2       => "Element_Gold 2",
            (short)SceneObject.Gold3       => "Element_Gold 3",
            (short)SceneObject.Tungsten1   => "Element_Tungsten 1",
            (short)SceneObject.Nickel1     => "Element_Nickel 1",

            (short)SceneObject.Checkpoint   => "Checkpoint",
            _                               => null,
        };
    }

    //==================================================== State =====================================================

    /// <inheritdoc />
    public override void LoadState()
    {

        foreach (KeyValuePair<short, object> pair in SavedState)
        {
            string objectName = Match(pair.Key);
            switch (pair.Key)
            {
                case (short)SceneObject.Thruster:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.Checkpoint1:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.Checkpoint2:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.Checkpoint3:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.Copper1:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.Gold1:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.Gold2:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.Gold3:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.Tungsten1:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
                                return;
                            }
                            targetObject.SetActive(value);
                        }
                    }
                    break;

                case (short)SceneObject.Nickel1:
                    {
                        bool value = (bool)pair.Value;
                        if (!value)
                        {
                            GameObject targetObject = GameObject.Find(objectName);
                            if (targetObject == null)
                            {
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
