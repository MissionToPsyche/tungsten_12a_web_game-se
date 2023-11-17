/** 
Description: battery script
Author: blopezro
Version: 20231109
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

    //Events to communicate to UI
    public delegate void OnBatteryPercentageChanged(float newPercentage);
    public event OnBatteryPercentageChanged onBatteryPercentageChanged;

    /// <summary>
    /// Initialize this script
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController) {
        //Base class variables
        toolName = "Battery";
        toolEnabled = false;
        _playerController = playerController;
        

        //Tool specific
        maxCapacity = 100f;
        batteryLevel = maxCapacity;
        batteryPercent = batteryLevel / maxCapacity * 100f;
        rate = 1;
        batteryDrained = (batteryLevel > 0);
    }

    /// <summary>
    /// drains battery at given rate
    /// </summary>
    /// <param name="rate"></param>
    public void DrainBatt(float rate) {
        batteryLevel -= Time.deltaTime * rate;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, maxCapacity); // keeps batt level between 0-100
        batteryPercent = batteryLevel / maxCapacity * 100f;
        if (batteryPercent == 0) {
            batteryDrained = true;
        }
        //Create package to send
        ArrayList args = new ArrayList {
                "UI", "None", "BatteryManager", Mathf.RoundToInt(batteryPercent),
        };
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// charges battery at given rate
    /// </summary>
    /// <param name="rate"></param>
    public void ChargeBatt(float rate) {
        batteryLevel += Time.deltaTime * rate;        
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, maxCapacity); // keeps batt level between 0-100
        batteryPercent = batteryLevel / maxCapacity * 100f;
        if (batteryPercent > 0) {
            batteryDrained = false;
        }
        //Create package to send
        ArrayList args = new ArrayList {
                "UI", "None", "BatteryManager", Mathf.RoundToInt(batteryPercent),
        };
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// charges battery to max capacity
    /// </summary>
    public void ChargeBattFull() {
        batteryLevel += maxCapacity;        
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, maxCapacity); // keeps batt level between 0-100
        batteryPercent = batteryLevel / maxCapacity * 100f;
        if (batteryPercent > 0) {
            batteryDrained = false;
        }
        //Create package to send
        ArrayList args = new ArrayList {
                "UI", "None", "BatteryManager", Mathf.RoundToInt(batteryPercent),
        };
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// Increases the max capacity when called.
    /// </summary>
    public override void Modify() {
        maxCapacity += 10f;
    }
}
