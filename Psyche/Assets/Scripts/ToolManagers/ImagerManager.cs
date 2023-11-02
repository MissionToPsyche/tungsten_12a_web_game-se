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
    //The player's imager which is being increased
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D imager;
    
    //Private Variables
    [SerializeField] private float _rangeIncrease; //Increases imager field

    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        toolName = "Imager";
        _playerManagement = playerManagement;
        _rangeIncrease = 2f;
    }

    /// <summary>
    /// Turns off light
    /// </summary>
    public void turnOff()
    {
        imager.intensity = 0;
    }

    /// <summary>
    /// Turns on light
    /// </summary>
    public void turnOn()
    {
        imager.intensity = 1;
    }

    /// <summary>
    /// Increases the range when called
    /// </summary>
    public override void Modify()
    {
        imager.pointLightInnerRadius += _rangeIncrease;
        imager.pointLightOuterRadius += _rangeIncrease;
        _playerManagement.audioManager.PlayToolImager(); // play tool sound
    }
}
