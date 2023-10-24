/** 
Description: imager tool script
Author: mcmyers4
Version: 20231014
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imager tool script increases field of vision with every additional imager pick up.
/// </summary>
public class Imager : MonoBehaviour
{
    //The player's imager which is being increased
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D imager;
    //The amount to increase the vision field
    [SerializeField] private float rangeIncrease = 2f;

    /// <summary>
    /// Increases size of vision
    /// </summary>
    public void increaseVision(AudioManager audioManager)
    {
        imager.pointLightInnerRadius += rangeIncrease;
        imager.pointLightOuterRadius += rangeIncrease;
        audioManager.PlayToolImager(); // play tool sound
    }
}
