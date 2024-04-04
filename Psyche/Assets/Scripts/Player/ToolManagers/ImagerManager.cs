/*
 * Description: Multispectral imager tool script
 * Authors: mcmyers4
 * Version: 20240403
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imager tool class with a spotlight surrounding the character and a spotlight that follows the user's mouse cursor
/// with the ability to increase the radius of each spotlight.
/// </summary>
public class ImagerManager : ToolManager
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private Variables
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D Playerlight; // The player's imager - Not initialized through code
    [SerializeField] private bool Active;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D Flashlight; // The cursor's imager - Not initialized through code
    [SerializeField] private float RadiusIncrease; // The amount to increase the vision field
    private Vector3 CursorPoint; // Mouse cursor location

    // Public variables
    public float Speed = 1f; // Speed to match cursor

    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerController playerManagement)
    {
        // Base class variables
        ToolName = "Imager";
        ToolEnabled = false;
        PlayerController = playerManagement;
        Flashlight.intensity = 0f;
        Level = 1;
        LevelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  6, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  7, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  8, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 2 },
                }
            },
            {  9, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 4 },
                }
            },
            {  10, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 },
                    { InventoryManager.Element.Iron, 0 },     
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 4 },
                }
            },
        };

        // Tool specific variables
        MaxLevel = LevelRequirements.Count + 1;
        RadiusIncrease = 1f;
        Active = false;
    }

    //=============================================== Tool Actions ===============================================

    /// <summary>
    /// Inherited
    /// </summary>
    public override void Activate()
    {
        Active = !PlayerController.solarArrayManager.BatteryDrained;

        if (Active)
        {
            Flashlight.intensity = 1;
        }
        else
        {
            Flashlight.intensity = 0;
        }
    }

    /// <summary>
    /// Increases the radius of both playerlight and flashlight when called
    /// </summary>
    protected override void UpgradeTool()
    {
        Playerlight.pointLightOuterRadius += RadiusIncrease;
        Flashlight.pointLightOuterRadius += RadiusIncrease;
    }

    /// <summary>
    /// Turns on flashlight.
    /// Flashlight tracks and then moves to user's mouse cursor.
    /// </summary>
    public void UpdateFlashlightPosition()
    {
        Flashlight.intensity = 1;
        CursorPoint = Input.mousePosition;
        CursorPoint.z = Speed;
        Flashlight.transform.position = Camera.main.ScreenToWorldPoint(CursorPoint);
    }
}
