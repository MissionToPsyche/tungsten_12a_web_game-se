using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Audio control for game
/// </summary>
/// Author: blopezro
public class AudioControl : MonoBehaviour {
    public GameObject checkMark;
    public Slider slider;

    private void OnEnable() {
        checkMark.SetActive(GameController.Instance.colorBlindMode);
    }

    public void setUiVolume() {
        GameController.Instance.audioManager.SetAudioVolumeForAllUi(slider.value);
    }

    public void setMusicVolume() {
        GameController.Instance.audioManager.SetAudioVolumeForAllMusic(slider.value);
    }

    public void setSfxVolume() {
        GameController.Instance.audioManager.SetAudioVolumeForAllSfx(slider.value);
    }
}