/** 
Description: imager on player tool script
Author: mcmyers4
Version: 20231024
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ImagerExpander tool script increases field of vision surrounding player with every additional imager pick up.
/// </summary>
public class ImagerExpander : MonoBehaviour
{
    //The player's imager which is being increased
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D playerlight;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D flashlight;
    //The amount to increase the vision field
    [SerializeField] private float rangeIncrease = 0.5f;

    /// <summary>
    /// Increases size of vision
    /// </summary>
    public void increaseVision(AudioManager audioManager)
    {
        playerlight.pointLightInnerRadius += rangeIncrease;
        playerlight.pointLightOuterRadius += rangeIncrease;
        audioManager.PlayToolImager(); // play tool sound
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Imager"))
        {
            if (flashlight.intensity > 0)
            {
                playerlight.pointLightInnerRadius += rangeIncrease;
                playerlight.pointLightOuterRadius += rangeIncrease;
                flashlight.pointLightInnerRadius += rangeIncrease;
                flashlight.pointLightOuterRadius += rangeIncrease;
                Destroy(collision.gameObject);
            }
        }
    }
}
