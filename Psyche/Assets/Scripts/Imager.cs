/** 
Description: imager expanding script
Author: mcmyers4
Version: 20231031
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imager tool script increases field of vision with every pick up.
/// </summary>
public class Imager : MonoBehaviour
{
    //The player's imager which is being increased
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D imager;
    //Mouse cursor light to be increased
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D flashlight;
    //The amount to increase the vision field
    [SerializeField] private float rangeIncrease = 0.5f;

    [SerializeField] private ImagerCounter imagerCounter;

    /// <summary>
    /// Increases size of vision
    /// </summary>
    public void increaseVision(AudioManager audioManager)
    {
        imager.pointLightInnerRadius += rangeIncrease;
        imager.pointLightOuterRadius += rangeIncrease;
        flashlight.pointLightInnerRadius += rangeIncrease;
        flashlight.pointLightOuterRadius += rangeIncrease;
        audioManager.PlayToolImager(); // play tool sound
        imagerCounter.updateImagerCount();
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
}
