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
        level = 1;
        levelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 2 }, { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 }, { InventoryManager.Element.Gold, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 2 } , { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 } , { InventoryManager.Element.Gold, 0 },
                }
            },
            {  4, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 3 } , { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 } , { InventoryManager.Element.Gold, 0 },
                }
            },
            {  5, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 4 } , { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 } , { InventoryManager.Element.Gold, 0 },
                }
            },
        };

        //Tool specific variables
        maxLevel = levelRequirements.Count + 1;
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
