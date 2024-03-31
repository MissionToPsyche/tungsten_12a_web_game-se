/**
Description: Custom options for managing color blind mode and audio
Author: jmolive8, blopezro
Version: 20240331
**/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {
    public GameObject checkMark;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;
    public Slider uiVolSlider;
    public Slider videoSlider;

    /// <summary>
    /// Activates color blind mode and audio instances
    /// </summary>
    private void OnEnable() {
        checkMark.SetActive(GameController.Instance.colorBlindMode);
        musicVolSlider.value = GameController.Instance.musicVol;
        sfxVolSlider.value = GameController.Instance.sfxVol;
        uiVolSlider.value = GameController.Instance.uiVol;
        GameStateManager.Scene GameStateManagerSn = 
            GameController.Instance.gameStateManager.MatchScene(SceneManager.GetActiveScene().name);
        // set video volume when not in title screen
        if (GameStateManagerSn != GameStateManager.Scene.Title) {
            videoSlider.value = GameController.Instance.videoVol;
        }
    }

    /// <summary>
    /// Toggles check mark
    /// </summary>
    public void toggleCheckMark() {
        checkMark.SetActive(!checkMark.activeInHierarchy);
        GameController.Instance.ChangeColorBlindMode(checkMark.activeInHierarchy);
    }

    /// <summary>
    /// Sets music volume
    /// </summary>
    public void setMusicVolume() {
        GameController.Instance.musicVol = musicVolSlider.value;
        GameController.Instance.audioManager.SetAudioVolumeForAllMusic(musicVolSlider.value);
    }
    
    /// <summary>
    /// Sets sfx volume
    /// </summary>
    public void setSfxVolume() {
        GameController.Instance.sfxVol = sfxVolSlider.value;
        GameController.Instance.audioManager.SetAudioVolumeForAllSfx(sfxVolSlider.value);
    }

    /// <summary>
    /// Sets ui volume
    /// </summary>
    public void setUiVolume() {
        GameController.Instance.uiVol = uiVolSlider.value;
        GameController.Instance.audioManager.SetAudioVolumeForAllUi(uiVolSlider.value);
    }

    /// <summary>
    /// Sets video volume
    /// </summary>
    public void setVideoVolume() {
        GameController.Instance.videoVol = videoSlider.value;
        GameController.Instance.audioManager.SetAudioVolumeForAllVideos(videoSlider.value);
    }
}
