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
    private Dictionary<Tool, bool>      _tools;
    private Dictionary<Element, ushort> _elements;

    // Events
    public event Action<Tool, bool>         OnUpdateInventoryTool;
    public event Action<Element, ushort>    OnUpdateInventoryElement;
    public event Action<string, object>     SetObjectState;

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
            "battery"       => Tool.BATTERY,
            "solarpanel"    => Tool.SOLARPANEL,
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
            Tool.IMAGER         => "Imager",
            Tool.BATTERY        => "Battery",
            Tool.SOLARPANEL     => "SolarPanel",
            Tool.SPECTROMETER   => "Spectrometer",
            Tool.THRUSTER       => "Thruster",
            Tool.ELECTROMAGNET  => "Electromagnet",
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
        _tools = new Dictionary<Tool, bool>()
        {
            { Tool.IMAGER,          false },
            { Tool.BATTERY,         false },
            { Tool.SPECTROMETER,    false },
            { Tool.THRUSTER,        false },
            { Tool.ELECTROMAGNET,   false },
        };
        _elements = new Dictionary<Element, ushort>()
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

    }

    /// <summary>
    /// When event call is no longer active, turns off function
    /// </summary>
    private void OnDisable()
    {

    }

    /// <summary>
    /// Activates tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void ToolPickUp(string toolName)
    {
        // Parse the tool, return if None
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
        // Remove the object from the scene state
        SetObjectState?.Invoke(toolName, false);
        
        //Other actions
        switch (tool)
        {
            case Tool.SOLARPANEL or Tool.BATTERY:
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

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }

    /// <summary>
    /// Set the tool as obtained or not obtained
    /// </summary>
    /// <param name="toolName"></param>
    /// <param name="value"></param>
    public void SetTool(Tool tool, bool value)
    {
        // Ensure the tool exists
        if (tool == Tool.None) return;

        // Handle if SolarPanel passed explicitly, maybe we should fix this so only one exists?
        if (tool == Tool.SOLARPANEL) { _tools[Tool.BATTERY] = value; }
        else { _tools[tool] = value; }

        // Update events
        OnUpdateInventoryTool?.Invoke(tool, value);
    }

    /// <summary>
    /// Checks if the tool is true or false
    /// </summary>
    /// <param name="tool"></param>
    public bool CheckTool(Tool tool)
    {
        //Check if valid name passed and return boolean
        if (tool == Tool.None)
        {
            Debug.LogError($"Incorrect tool name passed -- InventoryManager Check: {tool}");
            return false;
        }
        return _tools[tool];
    }

    /// <summary>
    /// Checks if the tool is true or false
    /// </summary>
    /// <param name="toolName"></param>
    public bool CheckTool(string toolName)
    {
        Tool tool = MatchTool(toolName);
        //Check if valid name passed and return boolean
        if (!_tools.ContainsKey(tool))
        {
            Debug.LogError("Incorrect tool name passed -- InventoryManager Check");
            return false;
        }
        return _tools[tool];
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="elementName"></param>
    /// <param name="amount"></param>
    public void AddElement(string item, ushort amount)
    {
        // Split the incoming value from its element name and element ID
        string[] element_set = item.Split(' ');

        //Check if valid name passed and return int
        Element element = MatchElement(element_set[0]);
        if (!_elements.ContainsKey(element))
        {
            Debug.LogError($"Incorrect element name passed -- InventoryManager Add: {element}");
            return;
        }
        _elements[element] += amount;

        // Update events
        SetObjectState?.Invoke(item, false);
        OnUpdateInventoryElement?.Invoke(element, _elements[element]);
    }

    /// <summary>
    /// Removes an amount of an element from the inventory
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void RemoveElement(Element element, ushort amount)
    {
        //Check if valid name passed and return int
        if (!_elements.ContainsKey(element))
        {
            Debug.LogError("Incorrect element name passed -- InventoryManager Remove");
            return;
        }
        _elements[element] -= amount;

        // Update events
        OnUpdateInventoryElement?.Invoke(element, _elements[element]);
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="element"></param>
    /// <param name="amount"></param>
    public void SetElement(Element element, ushort amount)
    {
        //Check if valid name passed and return int
        if (element == Element.None) { return; }
        _elements[element] = amount;

        // Update events
        OnUpdateInventoryElement?.Invoke(element, _elements[element]);
    }

    /// <summary>
    /// Returns the amount of elements the player has
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public ushort CheckElement(Element element)
    {
        //Check if valid name passed and return int
        if (element == Element.None)
        {
            Debug.LogError($"Incorrect element name passed -- InventoryManager Check: {element}");
            return 0;
        }
        return _elements[element];
    }
}
