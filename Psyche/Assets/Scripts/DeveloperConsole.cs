using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public class DeveloperConsole : MonoBehaviour
{
    //Private variables
    private GameController _gameController;
    private float _updateTimerDefault;
    private float _updateTimer;
    private ProfilerRecorder _totalUsedMemory;

    //Events Declaration
    private Dictionary<EventSource, bool> _eventSubscriptions = new Dictionary<EventSource, bool>
    {
        { EventSource.UI, false },
    };
    private Dictionary<DevConsoleCommand, bool> _toggledElements = new Dictionary<DevConsoleCommand, bool>()
    {
        { DevConsoleCommand.FPS, false }, { DevConsoleCommand.RESOURCE_MONITOR, false },
    };
    public event Action<ArrayList>  OnDevConsoleUIUpdate;               // Updating the UI communication
    public event Action<string>     OnDevConsoleTransition;             // Transition scenes

    private enum EventSource
    {
        UI,
    }

    // Enum for Commands
    public enum DevConsoleCommand
    {
        TOGGLE,             // For Updating UI Controller && Intaking commands
        SET,                // Intaking command
        UPDATE,             // For Updating the UIController

        SCENE,              // Changing the scene
        ELEMENT,            // Setting the element amount
        TOOL,               // Enabling or disabling a tool
        ALL,

        FPS,                // Toggle FPS on and off
        RESOURCE_MONITOR,   // Resource monitor

        ERROR,              // Invalid command passed
    }

    /// <summary>
    /// For translating between the given command (enum / short) and its respective string
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public string Match(DevConsoleCommand command)
    {
        return command switch
        {
            DevConsoleCommand.TOGGLE              => "toggle",
            DevConsoleCommand.SET                 => "set",
            DevConsoleCommand.UPDATE              => "update",

            DevConsoleCommand.SCENE               => "scene",
            DevConsoleCommand.ELEMENT             => "element",
            DevConsoleCommand.TOOL                => "tool",
            DevConsoleCommand.ALL                 => "all",

            DevConsoleCommand.FPS                 => "fps",
            DevConsoleCommand.RESOURCE_MONITOR    => "resource_monitor",

            _                                     => null,
        };
    }

    /// <summary>
    /// For translating between the given command (string) and its respective enum / short
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public DevConsoleCommand Match(string command)
    {
        return command.ToLower() switch
        {
            "toggle"              => DevConsoleCommand.TOGGLE,
            "set"                 => DevConsoleCommand.SET,
            "update"              => DevConsoleCommand.UPDATE,

            "scene"               => DevConsoleCommand.SCENE,
            "element"             => DevConsoleCommand.ELEMENT,
            "tool"                => DevConsoleCommand.TOOL,
            "all"                 => DevConsoleCommand.ALL,

            "fps"                 => DevConsoleCommand.FPS,
            "resource_monitor"    => DevConsoleCommand.RESOURCE_MONITOR,

            _                     => DevConsoleCommand.ERROR,
        };
    }


    /// <summary>
    /// Creates the script
    /// </summary>
    /// <param name="gameController"></param>
    public void Initialize(GameController gameController)
    {
        _gameController = gameController;
        
        // FPS timer
        _updateTimerDefault = 0.5f;  //every 1/2 second
        _updateTimer = _updateTimerDefault;
        
        // RAM tracker
        _totalUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        
        // Load the event messaging
        _gameController.sceneTransitionManager.LoadDevConsole();
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
        // Load the UI events when active
        if (!_eventSubscriptions[EventSource.UI] && UIController.Instance != null)
        {
            LoadUI();
        }
        // Only perform calculations if necessary
        if (_eventSubscriptions[EventSource.UI]) 
        {
            //Update once every _updateTimerDefault seconds
            _updateTimer -= Time.deltaTime;
            if (_updateTimer <= 0f)
            {
                double fps = (_toggledElements[DevConsoleCommand.FPS]) ? CalculateFPS() : 0.0f;
                double ram = (_toggledElements[DevConsoleCommand.RESOURCE_MONITOR]) ? CalculateRAM() : 0.0f;
                //create package to sender
                ArrayList args = new ArrayList {
                    DevConsoleCommand.UPDATE, fps,  ram,
                };
                //Send the message
                OnDevConsoleUIUpdate?.Invoke(args);
                _updateTimer = _updateTimerDefault; //reset back to default
            }
        }
        

        
    }

    /// <summary>
    /// Load the UI-based events
    /// </summary>
    private void LoadUI()
    {
        UIController.Instance.OnUpdateUIToDevConsole += HandleCommands;
        _eventSubscriptions[EventSource.UI] = true;
    }

    /// <summary>
    /// Upon disable, unsubscribe to all events
    /// </summary>
    private void OnDisable()
    {
        if (UIController.Instance != null)
        {
            UIController.Instance.OnUpdateUIToDevConsole -= HandleCommands;
        }
    }

    /// <summary>
    /// Processes commands
    /// </summary>
    /// <param name="commands"></param>
    public void HandleCommands(ArrayList commands)
    {
        var command = Match(commands[0].ToString());
        
        switch (command)
        {
            case DevConsoleCommand.TOGGLE:
                // Update the currently toggled item(s)
                DevConsoleCommand item = Match(commands[1].ToString());
                if (item == DevConsoleCommand.FPS || item == DevConsoleCommand.RESOURCE_MONITOR)
                {
                    _toggledElements[item] = !_toggledElements[item];
                }

                // Update the UI console
                OnDevConsoleUIUpdate?.Invoke(commands);
                break;
            case DevConsoleCommand.SET:
                var sub_command = Match(commands[1].ToString());
                switch (sub_command)
                {
                    case DevConsoleCommand.ALL:
                        PlayerController.Instance.inventoryManager.SetElement(InventoryManager.Element.COPPER,      100);
                        PlayerController.Instance.inventoryManager.SetElement(InventoryManager.Element.IRON,        100);
                        PlayerController.Instance.inventoryManager.SetElement(InventoryManager.Element.GOLD,        100);
                        PlayerController.Instance.inventoryManager.SetElement(InventoryManager.Element.NICKEL,      100);
                        PlayerController.Instance.inventoryManager.SetElement(InventoryManager.Element.TUNGSTEN,    100);
                        PlayerController.Instance.inventoryManager.SetTool(InventoryManager.Tool.SOLARPANEL, true);
                        PlayerController.Instance.inventoryManager.SetTool(InventoryManager.Tool.SPECTROMETER, true);
                        PlayerController.Instance.inventoryManager.SetTool(InventoryManager.Tool.THRUSTER, true);
                        PlayerController.Instance.inventoryManager.SetTool(InventoryManager.Tool.ELECTROMAGNET, true);
                        PlayerController.Instance.batteryManager.Enable();
                        PlayerController.Instance.thrusterManager.Enable();
                        PlayerController.Instance.eMagnetManager.Enable();
                        PlayerController.Instance.eMagnetActive = true;
                        PlayerController.Instance.imagerManager.Enable();
                        break;
                    // Element Manipulation
                    case DevConsoleCommand.ELEMENT:
                        // Check for a valid element being passed
                        var element = PlayerController.Instance.inventoryManager.MatchElement(commands[2].ToString());
                        if (element == InventoryManager.Element.None)
                        {
                            Debug.Log($"Invalid element name: {commands[2]}");
                            return;
                        }

                        // Check for a valid amount being passed
                        ushort amount;
                        if (commands.Count < 4 || !ushort.TryParse(commands[3].ToString(), out amount))
                        {
                            Debug.Log($"Invalid amount provided");
                            return;
                        }
                        Debug.Log($"{amount}");
                        PlayerController.Instance.inventoryManager.SetElement(element, amount);
                        break;


                    // Tool Manipulation
                    case DevConsoleCommand.TOOL:
                        // Check for valid tool being passed
                        InventoryManager.Tool tool = PlayerController.Instance.inventoryManager.MatchTool(commands[2].ToString());
                        if (tool == InventoryManager.Tool.None)
                        {
                            Debug.Log($"Invalid tool name {commands[2]}");
                            return;
                        }

                        // Check for valid value being passed
                        bool value;
                        if (commands.Count < 4 || !bool.TryParse(commands[3].ToString(), out value))
                        {
                            Debug.Log($"Invalid value passed");
                            return;
                        }
                        // Update the Inventory Manager
                        PlayerController.Instance.inventoryManager.SetTool(tool, value);

                        // Update the specific tool itself
                        switch (tool)
                        {
                            case InventoryManager.Tool.SPECTROMETER:
                                // Currently nothing to do here
                                break;
                            case InventoryManager.Tool.BATTERY:
                                PlayerController.Instance.batteryManager.Enable();
                                break;
                            case InventoryManager.Tool.THRUSTER:
                                PlayerController.Instance.thrusterManager.Enable();
                                break;
                            case InventoryManager.Tool.ELECTROMAGNET:
                                PlayerController.Instance.eMagnetManager.Enable();
                                PlayerController.Instance.eMagnetActive = value;
                                break;
                            case InventoryManager.Tool.IMAGER:
                                PlayerController.Instance.imagerManager.Enable();
                                break;
                            default:
                                Debug.Log($"You somehow broke existence -- DeveloperConsole: {tool}");
                                break;
                        }
                        break;


                    // Scene Manipulation
                    case DevConsoleCommand.SCENE:
                        // Adhoc, convert the scene to enum then back to string until transitioned over to enum passing
                        string scene = GameController.Instance.gameStateManager.MatchScene(GameController.Instance.gameStateManager.MatchScene(commands[2].ToString()));
                        GameController.Instance.sceneTransitionManager.devControl = true;
                        OnDevConsoleTransition?.Invoke((commands.Count > 3) ? scene + " " + commands[3].ToString() : scene);
                        break;
                    default:
                        Debug.Log(
                            $"Incorrect command passed {commands[1]}\n" + 
                            "Please ensure the correct syntax is used: `set <type> <item> <optional: value>`");
                        break;
                }
                break;
            default:
                Debug.Log(
                    "Incorrect command passed to console -- DevConsole\n" + 
                    "Commands:\n" +
                    "\tset <type> <item> <optional: value> || e.g. `set scene thruster` or `set element copper 15`" +
                    "\ttoggle <console_item> <value>       || e.g. `toggle fps on`"
                    );
                break;
        }
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
