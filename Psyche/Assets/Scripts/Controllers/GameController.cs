using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : BaseController<GameController>
{
    //Private variables
    

    //Public variables
    //public DeveloperConsole developerConsole;
    public GameStateManager gameStateManager;
    //public SceneManager sceneManager;
    //public AudioManager audioManager;


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
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //Implement update logic
        
    }

    /// <summary>
    /// Controller specific update method called by other methods
    /// </summary>
    public override void UpdateController()
    {

    }

    /// <summary>
    /// For shutting down and cleaning up
    /// </summary>
    public override void Shutdown()
    {
        // Perform cleanup if necessary
    }

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
    public event Action<GameStateManager.GameState> OnGameStateChanged;

    // Other stuff here
}
