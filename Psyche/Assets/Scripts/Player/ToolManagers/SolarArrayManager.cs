/** 
Description: battery script
Author: blopezro, mcmyers
Version: 20240220
**/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battery class in which a new batt can be created, drained, and charged. 
/// </summary>
public class SolarArrayManager : ToolManager {
    //Public variables
    public float batteryLevel;
    public float maxCapacity;
    public float batteryPercent;
    public float rate;
    public bool  batteryDrained;

    //Events to communicate to UI
    public event Action<float> OnBatteryPercentageChanged;

    /// <summary>
    /// Initialize this script
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController) {
        //Base class variables
        ToolName = "SolarArray";
        ToolEnabled = false;
        PlayerController = playerController;
        Level = 1;
        LevelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 2 }, { InventoryManager.Element.Gold, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 3 }, { InventoryManager.Element.Gold, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 4 }, { InventoryManager.Element.Gold, 0 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 5 }, { InventoryManager.Element.Gold, 0 },
                }
            },
        };

        //Tool specific
        MaxLevel = LevelRequirements.Count + 1;
        maxCapacity = 100f;
        batteryLevel = maxCapacity;
        batteryPercent = batteryLevel / maxCapacity * 100f;
        rate = 0.5f;
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
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(batteryPercent));
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
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(batteryPercent));
    }

    /// <summary>
    /// passively recharges battery at given rate
    /// </summary>
    public void PassiveBatt()
    {
        batteryLevel += rate/1000 ;
        batteryLevel = Mathf.Clamp(batteryLevel, 0f, maxCapacity); // keeps batt level between 0-100
        batteryPercent = batteryLevel / maxCapacity * 100f;
        if (batteryPercent > 0)
        {
            batteryDrained = false;
        }
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(batteryPercent));
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
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(batteryPercent));
    }

    /// <summary>
    /// Increases the max capacity when called.
    /// </summary>
    protected override void UpgradeTool() {
        rate += 0.5f;
    }
}
