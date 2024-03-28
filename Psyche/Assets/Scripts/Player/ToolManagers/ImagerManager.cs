/*
 * Description: Multispectral Imager Tool
 * Authors: mcmyers4
 * Version: 20240119
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Imager tool script with spotlight surrounding the character
/// and a spotlight that follows the user's mouse cursor
/// with the ability to increase the radius of each spotlight.
/// </summary>
public class ImagerManager : ToolManager
{
    //Private Variables
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _playerlight; //The player's imager - Not initialized through code
    [SerializeField] private bool _active;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _flashlight; //The cursor's imager - Not initialized through code
    Vector3 cursorPoint; //Mouse cursor location
    public float speed = 1f; //Speed to match cursor
    [SerializeField] private float _radiusIncrease; //The amount to increase the vision field

    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerController playerManagement)
    {
        //Base class variables
        ToolName = "Imager";
        ToolEnabled = false;
        PlayerController = playerManagement;
        _flashlight.intensity = 0f;
        Level = 1;
        LevelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  6, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  7, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  8, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 2 },
                }
            },
            {  9, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 4 },
                }
            },
            {  10, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 0 }, { InventoryManager.Element.Iron, 0 },     
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 4 },
                }
            },
        };

        //Tool specific variables
        MaxLevel = LevelRequirements.Count + 1;
        _radiusIncrease = 1f;
        _active = false;
    }

    /// <summary>
    /// Inherited,
    /// currently not in use
    /// </summary>
    public override void Activate()
    {
        _active = !PlayerController.solarArrayManager.batteryDrained;
        if (_active)
        {
            _flashlight.intensity = 1;
        }
        else
        {
            _flashlight.intensity = 0;
        }
    }

    /// <summary>
    /// Increases the radius of both playerlight and flashlight when called
    /// </summary>
    protected override void UpgradeTool()
    {
        //Increase player's spotlight radius
        //_playerlight.pointLightInnerRadius += _radiusIncrease;
        _playerlight.pointLightOuterRadius += _radiusIncrease;
        //Increase cursor's spotlight radius
        //_flashlight.pointLightInnerRadius += _radiusIncrease;
        _flashlight.pointLightOuterRadius += _radiusIncrease;
    }

    /// <summary>
    /// Turns on flashlight.
    /// Flashlight tracks and then moves to user's mouse cursor.
    /// </summary>
    public void updateFlashlightPosition()
    {
        _flashlight.intensity = 1;
        cursorPoint = Input.mousePosition;
        cursorPoint.z = speed;
        _flashlight.transform.position = Camera.main.ScreenToWorldPoint(cursorPoint);
    }
}
