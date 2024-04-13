/*
 * Authors: JoshBenn
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Derives the <see cref="BaseState"/> class and implements it for Combo1 <see cref="GameStateManager.Scene.EMagnet" />.
/// </summary>
public class EMagnet_State : BaseState
{
    //========================================= Initialize/Updates/Destroy =========================================
    /// <summary>
    /// Constructor for the scene state
    /// </summary>
    public EMagnet_State()
    {
        // This must be set up first before anything else is created as everything else is based off of this
        DefaultState = new Dictionary<short, object>
        {
            { (short)SceneObject.Electromagnet, true },
            { (short)SceneObject.Checkpoint0,   false },
            { (short)SceneObject.Checkpoint1,   false },
            { (short)SceneObject.Gold1,         true },
            { (short)SceneObject.Gold2,         true },
            { (short)SceneObject.Iron1,         true },
            { (short)SceneObject.Tungsten1,     true },
            { (short)SceneObject.FirstDeposits,    false },
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
        Electromagnet   = 0,
        Checkpoint0     = 1,
        Checkpoint1     = 2,
        Gold1           = 3,
        Gold2           = 4,
        Iron1           = 5,
        Tungsten1       = 6,
        FirstDeposits      = 7,

        Checkpoint      = 99,
    }

    /// <inheritdoc />
    public override short Match(string obj)
    {
        return obj.ToLower() switch
        {
            "electromagnet"         => (short)SceneObject.Electromagnet,
            "checkpoint 0"          => (short)SceneObject.Checkpoint0,
            "checkpoint 1"          => (short)SceneObject.Checkpoint1,
            "element_gold 1"        => (short)SceneObject.Gold1,
            "element_gold 2"        => (short)SceneObject.Gold2,
            "element_iron 1"        => (short)SceneObject.Iron1,
            "element_tungsten 1"    => (short)SceneObject.Tungsten1,
            "first_deposits"        => (short)SceneObject.FirstDeposits,

            "checkpoint"            => (short)SceneObject.Checkpoint,
            _                       => -1,
        };
    }

    /// <inheritdoc />
    public override string Match(short obj)
    {
        return obj switch
        {
            (short)SceneObject.Electromagnet    => "ElectroMagnet",
            (short)SceneObject.Checkpoint0      => "Checkpoint 0",
            (short)SceneObject.Checkpoint1      => "Checkpoint 1",
            (short)SceneObject.Gold1            => "Element_Gold 1",
            (short)SceneObject.Gold2            => "Element_Gold 2",
            (short)SceneObject.Iron1            => "Element_Iron 1",
            (short)SceneObject.Tungsten1        => "Element_Tungsten 1",
            (short)SceneObject.FirstDeposits    => "First_Deposits",

            (short)SceneObject.Checkpoint       => "Checkpoint",
            _                                   => null,
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
                case (short)SceneObject.Electromagnet:
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

                case (short)SceneObject.Checkpoint0:
                    {
                        bool value = (bool)pair.Value;
                        if (value)
                        {
                            Checkpoint targetObject = GameObject.Find(objectName).GetComponent<Checkpoint>();
                            targetObject.IsSpinning = value;
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

                case (short)SceneObject.FirstDeposits:
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