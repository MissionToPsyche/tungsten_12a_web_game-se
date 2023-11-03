using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the elements and their respective tracking
/// </summary>
public class ElementManager : MonoBehaviour
{
    //Private variables
    [SerializeField] private int _copper, _iron, _nickel, _gold, _platinum;
    private PlayerManagement _playerManagement;

    /// <summary>
    /// Initialize this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        _playerManagement = playerManagement;
        _copper = 0;
        _iron = 0;
        _nickel = 0;
        _gold = 0;
        _platinum = 0;
    }

    /// <summary>
    /// When the player walks over the object, pick up the object
    /// </summary>
    public void ElementPickUp (string element)
    {
        if (element == "Element_Copper")
            _copper++;
        if (element == "Element_Iron")
            _iron++;
        if (element == "Element_Nickel")
            _nickel++;
        if (element == "Element_Gold")
            _gold++;
        if (element == "Element_Platinum")
            _platinum++;
    }

    /// <summary>
    /// Modifies the tool when a given button is pressed
    /// </summary>
    public void ModifyTool (ToolManager toolManager)
    {
        switch(toolManager.toolName)
        {
            case "Thruster" when _copper > 0:
                toolManager.Modify();
                _copper--;
                break;
            case "Battery" when _nickel > 0:
                toolManager.Modify();
                _nickel--;
                break;
            case "Electromagnet" when _iron > 0:
                toolManager.Modify();
                _iron--;
                break;
            case "Imager" when _gold > 0:
                toolManager.Modify();
                _gold--;
                break;
            default:
                Debug.Log("Not enough of that element...");
                break;
        }
        Debug.Log("Copper: " + _copper + " Iron: " + _iron + " Nickel: " + _nickel + " Gold: " + _gold + " Platinum: " + _platinum);
    }
}
