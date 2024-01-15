/** 
Description: imager counter script
Author: mcmyers4
Version: 20231031
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Imager counter script to track imager pickups on screen.
/// </summary>
public class ImagerPickupCounter : MonoBehaviour
{
    //Imager counter text
    public TMP_Text imagerPickupCounterText;
    public int imagerPickupCount = 0;

    /// <summary>
    /// Increment imager counter
    /// </summary>
    public void updateImagerCount()
    {
        imagerPickupCount++;
        imagerPickupCounterText.text = "Imagers Found: " + imagerPickupCount.ToString();
    }
}
