using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller abstract script with generalization for subclasses
/// </summary>
public abstract class BaseController<T> : MonoBehaviour where T : BaseController<T>
{
    //Allows the subclass to implement this logic without the required code
    private static T _instance;

    /// <summary>
    /// Default initialization
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// Singleton pattern to prevent duplication
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"An instance of {typeof(T)} doesn't exist!");
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Virtual Awake() class, implements dontdestroyonload
    /// </summary>
    protected void Awake()
    {
        //Singleton to ensure only one instance exists
        if (_instance == null)
        {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Updates the controller when called
    /// </summary>
    public virtual void UpdateController() { }

    /// <summary>
    /// Shuts down or destroys the controller if implemented
    /// </summary>
    public virtual void Shutdown() { }

    // Example event -- to be added upon later
    public delegate void ControllerEvent();
    public event ControllerEvent OnControllerUpdated;

    /// <summary>
    /// Default event invokation
    /// </summary>
    protected void RaiseControllerUpdatedEvent()
    {
        OnControllerUpdated?.Invoke();
    }

    /// <summary>
    /// Base class initialization method allows for common logic across subclasses
    /// </summary>
    protected virtual void InitializeController()
    {
        
    }


}
