using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// Manages the inventory and all items within it
/// </summary>
public class InventoryManager : MonoBehaviour
{
    private PlayerController _playerController;
    private Dictionary<string, bool> _tools;
    private Dictionary<string, int> _elements;

    /// <summary>
    /// Initialize the class and set up base dictionaries
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        _tools = new Dictionary<string, bool>()
        {
            { "battery", false },
            { "imager", false },
            { "spectrometer", false },
            { "thruster", false },
            { "electromagnet", false },
            { "magnetometer", false },
        };
        _elements = new Dictionary<string, int>()
        {
            { "element_copper", 0 },
            { "element_iron", 0 },
            { "element_nickel", 0 },
            { "element_gold", 0 },
            { "element_platinum", 0 },
        };
    }

    public void ReceiveMessage(ArrayList args)
    {
        int i = 0;
        foreach(var arg in args)
        {
            Debug.Log(i++ + " " + arg.ToString());
        }
        string item = args[2].ToString().ToLower();
        if(_tools.ContainsKey(item))
        {
            SetTool(item, bool.Parse(args[3].ToString()));
        } 
        else if (_elements.ContainsKey(item))
        {
            SetElement(item, int.Parse(args[3].ToString())) ;
        }
        else
        {
            Debug.Log("Incorrect item name passed -- InventoryManager");
        }
    }

    /// <summary>
    /// Set the tool as obtained or not obtained
    /// </summary>
    /// <param name="toolName"></param>
    /// <param name="value"></param>
    public void SetTool(string toolName, bool value)
    {
        //Standardize lower case
        toolName = toolName.ToLower();
        //Check if the toolName passed is valid and update if so
        if(!_tools.ContainsKey(toolName)) 
        {
            Debug.LogError("Incorrect tool name passed -- InventoryManager Set");
            return;
        }
        _tools[toolName] = value;
        //Send message to UI
        ArrayList args = new ArrayList {
                "UI", "None", "InventoryManager", "tool_update", toolName, value,
            };
        //Send the message
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// Checks if the tool is true or false
    /// </summary>
    /// <param name="toolName"></param>
    public bool CheckTool(string toolName)
    {
        //Standardize lower case
        toolName = toolName.ToLower();
        //Check if valid name passed and return boolean
        if (!_tools.ContainsKey(toolName))
        {
            Debug.LogError("Incorrect tool name passed -- InventoryManager Check");
            return false;
        }
        return _tools[toolName];
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void AddElement(string element, int amount)
    {
        //Standardize lower case
        element = element.ToLower();
        //Check if valid name passed and return int
        if (!_elements.ContainsKey(element))
        {
            Debug.LogError("Incorrect element name passed -- InventoryManager Add");
            return;
        }
        _elements[element] += amount;
        //Send message to UI
        ArrayList args = new ArrayList {
                "UI", "None", "InventoryManager", "element_update", element, _elements[element],
            };
        //Send the message
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// Removes an amount of an element from the inventory
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void RemoveElement(string element, int amount)
    {
        //Standardize lower case
        element = element.ToLower();
        //Check if valid name passed and return int
        if (!_elements.ContainsKey(element))
        {
            Debug.LogError("Incorrect element name passed -- InventoryManager Remove");
            return;
        }
        _elements[element] -= amount;
        //Send message to UI
        ArrayList args = new ArrayList {
                "UI", "None", "InventoryManager", "element_update", element, _elements[element],
            };
        //Send the message
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void SetElement(string element, int amount)
    {
        //Standardize lower case
        element = element.ToLower();
        //Check if valid name passed and return int
        if (!_elements.ContainsKey(element))
        {
            Debug.LogError("Incorrect element name passed -- InventoryManager Set");
            return;
        }
        _elements[element] = amount;
        //Send message to UI
        ArrayList args = new ArrayList {
                "UI", "None", "InventoryManager", "element_update", element, _elements[element],
            };
        //Send the message
        _playerController.SendMessage(args);
    }

    /// <summary>
    /// Returns the amount of elements the player has
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public int CheckElement(string element)
    {
        //Standardize lower case
        element = element.ToLower();
        //Check if valid name passed and return int
        if (!_elements.ContainsKey(element))
        {
            Debug.LogError("Incorrect element name passed -- InventoryManager Check");
            return 0;
        }
        return _elements[element];
    }
}
