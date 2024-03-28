/*
 * Authors: JoshBenn
 */
using System;
using System.Collections.Generic;

/// <summary> 
/// Base class for all <see cref="GameStateManager.Scene"/> states.
/// </summary>
public abstract class BaseState
{
    //======================================== Initialize/Update/Destroy ========================================
    // Private variables
    // Use C#'s <object> type to store generic values (bool, int, etc.)
    protected Dictionary<short, object> DefaultState;
    protected Dictionary<short, object> SceneState;
    protected Dictionary<short, object> SavedState;

    // Public variables
    public abstract short Match(string obj);
    public abstract string Match(short obj);


    //============================================== State Actions ==============================================
    /// <summary>
    /// Holds the default game state for the <see cref="GameStateManager.Scene"/> allowing for a state reset.
    /// </summary>
    public void LoadDefaultState() 
    {
        SceneState = new Dictionary<short, object>();

        foreach (KeyValuePair<short, object> pair in DefaultState)
        {
            if (pair.Value is ICloneable clone)
            {
                SceneState[pair.Key] = clone.Clone();
            }
            else
            {
                SceneState[pair.Key] = pair.Value;
            }
        }
        SaveState();
    }

    /// <summary>
    /// Saves the current state of the  <see cref="GameStateManager.Scene"/>.
    /// </summary>
    public void SaveState() 
    {
        SavedState = new Dictionary<short, object>();

        foreach (KeyValuePair<short, object> pair in SceneState)
        {
            if (pair.Value is ICloneable clone)
            {
                SavedState[pair.Key] = clone.Clone();
            }
            else
            {
                SavedState[pair.Key] = pair.Value;
            }
        }
    }

    /// <summary>
    /// Loads the saved state into the  <see cref="GameStateManager.Scene"/>.
    /// </summary>
    public abstract void LoadState();

    /// <summary>
    /// Sets the state of an object in the  <see cref="GameStateManager.Scene"/>.
    /// </summary>
    /// <param name="key"><see cref="string"/> object name</param>
    /// <param name="value"><see cref="object"/> value to set for the object</param>
    public void SetObjectState(string key, object value)
    {
        short matched_key = Match(key);
        if (SceneState.ContainsKey(matched_key)) { 
            SceneState[matched_key] = value; 
        }
    }
}
