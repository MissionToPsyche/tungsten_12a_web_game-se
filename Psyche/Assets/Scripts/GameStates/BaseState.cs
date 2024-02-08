using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Base class for SceneStates to be based on.
/// -- Implements the methods: LoadState(), SaveState(), and SetObjectState()\
/// -- Objects are trackd via Dictionaries.  Key: `string` = Object's name || Value: `object` = tracked object state
///     -- For all `bool` items, `true` means the item can be picked up or the item has yet to be activated (such as checkpoints)
///     -- For all positions, these can be tracked via a vector
/// </summary>
public abstract class BaseState
{
    //Use C#'s <object> type to store generic values (bool, int, etc.)
    protected Dictionary<short, object> _defaultState;
    protected Dictionary<short, object> _sceneState;
    protected Dictionary<short, object> _savedState;

    /// <summary>
    /// Create the base class -- Unnecessary in this current iteration
    /// </summary>
    public BaseState() { }

    public abstract short Match(string obj);
    public abstract string Match(short obj);

    /// <summary>
    /// Holds the default game state for the scene, allowing for a state reset
    /// -- Values are cloned over so they aren't passed by referenced and then modified at a later time
    /// </summary>
    public void LoadDefaultState() 
    {
        _sceneState = new Dictionary<short, object>();

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
    /// -- Values are cloned over so they aren't passed by referenced and then modified at a later time
    /// </summary>
    public void SaveState() 
    {
        _savedState = new Dictionary<short, object>();

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
            //Debug.Log($"Object {Match(pair.Key)} saved with value {pair.Value}");
        }
    }

    /// <summary>
    /// Loads the saved state into the scene
    /// </summary>
    public abstract void LoadState();

    /// <summary>
    /// Sets the state of an object in the scene
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetObjectState(string key, object value)
    {
        //Debug.Log($"Passed in: {key}, {value}");
        short matched_key = Match(key);
        if (matched_key == -1) 
        {
            Debug.Log("No match found for the provided key");
            return;
        }
        _sceneState[matched_key] = value;
        //Debug.Log($"After: {matched_key}, {_sceneState[matched_key]}");
    }
}
