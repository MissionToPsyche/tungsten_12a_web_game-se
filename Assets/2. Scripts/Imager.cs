/** 
Description: imager tool script
Author: mcmyers4
Version: 20231004
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imager : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D imager;
    [SerializeField] private float rangeIncrease = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Imager"))
        {
            if (imager != null)
            {
                imager.pointLightInnerRadius += rangeIncrease;
                imager.pointLightOuterRadius += rangeIncrease;
                Destroy(collision.gameObject);
            }
        }
    }
}
