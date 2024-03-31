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
public class AudioControl : MonoBehaviour
{
    public GameObject checkMark;
    public Slider slider;

    private void OnEnable()
    {
        checkMark.SetActive(GameController.Instance.ColorBlindMode);
    }

    public void setUiVolume()
    {
        GameController.Instance.AudioManager.SetAudioVolumeForAllUi(slider.value);
        // todo: add example play of sound
    }

    public void setMusicVolume()
    {
        GameController.Instance.AudioManager.SetAudioVolumeForAllMusic(slider.value);
    }

    public void setSfxVolume()
    {
        GameController.Instance.AudioManager.SetAudioVolumeForAllSfx(slider.value);
        // todo: add example play of sound
    }

}