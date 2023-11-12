using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functions for controlling the title screen.
/// Functions are called from the OnClick() event of buttons.
/// Script is a component of the Main Menu gameobject.
/// </summary>
/// Author: jmolive8
public class TitleScreenManager : MonoBehaviour {
    /// <summary>
    /// Currently open submenu of title screen
    /// </summary>
    GameObject submenu;

    /// <summary>
    /// T/F for color blind mode
    /// </summary>
    public static bool colorBlindMode;

    /// <summary>
    /// Starts the intro cutscene
    /// </summary>
    public void StartGame()
    {
        //Play button click sound
        UIController.Instance.playButtonSound();
        SceneManager.LoadScene("Intro_Cutscene");
    }

    /// <summary>
    /// Hides the main menu and opens a submenu
    /// </summary>
    /// <param name="_submenu"></param>
    public void OpenSubmenu(GameObject _submenu)
    {
        //Play button click sound
        UIController.Instance.playButtonSound();

        submenu = _submenu;
        gameObject.SetActive(false);
        submenu.SetActive(true);
    }

    /// <summary>
    /// Closes the current submenu and unhides the main menu
    /// </summary>
    public void ReturnMainMenu()
    {
        submenu.SetActive(false);
        submenu = null;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Swap color blind mode
    /// </summary>
    /// <returns></returns>
    public static void ChangeColorBlindMode() {
        colorBlindMode = !colorBlindMode;
        Debug.Log("Color Blind Mode: "+colorBlindMode);
    }
}
