using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller abstract script
/// </summary>
public abstract class BaseController<T> : MonoBehaviour where T : BaseController<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Virtual Awak() class
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
    /// Base class initialization method allows for common logic across subclasses
    /// </summary>
    protected virtual void InitializeController()
    {
        
    }


}
