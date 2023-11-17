using UnityEngine;

/// <summary>
/// For managing color blind mode until GameController is implemented
/// </summary>
/// Author: jmolive8
public class TempGameManager : MonoBehaviour
{
    [HideInInspector] public static TempGameManager Instance;

    /// <summary>
    /// T/F for color blind mode
    /// </summary>
    public static bool colorBlindMode;

    /// <summary>
    /// When transitioning between scenes, ensures TempGameManager state remains
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Dev TempGameManager still in");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Swap color blind mode
    /// </summary>
    /// <returns></returns>
    public static void ChangeColorBlindMode(bool mode)
    {
        colorBlindMode = mode;
    }
}
