using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functions for controlling the title screen.
/// Functions are called from the OnClick() event of buttons.
/// </summary>
/// Author: jmolive8
public class TitleScreenManager : MonoBehaviour {
    public GameObject mainMenu;

    private GameObject curSubmenu;

    /// <summary>
    /// Starts the intro cutscene
    /// </summary>
    public void startGame()
    {
        //Play button click sound
        //UIController.Instance.playButtonSound(); //Prevents game from loading because UIController does not yet exist 

        SceneManager.LoadScene("Intro_Cutscene");
    }

    /// <summary>
    /// Hides the main menu and opens a submenu
    /// </summary>
    /// <param name="submenu"></param>
    public void openSubmenu(GameObject submenu)
    {
        //Play button click sound
        UIController.Instance.playButtonSound();

        curSubmenu = submenu;
        mainMenu.SetActive(false);
        curSubmenu.SetActive(true);
    }

    /// <summary>
    /// Closes the current submenu and unhides the main menu
    /// </summary>
    public void closeSubmenu()
    {
        curSubmenu.SetActive(false);
        curSubmenu = null;
        mainMenu.SetActive(true);
    }
}