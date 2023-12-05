using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;
using Unity.Profiling;

public class DeveloperConsole : MonoBehaviour
{
    //Private variables
    private GameController _gameController;
    private Dictionary<string, Action<ArrayList>> _commandRegistry; //stores commands
    private float _updateTimerDefault;
    private float _updateTimer;
    private int _processorCount;
    private float _cpuUsage;
    private Thread _cpuThread;
    private ProfilerRecorder _totalUsedMemory;
    private ProfilerRecorder _mainThread;
    private Queue<double> _frameTimes;
    private const int _frameSampleSize = 100;

    

    //Events Declaration
    public event Action<string> OnDevConsoleUIUpdate;  //dev console updates UI
    public event Action<string, string> OnDevConsolePlayerSet; //player controller changes

    /// <summary>
    /// Creates the script
    /// </summary>
    /// <param name="gameController"></param>
    public void Initialize(GameController gameController)
    {
        _gameController = gameController;
        _commandRegistry = new Dictionary<string, Action<ArrayList>>()
        {
            { "toggle", HandleToggle },
            { "set", HandleSet },
        };
        _updateTimerDefault = 0.5f;  //every 1/2 second
        _updateTimer = _updateTimerDefault;
        _cpuUsage = 0f;
        _cpuThread = new Thread(MaintainQueue);
        _processorCount = SystemInfo.processorCount;
        _totalUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        _mainThread = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread");
        _frameTimes = new Queue<double>();

        _cpuThread.Start();
    }

    private void OnDestroy()
    {
        _cpuThread?.Abort();
        _totalUsedMemory.Dispose();
    }

    /// <summary>
    /// Update runs once every frame
    /// </summary>
    private void Update()
    {
        _frameTimes.Enqueue(_mainThread.LastValue);
        //Update once every _updateTimerDefault seconds
        _updateTimer -= Time.deltaTime;

        if (_updateTimer <= 0f)
        {
            //create package to sender
            ArrayList args = new ArrayList {
                "UI", "None", "DeveloperConsole", "update",
                CalculateFPS().ToString(),  Math.Round(CalculateCPU()).ToString(), CalculateRAM(),
            };
            //Send the message
            _gameController.SendMessage(args);
            _updateTimer = _updateTimerDefault; //reset back to default
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
        if (!_commandRegistry.ContainsKey(commands[0].ToString()))
        {
            UnityEngine.Debug.Log("Incorrect command passed to console -- DevConsole");
            return;
        }
        _commandRegistry[commands[0].ToString()].Invoke(commands);
    }

    /// <summary>
    /// Handles toggling UI components on and off
    /// </summary>
    /// <param name="commands"></param>
    private void HandleToggle(ArrayList commands)
    {
        string[] package = { "UI", "None", "DeveloperConsole" }; //receiver address
        commands.InsertRange(0, package);
        _gameController.SendMessage(commands);  //send to event emitter in controller class
    }

    /// <summary>
    /// Handles setting variable values
    /// - set [tool/item] [value]
    /// </summary>
    /// <param name="commands"></param>
    private void HandleSet(ArrayList commands)
    {
        string[] package = { "Player", "InventoryManager", "DeveloperConsole" };
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

    private void MaintainQueue()
    {
        while(_frameTimes.Count > _frameSampleSize)
        {
            _frameTimes.Dequeue();
        }
    }

    /// <summary>
    /// Calculates the ms per frame to show general CPU usage
    /// </summary>
    /// <returns></returns>
    private double CalculateCPU()
    {
        return ((_frameTimes.Average() / TimeSpan.TicksPerMillisecond) / 1000) * _processorCount * 2;
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
