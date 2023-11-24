using System.Collections;
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
        levelRequirements = new Dictionary<int, Dictionary<string, int>>()
        {
            {  1, new Dictionary<string, int>()
                {
                    { "copper", 1 }, { "iron", 0 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  2, new Dictionary<string, int>()
                {
                    { "copper", 2 }, { "iron", 0 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  3, new Dictionary<string, int>()
                {
                    { "copper", 3 }, { "iron", 0 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  4, new Dictionary<string, int>()
                {
                    { "copper", 4 }, { "iron", 0 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
                }
            },
            {  5, new Dictionary<string, int>()
                {
                    { "copper", 5 }, { "iron", 0 }, { "nickel", 0 }, { "gold", 0 }, { "titanium", 0 }
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
