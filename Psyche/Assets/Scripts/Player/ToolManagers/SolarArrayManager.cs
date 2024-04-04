/** 
Description: Solar array tool and battery script
Author: blopezro, mcmyers4
Version: 20240403
**/

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Solar array class in which a solar array can be picked up in order to recharge the player's battery.
/// Also contains the logic for draining the player's battery.
/// </summary>
public class SolarArrayManager : ToolManager
{
    //======================================== Initialize/Update/Destroy =========================================

    // Public variables
    public float BatteryLevel;
    public float MaxCapacity;
    public float BatteryPercent;
    public float Rate;
    public bool  BatteryDrained;

    /// <summary>
    /// Initialize this script
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        // Base class variables
        ToolName = "SolarArray";
        ToolEnabled = false;
        PlayerController = playerController;
        Level = 1;
        LevelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 1 },
                    { InventoryManager.Element.Gold, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 1 },
                    { InventoryManager.Element.Gold, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 1 },
                    { InventoryManager.Element.Gold, 0 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 1 },
                    { InventoryManager.Element.Gold, 0 },
                }
            },
        };

        // Tool specific variables
        MaxLevel = LevelRequirements.Count + 1;
        MaxCapacity = 100f;
        BatteryLevel = MaxCapacity;
        BatteryPercent = BatteryLevel / MaxCapacity * 100f;
        Rate = 0.1f;
        BatteryDrained = (BatteryLevel > 0);
    }

    //================================================== Events ==================================================

    // Events to communicate to UI
    public event Action<float> OnBatteryPercentageChanged;

    //=============================================== Tool Actions ===============================================

    /// <summary>
    /// Charges battery to max capacity
    /// </summary>
    public override void Activate()
    {
        BatteryLevel += MaxCapacity;
        BatteryLevel = Mathf.Clamp(BatteryLevel, 0f, MaxCapacity); // keeps batt level between 0-100
        BatteryPercent = BatteryLevel / MaxCapacity * 100f;
        if (BatteryPercent > 0)
        {
            BatteryDrained = false;
        }
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(BatteryPercent));
    }

    /// <summary>
    /// Drains battery at given rate
    /// </summary>
    /// <param name="rate"></param>
    public void DrainBatt(float rate)
    {
        BatteryLevel -= Time.deltaTime * rate;
        BatteryLevel = Mathf.Clamp(BatteryLevel, 0f, MaxCapacity); // keeps batt level between 0-100
        BatteryPercent = BatteryLevel / MaxCapacity * 100f;
        if (BatteryPercent == 0)
        {
            BatteryDrained = true;
        }
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(BatteryPercent));
    }

    /// <summary>
    /// Charges battery at given rate
    /// </summary>
    /// <param name="rate"></param>
    public void ChargeBatt(float rate)
    {
        BatteryLevel += Time.deltaTime * rate;        
        BatteryLevel = Mathf.Clamp(BatteryLevel, 0f, MaxCapacity); // keeps batt level between 0-100
        BatteryPercent = BatteryLevel / MaxCapacity * 100f;
        if (BatteryPercent > 0)
        {
            BatteryDrained = false;
        }
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(BatteryPercent));
    }

    /// <summary>
    /// Passively recharges battery at given rate
    /// </summary>
    public void PassiveBatt()
    {
        BatteryLevel += Rate/1000 ;
        BatteryLevel = Mathf.Clamp(BatteryLevel, 0f, MaxCapacity); // keeps batt level between 0-100
        BatteryPercent = BatteryLevel / MaxCapacity * 100f;
        if (BatteryPercent > 0)
        {
            BatteryDrained = false;
        }
        OnBatteryPercentageChanged?.Invoke(Mathf.RoundToInt(BatteryPercent));
    }

    /// <summary>
    /// Increases the rate of recharge when called.
    /// </summary>
    protected override void UpgradeTool()
    {
        Rate += 0.5f;
    }
}
