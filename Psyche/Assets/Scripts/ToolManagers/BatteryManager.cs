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
public class BatteryManager : ToolManager {
    //Public variables
    public float batteryLevel;
    public float maxCapacity;
    public float batteryPercent;
    public float rate;
    public bool batteryDrained;
    public TMP_Text battPerText;

    //Events to communicate to UI
    public delegate void OnBatteryPercentageChanged(float newPercentage);
    public event OnBatteryPercentageChanged onBatteryPercentageChanged;

    /// <summary>
    /// Initialize this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        toolName = "Battery";
        _playerManagement = playerManagement;
        maxCapacity = 100f;
        batteryLevel = maxCapacity;
        batteryPercent = batteryLevel / maxCapacity * 100f;
        rate = 1;
    }

    // uncomment to test
    /*void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            DrainBatt(rate);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            ChargeBatt(rate);
        }
    }*/

    // drains battery at given rate
    public void DrainBatt(float rate) {
        batteryLevel -= Time.deltaTime * rate;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, maxCapacity); // keeps batt level between 0-100
        batteryPercent = batteryLevel / maxCapacity * 100f;
        if (batteryPercent == 0) {
            batteryDrained = true;
        }
        onBatteryPercentageChanged?.Invoke(batteryPercent);
        Debug.Log("Battery: " + batteryLevel + "/" + maxCapacity);
    }

    // charges battery at given rate
    public void ChargeBatt(float rate) {
        batteryLevel += Time.deltaTime * rate;        
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, maxCapacity); // keeps batt level between 0-100
        batteryPercent = batteryLevel / maxCapacity * 100f;
        if (batteryPercent > 0) {
            batteryDrained = false;
        }
        onBatteryPercentageChanged?.Invoke(batteryPercent);
        Debug.Log("Battery: " + batteryLevel + "/" + maxCapacity);
    }


    /// <summary>
    /// Increases the max capacity when called.
    /// </summary>
    public override void Modify()
    {
        maxCapacity += 10f;
    }
}
