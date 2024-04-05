using System;
using UnityEngine;

/// <summary>
/// Functions for controlling the title screen.
/// Functions are called from the OnClick() event of buttons.
/// </summary>
/// Author: jmolive8, dnguye99
public class TitleScreenManager : MonoBehaviour {
    public GameObject MainMenu;

    private GameObject CurSubmenu;

    /// <summary>
    /// Starts the intro cutscene
    /// </summary>
    public void startGame()
    {
        //Play button click sound
        GameController.Instance.AudioManager.buttonClick.Play();

        // Load the scene
        // Set the value this way so if any changes are made they are accounted for
        string scene = GameController.Instance.GameStateManager.MatchScene(GameStateManager.Scene.Intro);
        GameController.Instance.SceneTransitionManager.devControl = true;
        GameController.Instance.SceneTransitionManager.OnInitiateTransition(scene);
    }

    /// <summary>
    /// Hides the main menu and opens a submenu
    /// </summary>
    /// <param name="submenu"></param>
    public void openSubmenu(GameObject submenu)
    {
        //Play button click sound
        GameController.Instance.AudioManager.buttonClick.Play();

        CurSubmenu = submenu;
        MainMenu.SetActive(false);
        CurSubmenu.SetActive(true);
    }

    /// <summary>
    /// Closes the current submenu and unhides the main menu
    /// </summary>
    public void closeSubmenu()
    {
        CurSubmenu.SetActive(false);
        CurSubmenu = null;
        MainMenu.SetActive(true);
    }
}
