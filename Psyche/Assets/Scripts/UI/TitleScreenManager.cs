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
    public void StartGame()
    {
        //Play button click sound
        GameController.Instance.AudioManager.ButtonClick.Play();

        // Load the scene
        // Set the value this way so if any changes are made they are accounted for
        string scene = GameController.Instance.GameStateManager.MatchScene(GameStateManager.Scene.Intro);
        GameController.Instance.SceneTransitionManager.DevControl = true;
        GameController.Instance.SceneTransitionManager.OnInitiateTransition(scene);
    }

    /// <summary>
    /// Hides the main menu and opens a submenu
    /// </summary>
    /// <param name="submenu"></param>
    public void OpenSubmenu(GameObject submenu)
    {
        //Play button click sound
        GameController.Instance.AudioManager.ButtonClick.Play();

        CurSubmenu = submenu;
        MainMenu.SetActive(false);
        CurSubmenu.SetActive(true);
    }

    /// <summary>
    /// Closes the current submenu and unhides the main menu
    /// </summary>
    public void CloseSubmenu()
    {
        CurSubmenu.SetActive(false);
        CurSubmenu = null;
        MainMenu.SetActive(true);
    }

    public void OpenLink(string source)
    {
        switch (source)
        {
            case "Bensound":
                Application.OpenURL("https://www.bensound.com/free-music-for-videos");
                //License code: UD7WUXTB2NG5WX73
                break;
            case "Pixabay":
                Application.OpenURL("https://pixabay.com/sound-effects/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=80241");
                break;
            case "PixelAdventure":
                Application.OpenURL("https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360");
                break;
            case "PixelSpace":
                Application.OpenURL("https://assetstore.unity.com/packages/2d/characters/free-pixel-space-platform-pack-146318");
                break;
            default:
                break;
        }
    }
}
