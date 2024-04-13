/*
 * Authors: JoshBenn, JamesOliver
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives the <see cref="BaseState"/> class and implements it for Combo1 <see cref="GameStateManager.Scene.Combo3" />.
/// </summary>
public class Combo3_State : BaseState
{
    //========================================= Initialize/Updates/Destroy =========================================
    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Combo3_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.Checkpoint1, false }, 
            { (short)SceneObject.Checkpoint2, false },
            { (short)SceneObject.Checkpoint3, false },
            { (short)SceneObject.Tungsten1,   true },
            { (short)SceneObject.Iron1,       true },
            { (short)SceneObject.Iron2,       true },
            { (short)SceneObject.Iron3,       true },
            { (short)SceneObject.Iron4,       true },
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
        Checkpoint1     = 0,
        Checkpoint2     = 1,
        Checkpoint3     = 2,
        Tungsten1       = 3,
        Iron1           = 4,
        Iron2           = 5,
        Iron3           = 6,
        Iron4           = 7,

        Checkpoint      = 99,
    }

    /// <inheritdoc />
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "checkpoint 1"          => (short)SceneObject.Checkpoint1,
            "checkpoint 2"          => (short)SceneObject.Checkpoint2,
            "checkpoint 3"          => (short)SceneObject.Checkpoint2,
            "element_tungsten 1"    => (short)SceneObject.Tungsten1,
            "element_iron 1"        => (short)SceneObject.Iron1,
            "element_iron 2"        => (short)SceneObject.Iron2,
            "element_iron 3"        => (short)SceneObject.Iron3,
            "element_iron 4"        => (short)SceneObject.Iron4,

            "checkpoint"            => (short)SceneObject.Checkpoint,
            _                       => -1,
        };
    }

    /// <inheritdoc />
    public override string Match(short obj)
    {
        return obj switch
        {
            (short)SceneObject.Checkpoint1 => "Checkpoint 1",
            (short)SceneObject.Checkpoint2 => "Checkpoint 2",
            (short)SceneObject.Iron1       => "Element_Iron 1",
            (short)SceneObject.Iron2       => "Element_Iron 2",
            (short)SceneObject.Iron3       => "Element_Iron 3",
            (short)SceneObject.Iron4       => "Element_Iron 4",

            (short)SceneObject.Checkpoint   => "Checkpoint",
            _ => null,
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
                            targetObject.IsSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.Checkpoint2:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.IsSpinning = value;
                        }
                    }
                    break;

                case (short)SceneObject.Checkpoint3:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.IsSpinning = value;
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

                case (short)SceneObject.Iron2:
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
                case (short)SceneObject.Iron3:
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

                case (short)SceneObject.Iron4:
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
