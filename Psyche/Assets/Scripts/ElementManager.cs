using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the elements and their respective tracking
/// </summary>
public class ElementManager : MonoBehaviour
{
    //Private variables
    private PlayerManagement _playerManagement;

    //Public variables
    public static ElementManager Instance;
    public int copper, iron, nickel, gold, platinum;

    /// <summary>
    /// Initialize this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        _playerManagement = playerManagement;
        copper = 0;
        iron = 0;
        nickel = 0;
        gold = 0;
        platinum = 0;
    }
    
    /// <summary>
    /// Singleton check
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// When the player walks over the object, pick up the object
    /// </summary>
    public void ElementPickUp (string element)
    {
        if (element == "Element_Copper")
            copper++;
        if (element == "Element_Iron")
            iron++;
        if (element == "Element_Nickel")
            nickel++;
        if (element == "Element_Gold")
            gold++;
        if (element == "Element_Platinum")
            platinum++;
        Debug.Log("Copper: " + copper + " Iron: " + iron + " Nickel: " + nickel + " Gold: " + gold + " Platinum: " + platinum);
    }

    /// <summary>
    /// Modifies the tool when a given button is pressed
    /// </summary>
    public void ModifyTool (ToolManager toolScript)
    {
        switch(toolScript.toolName)
        {
            case "Thruster":
                Debug.Log("Thruster boost | Copper");
                toolScript.Modify();
                copper--;
                break;
            case "Battery":
                Debug.Log("Battery Boost | Nickel");
                toolScript.Modify();
                nickel--;
                break;
            case "Electromagnet":
                Debug.Log("Electromagnet Boost | Iron");
                toolScript.Modify();
                iron--;
                break;
            default:
                Debug.Log("Not enough of that element...");
                break;
        }
        Debug.Log("Copper: " + copper + " Iron: " + iron + " Nickel: " + nickel + " Gold: " + gold + " Platinum: " + platinum);
    }
}