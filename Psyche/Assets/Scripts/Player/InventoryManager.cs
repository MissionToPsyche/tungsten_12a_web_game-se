/*
 * Authors: JoshBenn
 */
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the inventory for the player character.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    //============================================== Initialize/Updates/Destroy ==============================================
    // Private variables
    private PlayerController PlayerController;
    private Dictionary<Tool, bool> Tools;
    private Dictionary<Element, ushort> Elements;
    

    /// <summary>
    /// Initialize the class and set up base dictionaries
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        PlayerController = playerController;
        Tools = new Dictionary<Tool, bool>()
        {
            { Tool.Imager,          false },
            { Tool.SolarArray,      false },
            { Tool.Spectrometer,    false },
            { Tool.Thruster,        false },
            { Tool.Electromagnet,   false },
        };
        Elements = new Dictionary<Element, ushort>()
        {
            { Element.Copper,     0 },
            { Element.Iron,       0 },
            { Element.Nickel,     0 },
            { Element.Gold,       0 },
            { Element.Tungsten,   0 },
        };
    }

    //================================================== Events ==================================================
    public event Action<Tool, bool> OnUpdateInventoryTool;
    public event Action<Element, ushort> OnUpdateInventoryElement;
    public event Action<string, object> SetObjectState;

    //=================================================== Enum ===================================================
    /// <summary>
    /// The various elements used within the game.
    /// </summary>
    public enum Element
    {
        Copper,
        Iron,
        Nickel,
        Gold,
        Tungsten,

        None,
    }

    /// <summary>
    /// Matches a given string with the <see cref="Element"/> variant
    /// </summary>
    /// <param name="element"><see cref="string"/> element name</param>
    /// <returns><see cref="Element"/> variant</returns>
    public Element MatchElement(string element)
    {
        return element.ToLower() switch
        {
            "copper" or "element_copper"        => Element.Copper,
            "iron" or "element_iron"            => Element.Iron,
            "nickel" or "element_nickel"        => Element.Nickel,
            "gold" or "element_gold"            => Element.Gold,
            "tungsten" or "element_tungsten"    => Element.Tungsten,
            _                                   => Element.None
        };
    }

    /// <summary>
    /// Matches a given <see cref="Element"/> variant with a string value.
    /// </summary>
    /// <param name="element"><see cref="Element"/> variant</param>
    /// <returns><see cref="string"/> or <see cref="null"/> if no match is found</returns>
    public string MatchElement(Element element)
    {
        return element switch
        {
            Element.Copper      => "element_copper",
            Element.Iron        => "element_iron",
            Element.Nickel      => "element_nickel",
            Element.Gold        => "element_gold",
            Element.Tungsten    => "element_tungsten",
            _                   => null
        };
    }

    /// <summary>
    /// The various tools used within the game.
    /// </summary>
    public enum Tool
    {
        Imager,
        SolarArray,
        Spectrometer,
        Thruster,
        Electromagnet,

        None,
    }

    /// <summary>
    /// Matches a given string with the <see cref="Tool"/> variant
    /// </summary>
    /// <param name="tool"><see cref="string"/> tool name</param>
    /// <returns><see cref="Tool"/> variant</returns>
    public Tool MatchTool(string tool)
    {
        return tool.ToLower() switch
        {
            "imager" => Tool.Imager,
            "solararray" => Tool.SolarArray,
            "spectrometer" => Tool.Spectrometer,
            "thruster" => Tool.Thruster,
            "electromagnet" => Tool.Electromagnet,
            _ => Tool.None,
        };
    }

    /// <summary>
    /// Matches a given <see cref="Tool"/> variant with a string value.
    /// </summary>
    /// <param name="tool"><see cref="Tool"/> variant</param>
    /// <returns><see cref="string"/> or <see cref="null"/> if no match is found</returns>
    public string MatchTool(Tool tool)
    {
        return tool switch
        {
            Tool.Imager => "Imager",
            Tool.SolarArray => "SolarArray",
            Tool.Spectrometer => "Spectrometer",
            Tool.Thruster => "Thruster",
            Tool.Electromagnet => "Electromagnet",
            _ => null,
        };
    }

    //======================================================= Inventory ======================================================

    /// <summary>
    /// Activates the tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void ToolPickUp(string toolName)
    {
        // Parse the tool, return if None
        Tool tool = MatchTool(toolName);
        if (tool == Tool.None)
        {
            return;
        }
        SetTool(tool, true);

        // Remove the object from the scene state
        SetObjectState?.Invoke(toolName, false);
        
        // Enable the tool and activate any other actions involved
        switch (tool)
        {
            case Tool.SolarArray:
                PlayerController.solarArrayManager.Enable();
                break;

            case Tool.Thruster:
                PlayerController.thrusterManager.Enable();
                break;

            case Tool.Imager:
                PlayerController.imagerManager.Enable();
                break;

            case Tool.Spectrometer:
                break;

            case Tool.Electromagnet:
                PlayerController.eMagnetManager.Enable();
                SetObjectState?.Invoke("First_Veins", true);
                break;
        }
    }

    /// <summary>
    /// Set the tool as obtained or not obtained
    /// </summary>
    /// <param name="tool"><see cref="Tool"/> variant</param>
    /// <param name="value"><see cref="bool"/></param>
    public void SetTool(Tool tool, bool value)
    {
        // Ensure the tool exists
        if (tool == Tool.None)
        {
            return;
        }
        Tools[tool] = value;

        // Update events
        OnUpdateInventoryTool?.Invoke(tool, value);
    }

    /// <summary>
    /// Checks if the tool has been picked up.
    /// </summary>
    /// <param name="tool"><see cref="Tool"/></param>
    public bool CheckTool(Tool tool)
    {
        //Check if valid name passed and return boolean
        if (tool == Tool.None)
        {
            return false;
        }
        return Tools[tool];
    }

    /// <summary>
    /// Checks if the tool has been picked up.
    /// </summary>
    /// <param name="toolName"><see cref="string"/></param>
    public bool CheckTool(string toolName)
    {
        Tool tool = MatchTool(toolName);
        //Check if valid name passed and return boolean
        if (tool == Tool.None)
        {
            return false;
        }
        return Tools[tool];
    }

    /// <summary>
    /// Adds a number of elements.
    /// </summary>
    /// <param name="item"><see cref="string"/> element name</param>
    /// <param name="amount"><see cref="ushort"/> element amount</param>
    public void AddElement(string item, ushort amount)
    {
        // Split the incoming value from its element name and element ID
        string[] element_set = item.Split(' ');

        // Check if valid name passed and return int
        Element element = MatchElement(element_set[0]);
        
        // If the element is tungsten, updates the UI for the element at the current scene
        if (element == Element.Tungsten) {
            GameStateManager.Scene currentScene = GameController.Instance.gameStateManager.currentScene;
            UIController.Instance.UpdateCapturedTungstens(currentScene);
        }

        // Update the dictionary
        if (!Elements.ContainsKey(element))
        {
            return;
        }
        Elements[element] += amount;

        // Update events
        SetObjectState?.Invoke(item, false);
        OnUpdateInventoryElement?.Invoke(element, Elements[element]);
    }

    /// <summary>
    /// Removes a number of elements from the inventory.
    /// </summary>
    /// <param name="element"><see cref="Element"/> variant</param>
    /// <param name="amount"><see cref="ushort"/> amount</param>
    public void RemoveElement(Element element, ushort amount)
    {
        //Check if valid name passed and return int
        if (!Elements.ContainsKey(element))
        {
            return;
        }
        Elements[element] -= amount;

        // Update events
        OnUpdateInventoryElement?.Invoke(element, Elements[element]);
    }

    /// <summary>
    /// Sets the amount of an element
    /// </summary>
    /// <param name="element"><see cref="Element"/> variant</param>
    /// <param name="amount"><see cref="ushort"/> amount</param>
    public void SetElement(Element element, ushort amount)
    {
        //Check if valid name passed and return int
        if (element == Element.None) 
        { 
            return; 
        }
        Elements[element] = amount;

        // Update events
        OnUpdateInventoryElement?.Invoke(element, Elements[element]);
    }

    /// <summary>
    /// Returns the number of elements the player has
    /// </summary>
    /// <param name="element"><see cref="Element"/> variant</param>
    /// <returns><see cref="ushorrt"/> number of that element</returns>
    public ushort CheckElement(Element element)
    {
        //Check if valid name passed and return int
        if (element == Element.None)
        {
            return 0;
        }
        return Elements[element];
    }
}