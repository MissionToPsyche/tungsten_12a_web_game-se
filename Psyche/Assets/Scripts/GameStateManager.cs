using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager 
{
    //Private variables
    private GameController _gameController;

    // Public variables
    public GameState currentState { get; private set; } //state of the game

    public GameStateManager(GameController gameController, GameState state)
    {
        _gameController = gameController;
        currentState = state;
    }


    //Enum to hold various gamestates
    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        GameOver
    }


    public void ChangeGameState(GameState state)
    {
        currentState = state;
        _gameController.HandleGameStateEvent(currentState);
    }
}
