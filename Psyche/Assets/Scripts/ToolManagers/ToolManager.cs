using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolManager : MonoBehaviour
{
    //Private Variables
    protected PlayerController _playerController;
    
    //Public variables
    public string toolName;
    public bool toolEnabled { get; protected set; }

    public void EnableTool()
    {
        toolEnabled = true;
        Debug.Log("Enabled: " + toolEnabled);
    }

    /// <summary>
    /// Modifies the tool when called
    /// </summary>
    public abstract void Modify();
}
