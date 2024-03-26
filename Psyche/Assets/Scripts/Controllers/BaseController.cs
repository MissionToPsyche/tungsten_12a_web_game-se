/**
 * Authors: JoshBenn
 */
using UnityEngine;

/// <summary>
/// Generic base Controller class with generalization for Controller subclasses implementing Singleton and DontDestroyOnLoad
/// </summary>
public abstract class BaseController<T> : MonoBehaviour where T : BaseController<T>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //Allows the subclass to implement this logic without the required code
    private static T Singleton;

    /// <summary>
    /// Default initialization of the class
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// Accessor for the Singleton Instance -- prevents duplication
    /// </summary>
    public static T Instance
    {
        get
        {
            if (Singleton == null)
            {
                Singleton = FindAnyObjectByType<T>();
            }
            return Singleton;
        }
    }

    /// <summary>
    /// Implementation of DontDestroyOnLoad to prevent object destruction when transitioning between scenes
    /// </summary>
    protected virtual void Awake()
    {
        //Singleton to ensure only one instance exists
        if (Singleton == null || Singleton == this)
        {
            Singleton = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Shuts down and destroys the controller
    /// </summary>
    public virtual void Shutdown() 
    {
        if (Singleton == this)
        {
            Singleton = null;
            Destroy(gameObject);
        }
    }
}
