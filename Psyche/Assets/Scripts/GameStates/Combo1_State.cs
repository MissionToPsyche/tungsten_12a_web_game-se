/*
 * Authors: JoshBenn
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives the <see cref="BaseState"/> class and implements it for Combo1 <see cref="GameStateManager.Scene.Combo1"/>.
/// </summary>
public class Combo1_State : BaseState
{
    //========================================= Initialize/Updates/Destroy =========================================

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Combo1_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.Checkpoint1, false },
            { (short)SceneObject.Checkpoint2, false },
            { (short)SceneObject.Checkpoint3, false },
            { (short)SceneObject.Checkpoint4, false },
            { (short)SceneObject.Tungsten1,   true },
            { (short)SceneObject.Gold1,       true },
            { (short)SceneObject.Copper1,     true },
            { (short)SceneObject.Copper2,     true },
            { (short)SceneObject.Iron1,       true },
            { (short)SceneObject.Nickel1,     true },
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
        Checkpoint1    = 0,
        Checkpoint2    = 1,
        Checkpoint3    = 2,
        Checkpoint4    = 3,
        Tungsten1      = 4,
        Gold1          = 5,
        Copper1        = 6,
        Copper2        = 7,
        Iron1          = 8,
        Nickel1        = 9,

        Checkpoint      = 99,
    }

    /// <inheritdoc />
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "checkpoint 1"          => (short)SceneObject.Checkpoint1,
            "checkpoint 2"          => (short)SceneObject.Checkpoint2,
            "checkpoint 3"          => (short)SceneObject.Checkpoint3,
            "checkpoint 4"          => (short)SceneObject.Checkpoint4,
            "element_tungsten 1"    => (short)SceneObject.Tungsten1,
            "element_gold 1"        => (short)SceneObject.Gold1,
            "element_copper 1"      => (short)SceneObject.Copper1,
            "element_copper 2"      => (short)SceneObject.Copper2,
            "element_iron 1"        => (short)SceneObject.Iron1,
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
            (short)SceneObject.Checkpoint1  => "Checkpoint 1",
            (short)SceneObject.Checkpoint2  => "Checkpoint 2",
            (short)SceneObject.Checkpoint3  => "Checkpoint 3",
            (short)SceneObject.Checkpoint4  => "Checkpoint 4",
            (short)SceneObject.Tungsten1    => "Element_Tungsten 1",
            (short)SceneObject.Gold1        => "Element_Gold 1",
            (short)SceneObject.Copper1      => "Element_Copper 1",
            (short)SceneObject.Copper2      => "Element_Copper 2",
            (short)SceneObject.Iron1        => "Element_Iron 1",
            (short)SceneObject.Nickel1      => "Element_Nickel 1",

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

                case (short)SceneObject.Checkpoint4:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.isSpinning = value;
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

                case (short) SceneObject.Copper1:
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

                case (short)SceneObject.Copper2:
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

                case (short)SceneObject.Iron1:
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
