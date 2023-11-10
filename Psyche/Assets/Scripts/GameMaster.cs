using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles overarching game logic and initialization
/// </summary>
public static class GameMaster 
{
    // Static reference to controller classes
    //public static PlayerController playerController { get; private set; }
    public static GameController gameController { get; private set; }
    public static UIController uiController { get; private set; }

    public static void Initialize()
    {
        // Initialize the game components.
        //playerController = new PlayerController();
        gameController = new GameController();
        //uiController = new UIController();

        // Additional initialization logic can go here.
    }

    public static void StartGame()
    {
        // Start the game or perform any other necessary actions.
    }
}
