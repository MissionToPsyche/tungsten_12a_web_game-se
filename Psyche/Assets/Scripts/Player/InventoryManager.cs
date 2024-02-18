using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.Tracing;

/// <summary>
/// Manages the inventory and all items within it
/// </summary>
public class InventoryManager : MonoBehaviour
{ 
    private PlayerController _playerController;
    private Dictionary<string, bool>    _tools;
    private Dictionary<Tool, bool>      _tools2;
    private Dictionary<string, ushort>  _elements;
    private Dictionary<Element, ushort> _elements2;

    // Events
    public event Action<Tool, bool>      OnUpdateInventoryTool;
    public event Action<ArrayList>      OnUpdateInventoryElement;
    public event Action<string, object> SetObjectState;

    public enum Element
    {
        COPPER,
        IRON,
        NICKEL,
        GOLD,
        TUNGSTEN,

        None,
    }

    public Element MatchElement(string element)
    {
        return element.ToLower() switch
        {
            "copper"    or "element_copper"     => Element.COPPER,
            "iron"      or "element_iron"       => Element.IRON,
            "nickel"    or "element_nickel"     => Element.NICKEL,
            "gold"      or "element_gold"       => Element.GOLD,
            "tungsten"  or "element_tungsten"   => Element.TUNGSTEN,
            _                                   => Element.None
        };
    }
    public string MatchElement(Element element)
    {
        return element switch
        {
            Element.COPPER      => "element_copper",
            Element.IRON        => "element_iron",
            Element.NICKEL      => "element_nickel",
            Element.GOLD        => "element_gold",
            Element.TUNGSTEN    => "element_tungsten",
            _                   => null
        };
    }

    public enum Tool
    {
        IMAGER,
        BATTERY,
        SOLARPANEL,
        SPECTROMETER,
        THRUSTER,
        ELECTROMAGNET,
        // MAGNETOMETER,

        None,
    }

    public Tool MatchTool(string tool)
    {
        return tool.ToLower() switch
        {
            "imager"        => Tool.IMAGER,
            "battery"       or "battery pickup variant" => Tool.BATTERY,
            "solarpanel"    or "soar panel pickup"      => Tool.SOLARPANEL,
            "spectrometer"  => Tool.SPECTROMETER,
            "thruster"      => Tool.THRUSTER,
            "electromagnet" => Tool.ELECTROMAGNET,
            _               => Tool.None,
        };
    }

    public string MatchTool(Tool tool)
    {
        return tool switch
        {
            Tool.IMAGER         => "imager",
            Tool.BATTERY        => "battery",
            Tool.SOLARPANEL     => "solarpanel",
            Tool.SPECTROMETER   => "spectrometer",
            Tool.THRUSTER       => "thruster",
            Tool.ELECTROMAGNET  => "electromagnet",
            _                   => null,
        };
    }

