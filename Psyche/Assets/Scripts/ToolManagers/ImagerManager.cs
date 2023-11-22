/** 
Description: imager tool script
Author: mcmyers4
Version: 20231028
**/

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imager tool script increases field of vision with every additional imager pick up.
/// </summary>
public class ImagerManager : ToolManager
{
    //Private Variables
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _imager; //The player's imager - Not initialized through code
    [SerializeField] private bool _active;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _flashlight; //The cursor's imager - Not initialized through code
    [SerializeField] private float _rangeIncrease; //The amount to increase the vision field
    [SerializeField] private ImagerCounter _imagerCounter; //Not initialized though code
    

    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerController playerManagement)
    {
        //Base class variables
        toolName = "Imager";
        toolEnabled = false;
        _playerController = playerManagement;
        level = 0;
        levelRequirements = new Dictionary<int, Dictionary<string, int>>()
        {
            {  1, new Dictionary<string, int>()
                {
                    { "copper", 0 }, { "iron", 0 }, { "nickel", 0 }, { "gold", 1 }, { "titanium", 0 }
                }
            },
            {  2, new Dictionary<string, int>()
                {
                    { "copper", 0 }, { "iron", 0 } , { "nickel", 0 } , { "gold", 2 }, { "titanium", 0 }
                }
            },
            {  3, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 0 } , { "nickel", 0 } , { "gold", 3 }, { "titanium", 0 }
                }
            },
            {  4, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 0 } , { "nickel", 0 } , { "gold", 4 }, { "titanium", 0 }
                }
            },
            {  5, new Dictionary<string, int>()
                {
                    { "copper", 0 } ,   { "iron", 0 } , { "nickel", 0 } , { "gold", 5 }, { "titanium", 0 }
                }
            },
        };

        //Tool specific variables
        _rangeIncrease = 0.5f;
        _active = false;
    }

    /// <summary>
    /// Turns on light
    /// </summary>
    public override void Activate()
    {
        _active = !_playerController.batteryManager.batteryDrained;
        if(_active)
        {
            _imager.intensity = 1;
        } 
        else
        {
            _imager.intensity = 0;
        }
    }

    /// <summary>
    /// Increases the range when called
    /// </summary>
    protected override void UpgradeTool()
    {
        //Increase imager radius
        _imager.pointLightInnerRadius += _rangeIncrease;
        _imager.pointLightOuterRadius += _rangeIncrease;
        //Increase cursor radius
        _flashlight.pointLightInnerRadius += _rangeIncrease;
        _flashlight.pointLightOuterRadius += _rangeIncrease;
        GameController.Instance.audioManager.PlayAudio(
            GameController.Instance.audioManager.toolImager);
        _imagerCounter.updateImagerCount(); 
    }
}
