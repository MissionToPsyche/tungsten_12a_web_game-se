using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public class DeveloperConsole : MonoBehaviour
{
    //Private variables
    private GameController _gameController;
    private Dictionary<DevConsoleCommand, Action<ArrayList>> _commandRegistry; //stores commands
    private float _updateTimerDefault;
    private float _updateTimer;
    private ProfilerRecorder _totalUsedMemory;

    //Events Declaration
    public event Action<ArrayList>      OnDevConsoleUIUpdate;       // Updating the UI communication
    public event Action<string, string> OnDevConsolePlayerSet; // Player controller changes
    public event Action<ArrayList> OnDevConsoleSceneSet;       // Setting the scene

    // Enum for Commands
    public enum DevConsoleCommand
    {
        TOGGLE,  // For Updating UI Controller && Intaking commands
        SET,     // Intaking command
        UPDATE,  // For Updating the UIController

        FPS,
        RESOURCE_MONITOR,

        ERROR,
    }

    /// <summary>
    /// For translating between the given command (enum / short) and its respective string
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public string Match(DevConsoleCommand command)
    {
        switch (command)
        {
            case DevConsoleCommand.TOGGLE:              return "toggle";
            case DevConsoleCommand.SET:                 return "set";
            case DevConsoleCommand.UPDATE:              return "update";
                    
            case DevConsoleCommand.FPS:                 return "fps";
            case DevConsoleCommand.RESOURCE_MONITOR:    return "resource_monitor";

            default: return null;
        }
    }

    /// <summary>
    /// For translating between the given command (string) and its respective enum / short
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public DevConsoleCommand Match(string command)
    {
        switch (command.ToLower())
        {
            case "toggle":              return DevConsoleCommand.TOGGLE;
            case "set":                 return DevConsoleCommand.SET;
            case "update":              return DevConsoleCommand.UPDATE;

            case "fps":                 return DevConsoleCommand.FPS;
            case "resource_monitor":    return DevConsoleCommand.RESOURCE_MONITOR;

            default:                    return DevConsoleCommand.ERROR;
        }
    }


    /// <summary>
    /// Creates the script
    /// </summary>
    /// <param name="gameController"></param>
    public void Initialize(GameController gameController)
    {
        _gameController = gameController;
        _commandRegistry = new Dictionary<DevConsoleCommand, Action<ArrayList>>()
        {
            { DevConsoleCommand.TOGGLE, HandleToggle },
            { DevConsoleCommand.SET, HandleSet },
        };
        _updateTimerDefault = 0.5f;  //every 1/2 second
        _updateTimer = _updateTimerDefault;
        _totalUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
    }

    private void OnDestroy()
    {
        _totalUsedMemory.Dispose();
    }

    /// <summary>
    /// Update runs once every frame
    /// </summary>
    private void Update()
    {
        //Update once every _updateTimerDefault seconds
        _updateTimer -= Time.deltaTime;

        if (_updateTimer <= 0f)
        {
            //create package to sender
            ArrayList args = new ArrayList {
                DevConsoleCommand.UPDATE, CalculateFPS().ToString(),  CalculateRAM(),
            };
            //Send the message
            OnDevConsoleUIUpdate(args);
            _updateTimer = _updateTimerDefault; //reset back to default
        }
    }

    private void OnEnable()
    {
        UIController.Instance.OnUpdateUIToDevConsole += HandleCommands;
    }

    private void OnDisable()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.OnUpdateUIToDevConsole -= HandleCommands;
        }
    }

    /// <summary>
    /// General Event intake manager
    /// - args[0] = source
    /// </summary>
    /// <param name="args"></param>
    public void IntakeEvents(ArrayList args)
    {
        string source = args[0].ToString();
        args.RemoveAt(0);

        switch(source)
        {
            case "UI":
                HandleCommands(args); 
                break;
            default:
                UnityEngine.Debug.Log("Incorrect source -- DevConsole");
                break;
        }
    }

    /// <summary>
    /// Processes commands
    /// </summary>
    /// <param name="commands"></param>
    public void HandleCommands(ArrayList commands)
    {
        var command = Match(commands[0].ToString());

        if (command == DevConsoleCommand.ERROR)
        {
            UnityEngine.Debug.Log("Incorrect command passed to console -- DevConsole");
            return;
        }
        _commandRegistry[command].Invoke(commands);
    }

    /// <summary>
    /// Handles toggling UI components on and off
    /// </summary>
    /// <param name="commands"></param>
    private void HandleToggle(ArrayList commands)
    {
        OnDevConsoleUIUpdate(commands);  //send to event emitter in controller class
    }

    /// <summary>
    /// Handles setting variable values
    /// - set [tool/item] [value]
    /// </summary>
    /// <param name="commands"></param>
    private void HandleSet(ArrayList commands)
    {
        string[] package = { "Player", "InventoryManager", "DeveloperConsole" };           ///////////// Update this
        commands.InsertRange(0, package);
        _gameController.SendMessage(commands);
    }

    /// <summary>
    /// Calculates the FPS when called
    /// </summary>
    /// <returns></returns>
    private double CalculateFPS()
    {
        return Math.Round(1.0f / Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Calculates RAM usage and returns in MB
    /// </summary>
    /// <returns></returns>
    private double CalculateRAM()
    {
        return Math.Round((_totalUsedMemory.LastValueAsDouble / (1024 * 1024)), 2);
    }
}
