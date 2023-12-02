using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test integration for a gamestate class to manage the state of a scene
/// </summary>
public class GameStateTest
{
    //Use C#'s <object> type to store generic values (bool, int, etc.)
    private Dictionary<string, object> objectStates;

    /// <summary>
    /// Constructor for the gamestate
    /// </summary>
    public GameStateTest()
    {
        objectStates = new Dictionary<string, object>();
    }

    /// <summary>
    /// Holds the default game state for the scene, allowing for a state reset
    /// </summary>
    public void LoadDefaultGameState()
    {

    }

    /// <summary>
    /// Returns the state of the scene
    /// - Must cast the objects into their respective types (e.g. (int))
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, object> GetGameState()
    {
        return objectStates;
    }
}
