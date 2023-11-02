using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolManager : MonoBehaviour
{
    //Private Variables
    protected PlayerManagement _playerManagement;

    //Public variables
    public string toolName;

    /// <summary>
    /// Modifies the tool when called
    /// </summary>
    public abstract void Modify();
}
