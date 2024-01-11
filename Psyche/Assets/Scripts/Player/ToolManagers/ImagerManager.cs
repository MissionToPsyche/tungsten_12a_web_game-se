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
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _playerlight; //The player's imager - Not initialized through code
    [SerializeField] private bool _active;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _flashlight; //The cursor's imager - Not initialized through code
    [SerializeField] private float _radiusIncrease; //The amount to increase the vision field
    [SerializeField] private ImagerCounter _imagerPickupCounter; //Not initialized though code
    

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
                    { "element_copper", 0 }, { "element_iron", 0 }, { "element_nickel", 0 }, { "element_gold", 1 }, { "element_platinum", 0 }
                }
            },
            {  2, new Dictionary<string, int>()
                {
                    { "element_copper", 0 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 2 }, { "element_platinum", 0 }
                }
            },
            {  3, new Dictionary<string, int>()
                {
                    { "element_copper", 0 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 3 }, { "element_platinum", 0 }
                }
            },
            {  4, new Dictionary<string, int>()
                {
                    { "element_copper", 0 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 4 }, { "element_platinum", 0 }
                }
            },
            {  5, new Dictionary<string, int>()
                {
                    { "element_copper", 0 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 5 }, { "element_platinum", 0 }
                }
            },
        };

        //Tool specific variables
        _radiusIncrease = 0.5f;
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
            _playerlight.intensity = 1;
        } 
        else
        {
            _playerlight.intensity = 0;
        }
    }

    /// <summary>
    /// Increases the range when called
    /// </summary>
    protected override void UpgradeTool()
    {
        //Increase imager radius
        _playerlight.pointLightInnerRadius += _radiusIncrease;
        _playerlight.pointLightOuterRadius += _radiusIncrease;
        //Increase cursor radius
        _flashlight.pointLightInnerRadius += _radiusIncrease;
        _flashlight.pointLightOuterRadius += _radiusIncrease;
        GameController.Instance.audioManager.PlayAudio(
            GameController.Instance.audioManager.toolImager);
        _imagerPickupCounter.updateImagerCount();
    }
}
