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
    /// Opens the Scene of the given name
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        //Play button click sound
        //UIController.Instance.playButtonSound(); //Prevents game from loading because UIController does not yet exist 

        SceneManager.LoadScene(sceneName);
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
}
