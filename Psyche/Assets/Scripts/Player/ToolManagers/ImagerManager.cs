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
        toolName = "Imager";
        toolEnabled = false;
        _playerController = playerManagement;
        _flashlight.intensity = 0f;
        level = 0;
        levelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  1, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 2 },
                }
            },
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  6, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  7, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  8, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  9, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 },     
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
            {  10, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 0 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 4 },
                }
            },
        };

        //Tool specific variables
        maxLevel = levelRequirements.Count;
        _radiusIncrease = 1f;
        _active = false;
    }

    /// <summary>
    /// Inherited,
    /// currently not in use
    /// </summary>
    public override void Activate()
    {
        _active = !_playerController.batteryManager.batteryDrained;
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
        GameController.Instance.audioManager.PlayAudio(
            GameController.Instance.audioManager.toolImager);
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
