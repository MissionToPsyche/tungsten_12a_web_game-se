/*
 * Authors: JoshBenn
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives the <see cref="BaseState"/> class and implements it for Combo1 <see cref="GameStateManager.Scene.Grns" />.
/// </summary>
public class GRNS_State : BaseState
{
    //========================================= Initialize/Updates/Destroy =========================================

    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public GRNS_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.Spectrometer, true }, // 'true' for available
            { (short)SceneObject.Checkpoint1, false },
            { (short)SceneObject.Gold1, true },
            { (short)SceneObject.Gold2, true },
            { (short)SceneObject.Gold3, true },
            { (short)SceneObject.Gold4, true },
            { (short)SceneObject.Gold5, true },
            { (short)SceneObject.Gold6, true },
            { (short)SceneObject.Gold7, true },
            { (short)SceneObject.Tungsten1, true },
            { (short)SceneObject.Nickel1,   true },
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
        Spectrometer    = 0,
        Checkpoint1     = 2,
        Gold1           = 3,
        Gold2           = 4,
        Gold3           = 5,
        Gold4           = 6,
        Gold5           = 7,
        Gold6           = 8,
        Gold7           = 9,
        Tungsten1       = 10,
        Nickel1         = 11,

        Checkpoint      = 99,
    }

    /// <inheritdoc />
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "spectrometer"          => (short)SceneObject.Spectrometer,
            "checkpoint 1"          => (short)SceneObject.Checkpoint1,
            "element_gold 1"        => (short)SceneObject.Gold1,
            "element_gold 2"        => (short)SceneObject.Gold2,
            "element_gold 3"        => (short)SceneObject.Gold3,
            "element_gold 4"        => (short)SceneObject.Gold4,
            "element_gold 5"        => (short)SceneObject.Gold5,
            "element_gold 6"        => (short)SceneObject.Gold6,
            "element_gold 7"        => (short)SceneObject.Gold7,
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
            (short)SceneObject.Spectrometer => "Spectrometer",
            (short)SceneObject.Checkpoint1  => "Checkpoint 1",
            (short)SceneObject.Gold1        => "Element_Gold 1",
            (short)SceneObject.Gold2        => "Element_Gold 2",
            (short)SceneObject.Gold3        => "Element_Gold 3",
            (short)SceneObject.Gold4        => "Element_Gold 4",
            (short)SceneObject.Gold5        => "Element_Gold 5",
            (short)SceneObject.Gold6        => "Element_Gold 6",
            (short)SceneObject.Gold7        => "Element_Gold 7",
            (short)SceneObject.Tungsten1    => "Element_Tungsten 1",
            (short)SceneObject.Nickel1      => "Element_Nickel 1",

            (short)SceneObject.Checkpoint   => "Checkpoint",
            _                               => "",
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
                case (short)SceneObject.Spectrometer:
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
                            targetObject.IsSpinning = value;
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

                case (short)SceneObject.Gold6:
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

                case (short)SceneObject.Gold7:
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
