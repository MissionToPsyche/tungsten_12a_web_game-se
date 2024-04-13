/**
Description: Custom options for managing color blind mode and audio
Author: jmolive8, blopezro
Version: 20240410
**/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    public GameObject CheckMark;
    private float MusicVolSliderPrevious;
    public Slider MusicVolSlider;
    public Slider SfxVolSlider;
    public Slider UiVolSlider;
    public Slider VideoSlider;

    /// <summary>
    /// Activates color blind mode and audio instances
    /// </summary>
    private void OnEnable()
    {
        CheckMark.SetActive(GameController.Instance.ColorBlindMode);
        MusicVolSlider.value = GameController.Instance.musicVol;
        SfxVolSlider.value = GameController.Instance.sfxVol;
        UiVolSlider.value = GameController.Instance.uiVol;
        GameStateManager.Scene GameStateManagerSn =
            GameController.Instance.GameStateManager.MatchScene(SceneManager.GetActiveScene().name);
        // set video volume when not in title screen
        if (GameStateManagerSn != GameStateManager.Scene.Title)
        {
            VideoSlider.value = GameController.Instance.videoVol;
        }
    }

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void ToggleCheckMark()
    {
        CheckMark.SetActive(!CheckMark.activeInHierarchy);
        GameController.Instance.ChangeColorBlindMode(CheckMark.activeInHierarchy);
    }

    /// <summary>
    /// Sets music volume
    /// </summary>
    public void SetMusicVolume()
    {
        GameController.Instance.musicVol = MusicVolSlider.value;
        GameController.Instance.AudioManager.SetAudioVolumeForAllMusic(MusicVolSlider.value);
    }

    /// <summary>
    /// Sets sfx volume
    /// </summary>
    public void SetSfxVolume()
    {
        GameController.Instance.sfxVol = SfxVolSlider.value;
        GameController.Instance.AudioManager.SetAudioVolumeForAllSfx(SfxVolSlider.value);
    }

    /// <summary>
    /// Sets ui volume
    /// </summary>
    public void SetUiVolume()
    {
        GameController.Instance.uiVol = UiVolSlider.value;
        GameController.Instance.AudioManager.SetAudioVolumeForAllUi(UiVolSlider.value);
    }

    /// <summary>
    /// Sets the music to mute
    /// </summary>
    public void SetMusicMute()
    {
        MusicVolSliderPrevious = MusicVolSlider.value;
        MusicVolSlider.value = 0;
    }

    /// <summary>
    /// Unmutes the music
    /// </summary>
    public void SetMusicUnmute()
    {
        MusicVolSlider.value = MusicVolSliderPrevious;
    }

}
