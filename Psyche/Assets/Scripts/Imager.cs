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
    public void increaseVision()
    {
        imager.pointLightInnerRadius += rangeIncrease;
        imager.pointLightOuterRadius += rangeIncrease;
        //Plays tool sound
        AudioManager audioManager = GameObject
            .FindGameObjectWithTag("AudioSources")
            .GetComponent<AudioManager>();
        audioManager.PlayToolImager();
    }

    //***Now handled from toolPickedUp() in PlayerManagement***
    ////Pick up imager on contact and increase size of vision
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Imager"))
    //    {
    //        if (imager != null)
    //        {
    //            imager.pointLightInnerRadius += rangeIncrease;
    //            imager.pointLightOuterRadius += rangeIncrease;
    //            Destroy(collision.gameObject);
    //        }
    //    }
    //}
}
