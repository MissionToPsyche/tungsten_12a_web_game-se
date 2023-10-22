/** 
Description: battery script
Author: blopezro
Version: 20231020
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Battery class in which a new batt can be created, drained, and charged. 
/// </summary>
public class Battery : MonoBehaviour {

    public float batteryPercentage;
    public float rate;
    public TMP_Text battPerText;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            DrainBatt(rate);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            ChargeBatt(rate);
        }
    }

    // drains battery at given rate
    public void DrainBatt(float rate) {
        batteryPercentage -= Time.deltaTime * rate;
        batteryPercentage = Mathf.Clamp(batteryPercentage, 0f, 100f); // keeps batt level between 0-100
        UpdateSceneText();
        //Debug.Log("Battery % "+ batteryPercentage);
    }

    // charges battery at given rate
    public void ChargeBatt(float rate) {
        batteryPercentage += Time.deltaTime * rate;
        batteryPercentage = Mathf.Clamp(batteryPercentage, 0f, 100f); // keeps batt level between 0-100
        UpdateSceneText();
        //Debug.Log("Battery % "+ batteryPercentage);
    }

    // update text for scene
    public void UpdateSceneText() {
        battPerText.text = Mathf.RoundToInt(batteryPercentage).ToString() + "%";
    }

}
