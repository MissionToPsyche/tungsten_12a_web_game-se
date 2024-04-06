/*
 * Authors: JoshBenn
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives the <see cref="BaseState"/> class and implements it for Combo1 <see cref="GameStateManager.Scene.EMagnet/>.
/// </summary>
public class Imager_State : BaseState
{
    //========================================= Initialize/Updates/Destroy =========================================

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public Imager_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.Imager,        true }, // 'true' for available
            { (short)SceneObject.Checkpoint1,   false },
            { (short)SceneObject.Checkpoint2,   false },
            { (short)SceneObject.Tungsten1,     true },
            { (short)SceneObject.Gold1,         true },
            { (short)SceneObject.Gold2,         true },
            { (short)SceneObject.Gold3,         true },
            { (short)SceneObject.Gold4,         true },
            { (short)SceneObject.Gold5,         true },
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
        Imager          = 0,
        Checkpoint1     = 1,
        Checkpoint2     = 2,
        Tungsten1       = 3,
        Gold1           = 4,
        Gold2           = 5,
        Gold3           = 6, 
        Gold4           = 7, 
        Gold5           = 8,

        Checkpoint      = 99,
    }

    /// <inheritdoc />
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "imager"                => (short)SceneObject.Imager,
            "checkpoint 1"          => (short)SceneObject.Checkpoint1,
            "checkpoint 2"          => (short)SceneObject.Checkpoint2,
            "element_tungsten 1"    => (short)SceneObject.Tungsten1,
            "element_gold 1"        => (short)SceneObject.Gold1,
            "element_gold 2"        => (short)SceneObject.Gold2,
            "element_gold 3"        => (short)SceneObject.Gold3,
            "element_gold 4"        => (short)SceneObject.Gold4,
            "element_gold 5"        => (short)SceneObject.Gold5,

            "checkpoint"            => (short)SceneObject.Checkpoint,
            _                       => -1,
        };
    }

    /// <inheritdoc />
    public override string Match(short obj)
    {
        return obj switch
        {
            (short)SceneObject.Imager       => "Imager",
            (short)SceneObject.Checkpoint1  => "Checkpoint 1",
            (short)SceneObject.Checkpoint2  => "Checkpoint 2",
            (short)SceneObject.Tungsten1    => "Element_Tungsten 1",
            (short)SceneObject.Gold1        => "Element_Gold 1",
            (short)SceneObject.Gold2        => "Element_Gold 2",
            (short)SceneObject.Gold3        => "Element_Gold 3",
            (short)SceneObject.Gold4        => "Element_Gold 4",
            (short)SceneObject.Gold5        => "Element_Gold 5",

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
                case (short)SceneObject.Imager:
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
                case (short)SceneObject.Gold4:
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
                case (short)SceneObject.Gold5:
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