    /// <summary>
    /// Initialize the class and set up base dictionaries
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        _tools = new Dictionary<string, bool>()
        {
            { "imager",         false },
            { "battery",        false },
            { "spectrometer",   false },
            { "thruster",       false },
            { "electromagnet",  false },
            { "magnetometer",   false },
        };
        _tools2 = new Dictionary<Tool, bool>()
        {
            { Tool.IMAGER,          false },
            { Tool.BATTERY,         false },
            { Tool.SPECTROMETER,    false },
            { Tool.THRUSTER,        false },
            { Tool.ELECTROMAGNET,   false },
        };
        _elements = new Dictionary<string, ushort>()
        {
            { "element_copper",     0 },
            { "element_iron",       0 },
            { "element_nickel",     0 },
            { "element_gold",       0 },
            { "element_tungsten",   0 },
        };
        _elements2 = new Dictionary<Element, ushort>()
        {
            { Element.COPPER,     0 },
            { Element.IRON,       0 },
            { Element.NICKEL,     0 },
            { Element.GOLD,       0 },
            { Element.TUNGSTEN,   0 },
        };
    }
    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// </summary>
    private void OnEnable()
    {
        if (GameController.Instance != null && GameController.Instance.developerConsole != null)
        {
            GameController.Instance.developerConsole.OnDevConsoleInventorySetElement += SetElement;
            GameController.Instance.developerConsole.OnDevConsoleInventorySetTool += SetTool;
        }
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    /// </summary>
    private void OnDisable()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.developerConsole.OnDevConsoleInventorySetElement -= SetElement;
            GameController.Instance.developerConsole.OnDevConsoleInventorySetTool -= SetTool;
        }
    }

    /// <summary>
    /// Activates tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void ToolPickUp(string toolName)
    {
        // Transition to this at some point
        Tool tool = MatchTool(toolName);
        if (tool == Tool.None)
        {
            Debug.Log($"Not a tool {toolName}");
            return;
        }

        if (tool != Tool.None && tool != Tool.BATTERY && tool != Tool.SOLARPANEL)
        {
            SetTool(tool, true);
        }
        OnUpdateInventoryTool?.Invoke(tool, true);
        // Remove the object from the scene state
        SetObjectState?.Invoke(toolName, false);
        
        //Other actions
        switch (tool)
        {
            case Tool.SOLARPANEL:
                _playerController.batteryManager.Enable();
                SetTool(Tool.BATTERY, true);
                break;

            case Tool.THRUSTER:
                _playerController.thrusterManager.Enable();
                break;

            case Tool.IMAGER:
                _playerController.imagerManager.Enable();
                break;

            case Tool.SPECTROMETER:
                break;

            case Tool.ELECTROMAGNET:
                _playerController.eMagnetManager.Enable();
                break;

            case Tool.BATTERY:
                _playerController.batteryManager.ChargeBatt(500);
                break;

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }

    public void SetTool(Tool tool, bool value) // New version that uses enums in place of strings
    {
        if (tool == Tool.None) return;
        _tools2[tool] = value;
        // Temporary translation layer until old version is removed
        _tools[MatchTool(tool)] = value;

        ArrayList args = new ArrayList { MatchTool(tool), value };
        //OnUpdateInventoryTool?.Invoke(args);
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
        OnUpdateInventoryTool?.Invoke(MatchTool(toolName), value);
    }

    /// <summary>
    /// Checks if the tool is true or false
    /// </summary>
    /// <param name="toolName"></param>
    public bool CheckTool(Tool toolName)
    {
        //Check if valid name passed and return boolean
        if (toolName == Tool.None)
        {
            Debug.LogError($"Incorrect tool name passed -- InventoryManager Check: {toolName}");
            return false;
        }
        return _tools2[toolName];
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
    /// <param name="elementName"></param>
    /// <param name="amount"></param>
    public void AddElement(string element, ushort amount)
    {
        // Split the incoming value from its element name and element ID
        string[] element_set = element.Split(' ');
        //Standardize lower case
        Element elementName = MatchElement(element_set[0].ToLower());
        SetObjectState?.Invoke(element, false);
        //Check if valid name passed and return int
        if (!_elements2.ContainsKey(elementName))
        {
            Debug.LogError($"Incorrect element name passed -- InventoryManager Add: {elementName}");
            return;
        }
        _elements2[elementName] += amount;
        // Temporary - continue to track with both dictionaries until fully transitioned over
        _elements[element_set[0].ToLower()] += amount;

        ArrayList args = new ArrayList { elementName, _elements[element_set[0].ToLower()] };
        //Send the message
        OnUpdateInventoryElement.Invoke(args);
    }

    /// <summary>
    /// Removes an amount of an element from the inventory
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void RemoveElement(Element element, ushort amount)
    {
        if (element == Element.None) { return; }
        _elements2[element] -= amount;

        _elements[MatchElement(element)] -= amount;
        //Send message to UI
        ArrayList args = new ArrayList { MatchElement(element), _elements2[element] };
        //Send the message
        OnUpdateInventoryElement.Invoke(args);
    }

    /// <summary>
    /// Removes an amount of an element from the inventory
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void RemoveElement(string element, ushort amount)
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
        ArrayList args = new ArrayList { element, _elements[element] };
        //Send the message
        OnUpdateInventoryElement.Invoke(args);
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void SetElement(Element element, ushort amount)
    {
        Debug.Log("boop");
        Debug.Log($"{element}, {amount}");
        //Check if valid name passed and return int
        if (element == Element.None) { return; }
        _elements2[element] = amount;

        _elements[MatchElement(element)] = amount;
        //Send message to UI
        ArrayList args = new ArrayList { MatchElement(element), _elements2[element] };
        //Send the message
        OnUpdateInventoryElement.Invoke(args);
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void SetElement(string element, ushort amount)
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
        ArrayList args = new ArrayList { element, _elements[element] };
        //Send the message
        OnUpdateInventoryElement.Invoke(args);
    }

    /// <summary>
    /// Returns the amount of elements the player has
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public ushort CheckElement(Element element) /////////////////////////////////////////////////////////////////////////////
    {
        //Check if valid name passed and return int
        if (element == Element.None)
        {
            Debug.LogError($"Incorrect element name passed -- InventoryManager Check: {element}");
            return 0;
        }
        return _elements2[element];
    }

    /// <summary>
    /// Returns the amount of elements the player has
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public int CheckElement(string element) /////////////////////////////////////////////////////////////////////////////
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
