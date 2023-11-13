using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BaseController<GameController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //Private variables


    //Public variables
    //public DeveloperConsole developerConsole;
    public GameStateManager gameStateManager;
    //public SceneManager sceneManager;
    //public AudioManager audioManager;

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        gameStateManager = new GameStateManager(this, GameStateManager.GameState.MainMenu);
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
        // Perform cleanup if necessary
    }

    //======================================================== Events ========================================================

    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnEnable()
    {
        //Insert logic
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        //Insert logic
    }

    //Gamestate changed
    public event Action<GameStateManager.GameState> OnGameStateChanged;

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
