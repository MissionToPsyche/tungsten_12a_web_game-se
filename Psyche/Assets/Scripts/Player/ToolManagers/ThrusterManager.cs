/*
 * Description: Thruster tool script
 * Authors: JoshBenn, mcmyers4
 * Version: 20240403
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thruster tool class in which the player can hold jump in order to fly or slow falls
/// </summary>
public class ThrusterManager : ToolManager
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private Variables
    [SerializeField] private float Limit = 4;
    private float ThrusterForce;
    private float Duration;
    private bool Working = true;

    /// <summary>
    /// Initializes the tool
    /// </summary>
    /// <param name="playerController"></param>
    public void Initialize(PlayerController playerController)
    {
        // Base class variables
        ToolName = "Thruster";
        ToolEnabled = false;
        PlayerController = playerController;
        Level = 1;
        LevelRequirements = new Dictionary<int, Dictionary<InventoryManager.Element, ushort>>()
        {
            {  2, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 2 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 0 },
                }
            },
            {  3, new Dictionary<InventoryManager.Element, ushort>()
                {
                    { InventoryManager.Element.Copper, 2 },
                    { InventoryManager.Element.Iron, 0 }, 
                    { InventoryManager.Element.Nickel, 0 },
                    { InventoryManager.Element.Gold, 0 },
                }
            }
        };

        // Tool specific variables
        MaxLevel = LevelRequirements.Count + 1;
        ThrusterForce = 0.9f;
    }

    /// <summary>
    /// Countdown timer to limit thruster use
    /// </summary>
    private void Update()
    {
        if (Duration > 0)
        {
            Duration--;
            Working = true;
        }
        else
        {
            Duration = 0;
            Working = false;
        }
    }

    //=============================================== Tool Actions ===============================================

    /// <inheritdoc/>
    /// Activates the thruster applying a vertical force if time remains on timer
    public override void Activate()
    {
        if (Working)
        {
            PlayerController.playerCharacter.velocity += new Vector2(0f, ThrusterForce * Time.deltaTime * 10f);
        }
        else
        {
            Duration = Limit;
        }
    }

    /// <inheritdoc/>
    /// Increases thruster force when called
    protected override void UpgradeTool()
    {
        ThrusterForce += 0.25f;
    }
}
