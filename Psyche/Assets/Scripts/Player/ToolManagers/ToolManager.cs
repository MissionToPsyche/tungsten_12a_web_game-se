using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolManager : MonoBehaviour
{
    //Private Variables
    protected PlayerController _playerController;
    
    //Public variables
    public string toolName;

    //Tool State
    public int level;
    public Dictionary<int, Dictionary<string, ushort>> levelRequirements;

    public bool toolEnabled;

    public enum ToolDirective
    {
        Upgrade, 
        Info,
    }

    // Events
    public event Action<ToolDirective, bool, string, Dictionary<string, ushort>, int> ToolManagerUpdate;

    /// <summary>
    /// Enables the tool
    /// </summary>
    public void Enable()
    {
        toolEnabled = true;
        //Create package to send to UI to upgrade interface
        ArrayList args = new ArrayList {
                "UI", "None", "ToolManager", "ToolInfo", "info", false, levelRequirements[level+1],
        };
        ToolManagerUpdate?.Invoke(ToolDirective.Info, false, toolName, levelRequirements[level + 1], level + 1);
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
        //int newLevel = level + 1;
        //Create package to send to UI to upgrade interface
        ArrayList args = new ArrayList {
                "UI", "None", "ToolManager", "ToolInfo", "upgrade",
        };

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
        //args.Add(upgraded);
        //args.Add(toolName);
        //Confirm the number of required elements exists
        if (upgraded)
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
            //args.Add(levelRequirements[newLevel]);
        } else
        {
            //args.Add(levelRequirements[newLevel]);
        }
        //args.Add(level + 1);
        ToolManagerUpdate?.Invoke(ToolDirective.Upgrade, upgraded, toolName, levelRequirements[level + 1], level + 1);
        //_playerController.SendMessage(args);
    }
}
