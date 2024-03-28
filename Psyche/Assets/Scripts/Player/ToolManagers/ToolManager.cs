/*
 * Authors: JoshBenn
 */
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all managers of every <see cref="InventoryManager.Tool"/>
/// </summary>
public abstract class ToolManager : MonoBehaviour
{
    //======================================== Initialize/Updates/Destroy ========================================

    //Private Variables
    protected PlayerController PlayerController;
    
    //Public variables
    public string ToolName;
    public int Level;
    public Dictionary<int, Dictionary<InventoryManager.Element, ushort>> LevelRequirements;
    public int MaxLevel;
    public bool ToolEnabled;


    //================================================== Events ==================================================
    public event Action<ToolDirective, bool, string, Dictionary<InventoryManager.Element, ushort>, int, int> ToolManagerUpdate;

    //=================================================== Enum ===================================================
    /// <summary>
    /// Directive for actions on the <see cref="InventoryManager.Tool"/>
    /// </summary>
    public enum ToolDirective
    {
        Upgrade, 
        Info,
        MaxLevel
    }

    //=============================================== Tool Actions ===============================================
    /// <summary>
    /// Enable the <see cref="InventoryManager.Tool"/>
    /// </summary>
    public void Enable()
    {
        ToolEnabled = true;
        ToolManagerUpdate?.Invoke(ToolDirective.Info, false, ToolName, LevelRequirements[Level + 1], Level + 1, MaxLevel);
    }

    /// <summary>
    /// Used for <see cref="InventoryManager.Tool"/> activation
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Upgrades the <see cref="InventoryManager.Tool"/>
    /// </summary>
    protected abstract void UpgradeTool();

    /// <summary>
    /// Modifies the <see cref="InventoryManager.Tool"/> when called
    /// </summary>
    public void Modify()
    {
        // Check for upgrading
        int nextLevel = (LevelRequirements.ContainsKey(Level + 1) ? Level + 1 : Level);
        bool upgraded = true;
        if (LevelRequirements.ContainsKey(Level + 1))
        {
            foreach (var requirement in LevelRequirements[Level + 1])
            {
                if (PlayerController.inventoryManager.CheckElement(requirement.Key) < requirement.Value)
                {
                    upgraded = false;
                }
            }
        }
        //Confirm the number of required elements exists
        if (upgraded && (Level + 1) <= nextLevel)
        {
            //Remove the required elements
            foreach (var requirement in LevelRequirements[nextLevel])
            {
                if (requirement.Value > 0) //No need to activate underlying code if this isn't met
                {
                    PlayerController.inventoryManager.RemoveElement(requirement.Key, requirement.Value);
                }
            }
            //Upgrade the tool
            Level++;
            UpgradeTool();
        }

        // Update the event
        if (nextLevel < MaxLevel && upgraded)
        {
            ToolManagerUpdate?.Invoke(ToolDirective.Upgrade, upgraded, ToolName, LevelRequirements[nextLevel + 1], nextLevel, MaxLevel);
        }
        else
        {
            ToolManagerUpdate?.Invoke(ToolDirective.Upgrade, upgraded, ToolName, LevelRequirements[nextLevel], nextLevel, MaxLevel);
        }
    }
}
