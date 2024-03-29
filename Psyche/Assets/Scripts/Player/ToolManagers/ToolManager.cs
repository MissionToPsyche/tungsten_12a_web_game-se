using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ToolManager : MonoBehaviour
{
    //Private Variables
    protected PlayerController _playerController;
    
    //Public variables
    public string toolName;

    //Tool State
    public int level;
    public Dictionary<int, Dictionary<InventoryManager.Element, ushort>> levelRequirements;
    public int maxLevel;

    public bool toolEnabled;

    public enum ToolDirective
    {
        Upgrade, 
        Info,
        MaxLevel
    }

    // Events
    public event Action<ToolDirective, bool, string, Dictionary<InventoryManager.Element, ushort>, int, int> ToolManagerUpdate;

    /// <summary>
    /// Enables the tool
    /// </summary>
    public void Enable()
    {
        toolEnabled = true;
        ToolManagerUpdate?.Invoke(ToolDirective.Info, false, toolName, levelRequirements[level + 1], level + 1, maxLevel);
    }

    /// <summary>
    /// Used for tool activation
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Class-specific implementation to update local variables
    /// </summary>
    protected abstract void UpgradeTool();

    /// <summary>
    /// Modifies the tool when called
    /// </summary>
    public void Modify()
    {
        // Check for upgrading
        int nextLevel = (levelRequirements.ContainsKey(level + 1) ? level + 1 : level);
        bool upgraded = true;
        if (levelRequirements.ContainsKey(level + 1))
        {
            foreach (var requirement in levelRequirements[level + 1])
            {
                if (_playerController.inventoryManager.CheckElement(requirement.Key) < requirement.Value)
                {
                    upgraded = false;
                }
            }
        }
        //Confirm the number of required elements exists
        if (upgraded && level + 1 <= nextLevel)
        {
            //Remove the required elements
            foreach (var requirement in levelRequirements[level + 1])
            {
                if (requirement.Value > 0) //No need to activate underlying code if this isn't met
                {
                    _playerController.inventoryManager.RemoveElement(requirement.Key, requirement.Value);
                }
            }
            //Upgrade the tool
            level++;
            UpgradeTool();
        }
        ToolManagerUpdate?.Invoke(ToolDirective.Upgrade, upgraded, toolName, levelRequirements[nextLevel], level + 1, maxLevel);
    }
}
