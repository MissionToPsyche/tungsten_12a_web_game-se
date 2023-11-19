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
    public Dictionary<int, Dictionary<string, int>> levelRequirements;

    public bool toolEnabled { get; protected set; }

    /// <summary>
    /// Enables the tool
    /// </summary>
    public void Enable()
    {
        toolEnabled = true;
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
    public bool Modify()
    {
        if (!levelRequirements.ContainsKey(level + 1))
        {
            Debug.Log("Not Upgraded yo");
            return false; //Level out of scope
        }
        //foreach (var requirement in levelRequirements[level + 1])
        //{
            //if (inventory.get_resource_amount(requirement) < requirement.Value)
            //{
            //    return false;
            //}
        //}
        //Upgrade the tool
        level++;
        UpgradeTool(); //<-- Maybe have another dictionary which holds different values per level
        //Create package to send to UI to upgrade interface
        ArrayList args = new ArrayList {
                "UI", "None", "ToolUpgradeInterface", levelRequirements[level],
        };
        _playerController.SendMessage(args);
        
        //Notify success
        return true;
    }
}
