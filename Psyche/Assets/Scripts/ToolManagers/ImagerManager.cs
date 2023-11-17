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
        _playerManagement = playerManagement;

        //Tool specific variables
        _rangeIncrease = 0.5f;
    }

    /// <summary>
    /// Turns off light
    /// </summary>
    public void turnOff()
    {
        _imager.intensity = 0;
    }

    /// <summary>
    /// Turns on light
    /// </summary>
    public void turnOn()
    {
        _imager.intensity = 1;
    }

    /// <summary>
    /// Increases the range when called
    /// </summary>
    public override void Modify()
    {
        //Increase imager radius
        _imager.pointLightInnerRadius += _rangeIncrease;
        _imager.pointLightOuterRadius += _rangeIncrease;
        //Increase cursor radius
        _flashlight.pointLightInnerRadius += _rangeIncrease;
        _flashlight.pointLightOuterRadius += _rangeIncrease;
        _playerManagement.audioManager.PlayAudio(_playerManagement.audioManager.toolImager);
        _imagerCounter.updateImagerCount(); 
    }
}
