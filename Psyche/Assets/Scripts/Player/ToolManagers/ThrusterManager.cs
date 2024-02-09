/*
 * Description: Electric Hall Thruster Tool
 * Authors: joshbenn, mcmyers4
 * Version: 20240119
 */

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
        levelRequirements = new Dictionary<int, Dictionary<string, ushort>>()
        {
            {  1, new Dictionary<string, ushort>()
                {
                    { "element_copper", 1 }, { "element_iron", 0 }, { "element_nickel", 0 }, { "element_gold", 0 }, { "element_platinum", 0 }
                }
            },
            {  2, new Dictionary<string, ushort>()
                {
                    { "element_copper", 2 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 0 }, { "element_platinum", 0 }
                }
            },
            {  3, new Dictionary<string, ushort>()
                {
                    { "element_copper", 3 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 0 }, { "element_platinum", 0 }
                }
            },
            {  4, new Dictionary<string, ushort>()
                {
                    { "element_copper", 4 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 0 }, { "element_platinum", 0 }
                }
            },
            {  5, new Dictionary<string, ushort>()
                {
                    { "element_copper", 5 } , { "element_iron", 0 } , { "element_nickel", 0 } , { "element_gold", 0 }, { "element_platinum", 0 }
                }
            },
        };

        //Tool specific variables
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
