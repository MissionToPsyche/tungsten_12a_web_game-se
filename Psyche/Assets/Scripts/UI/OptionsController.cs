/**
Description: Custom options for managing color blind mode and audio
Author: jmolive8, blopezro
Version: 20240331
**/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    public GameObject checkMark;
    private float musicVolSliderPrevious;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;
    public Slider uiVolSlider;
    public Slider videoSlider;

    /// <summary>
    /// Activates color blind mode and audio instances
    /// </summary>
    private void OnEnable()
    {
        checkMark.SetActive(GameController.Instance.ColorBlindMode);
        musicVolSlider.value = GameController.Instance.musicVol;
        sfxVolSlider.value = GameController.Instance.sfxVol;
        uiVolSlider.value = GameController.Instance.uiVol;
        GameStateManager.Scene GameStateManagerSn =
            GameController.Instance.GameStateManager.MatchScene(SceneManager.GetActiveScene().name);
        // set video volume when not in title screen
        if (GameStateManagerSn != GameStateManager.Scene.Title)
        {
            videoSlider.value = GameController.Instance.videoVol;
        }
    }

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void toggleCheckMark()
    {
        checkMark.SetActive(!checkMark.activeInHierarchy);
        GameController.Instance.ChangeColorBlindMode(checkMark.activeInHierarchy);
    }

    /// <summary>
    /// Sets music volume
    /// </summary>
    public void setMusicVolume()
    {
        GameController.Instance.musicVol = musicVolSlider.value;
        GameController.Instance.AudioManager.SetAudioVolumeForAllMusic(musicVolSlider.value);
    }

    /// <summary>
    /// Sets sfx volume
    /// </summary>
    public void setSfxVolume()
    {
        GameController.Instance.sfxVol = sfxVolSlider.value;
        GameController.Instance.AudioManager.SetAudioVolumeForAllSfx(sfxVolSlider.value);
    }

    /// <summary>
    /// Sets ui volume
    /// </summary>
    public void setUiVolume()
    {
        GameController.Instance.uiVol = uiVolSlider.value;
        GameController.Instance.AudioManager.SetAudioVolumeForAllUi(uiVolSlider.value);
    }

    /// <summary>
    /// Sets the music to mute
    /// </summary>
    public void setMusicMute()
    {
        musicVolSliderPrevious = musicVolSlider.value;
        musicVolSlider.value = 0; 
    }

    /// <summary>
    /// Unmutes the music
    /// </summary>
    public void setMusicUnmute()
    {
        musicVolSlider.value = musicVolSliderPrevious; 
    }

}
