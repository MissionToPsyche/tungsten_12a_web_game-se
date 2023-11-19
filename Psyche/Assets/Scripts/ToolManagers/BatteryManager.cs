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
        level = 0;
        levelRequirements = new Dictionary<int, Dictionary<string, int>>()
        {
            {  1, new Dictionary<string, int>()
                {
                    { "copper", 0 }, { "iron", 0 }, { "nickel", 1 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  2, new Dictionary<string, int>()
                {
                    { "copper", 0 }, { "iron", 0 } , { "nickel", 2 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  3, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 0 } , { "nickel", 3 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  4, new Dictionary<string, int>()
                {
                    { "copper", 0 } , { "iron", 0 } , { "nickel", 4 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  5, new Dictionary<string, int>()
                {
                    { "copper", 0 } ,   { "iron", 0 } , { "nickel", 5 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
        };

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
    public override void Activate() {
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
    protected override void UpgradeTool() {
        maxCapacity += 10f;
    }
}
