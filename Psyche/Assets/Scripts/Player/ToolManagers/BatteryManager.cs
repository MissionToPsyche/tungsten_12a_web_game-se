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
public class BatteryManager : ToolManager {
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
        toolName = "Battery";
        toolEnabled = false;
        _playerController = playerController;
        level = 1;
        levelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 2 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 3 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 4 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 5 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
        };

        //Tool specific
        maxLevel = levelRequirements.Count + 1;
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
