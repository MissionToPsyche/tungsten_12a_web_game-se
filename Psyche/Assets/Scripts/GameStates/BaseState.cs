using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    //Use C#'s <object> type to store generic values (bool, int, etc.)
    protected Dictionary<string, object> _objectStates;

    /// <summary>
    /// Create the base class
    /// </summary>
    public BaseState()
    {
        _objectStates = new Dictionary<string, object>();
    }
}
