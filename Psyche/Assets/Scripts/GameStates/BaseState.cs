using System;
using System.Collections.Generic;


public class BaseState
{
    //Use C#'s <object> type to store generic values (bool, int, etc.)
    protected Dictionary<string, object> _defaultState;
    protected Dictionary<string, object> _sceneState;
    protected Dictionary<string, object> _savedState;

    /// <summary>
    /// Create the base class
    /// </summary>
    public BaseState() { }

    /// <summary>
    /// Holds the default game state for the scene, allowing for a state reset
    /// </summary>
    public void LoadDefaultState() 
    {
        _sceneState = new Dictionary<string, object>();

        foreach (var pair in _defaultState)
        {
            if (pair.Value is ICloneable clone)
            {
                _sceneState[pair.Key] = clone.Clone();
            }
            else
            {
                _sceneState[pair.Key] = pair.Value;
            }
        }
    }

    /// <summary>
    /// Saves the current state of the scene
    /// </summary>
    public void SaveState() 
    {
        _savedState = new Dictionary<string, object>();

        foreach (var pair in _sceneState)
        {
            if (pair.Value is ICloneable clone)
            {
                _savedState[pair.Key] = clone.Clone();
            }
            else
            {
                _savedState[pair.Key] = pair.Value;
            }
        }
    }

    /// <summary>
    /// Loads the saved state into the scene
    /// </summary>
    public virtual void LoadState() { }

    /// <summary>
    /// Sets the state of an object in the scene
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetObjectState(string key, object value) 
    {
        _sceneState[key] = value;
    }
}
