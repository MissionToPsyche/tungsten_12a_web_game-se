using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BaseController<GameController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //Private variables


    //Public variables
    public DeveloperConsole developerConsole;
    public GameStateManager gameStateManager;
    //public SceneManager sceneManager;
    //public AudioManager audioManager;

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        developerConsole = GetComponent<DeveloperConsole>();
        developerConsole.Initialize(this);
        gameStateManager = new GameStateManager(this, GameStateManager.GameState.MainMenu);
    }

    /// <summary>
    /// Runs when scene starts
    ///   - Calls base.Awake()
    ///   - Initializes script
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    /// <summary>
    /// Called just before first frame
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Called when necessary - not every frame
    /// </summary>
    public override void UpdateController()
    {
        base.UpdateController();
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //Implement update logic

    }

    /// <summary>
    /// For shutting down and cleaning up
    /// </summary>
    public override void Shutdown()
    {
        base.Shutdown();
    }

    //======================================================== Events ========================================================

    //Event Definitions
    public event Action<ArrayList> OnUpdateGameToUI;
    public event Action<ArrayList> OnUpdateGameToPlayer;
    //Gamestate changed
    public event Action<GameStateManager.GameState> OnGameStateChanged;

    /// <summary>
    /// Invokes events for this and any subclasses.
    /// - Takes in an arraylist -- Requirements:
    ///   - ArrayList[0] = destination
    ///   - ArrayList[1] = sub-destination ('None' if Controller)
    ///   - ArrayList[2] = source
    /// </summary>
    /// <param name="args"></param>
    public override void EventInvocation(ArrayList args)
    {
        string destination = args[0].ToString();
        args.RemoveAt(0);
        base.EventInvocation(args);  //even necessary?
        
        //Send out events depending on the invokee
        switch (destination)
        {
            case "UI":
                OnUpdateGameToUI?.Invoke(args);
                break;
            case "Player":
                OnUpdateGameToPlayer.Invoke(args);
                break;
            default:
                Debug.Log("Incorrect invocation in GameController");
                break;
        }
    }


    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnEnable()
    {
        UIController.Instance.OnUpdateUIToGame += ProcessEvent;
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if(UIController.Instance)
        {
            UIController.Instance.OnUpdateUIToGame -= ProcessEvent;
        }
    }

    /// <summary>
    /// Processes any event passed to this class
    /// Requirements:
    ///   - ArrayList[0] = sub-destination ('None' if Controller)
    ///   - ArrayList[1] = source
    /// </summary>
    /// <param name="args"></param>
    private void ProcessEvent(ArrayList args)
    {
        string subdestination = args[0].ToString();
        args.RemoveAt(0);

        switch(subdestination)
        {
            case "None":
                string source = args[0].ToString();
                args.RemoveAt(0);
                //Process command
                break;
            case "DeveloperConsole":
                developerConsole.IntakeEvents(args); 
                break;
            default:
                Debug.Log("Incorrect subdestination -- GameController");
                break;
        }
    }

    

    //======================================================= Gamestate ======================================================

    /// <summary>
    /// Event handling for game state changes
    /// </summary>
    /// <param name="gameState"></param>
    public void HandleGameStateEvent(GameStateManager.GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        // Handle game state updates
        switch (gameStateManager.currentState)
        {
            case GameStateManager.GameState.MainMenu:
                // Update in-game logic
                break;
            case GameStateManager.GameState.InGame:
                // Update pause logic
                break;
            case GameStateManager.GameState.Paused:
                // Pause Logic
                break;
            case GameStateManager.GameState.GameOver:
                // GameOver Logic
                break;
            default:
                Debug.LogError("Game in unknown state!");
                break;

        }
    }
    
    // Other stuff here
}
