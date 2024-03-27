/**
Description: Audio control script
Author: blopezro
Version: 20240326
**/

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AudioControl Class
/// </summary>
public class AudioControl : MonoBehaviour {
    public GameObject checkMark;
    public Slider slider;

    /// <summary>
    /// Activates check mark
    /// </summary>
    private void OnEnable() {
        checkMark.SetActive(GameController.Instance.colorBlindMode);
    }

    /// <summary>
    /// Sets the volume for all ui
    /// </summary>
    public void setUiVolume() {
        GameController.Instance.audioManager.SetAudioVolumeForAllUi(slider.value);
    }

    /// <summary>
    /// Sets the volume for all music
    /// </summary>
    public void setMusicVolume() {
        GameController.Instance.audioManager.SetAudioVolumeForAllMusic(slider.value);
    }

    /// <summary>
    /// Sets the volume for all sfx
    /// </summary>
    public void setSfxVolume() {
        GameController.Instance.audioManager.SetAudioVolumeForAllSfx(slider.value);
    }

}