using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to manage the behavior of the Thruster tool
/// </summary>
public class ThrusterManager : ToolManager
{
    //Private Variables
    private float _thrusterForce;
    [SerializeField] private float limit = 4;
    private float duration;
    private bool working = true;

    /// <summary>
    /// Countdown timer
    /// </summary>
    private void Update()
    {
        if (duration > 0)
        {
            duration--;
            working = true;
        } else
        {
            duration = 0;
            working = false;
        }
    }

    /// <summary>
    /// Initializes the tool
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        //Base class variables
        toolName = "Thruster";
        toolEnabled = false;
        _playerController = playerController;
        level = 0;
        levelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  1, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 2 }, { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 }, { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 2 } , { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 } , { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 3 } , { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 } , { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 4 } , { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 } , { InventoryManager.Element.GOLD, 0 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.COPPER, 5 } , { InventoryManager.Element.IRON, 0 }, 
                    { InventoryManager.Element.NICKEL, 0 } , { InventoryManager.Element.GOLD, 0 },
                }
            },
        };

        //Tool specific variables
        maxLevel = levelRequirements.Count;
        _thrusterForce = 0.9f;
    }
    
    /// <summary>
    /// Activates the thruster applying a vertical force if time remains on timer
    /// </summary>
    /// <param name="playerCharacter"></param>
    public override void Activate()
    {
        if (working)
        {
            _playerController.playerCharacter.velocity += new Vector2(0f, _thrusterForce * Time.deltaTime * 10f);
        } else
        {
            duration = limit;
        }
    }

    /// <summary>
    /// Increases thruster force
    /// </summary>
    protected override void UpgradeTool()
    {
        _thrusterForce += 0.25f;
    }
}
