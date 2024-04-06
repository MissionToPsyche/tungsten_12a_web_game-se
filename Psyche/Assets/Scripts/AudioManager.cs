/** 
Description: Audio manager script
Author: blopezro
Version: 20240326
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// AudioManager class which handles all audio within game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Slider slider;

    [Header("User Interface")]
    [SerializeField] public AudioSource buttonClick;
    public List<AudioSource> uiAudioSources;

    [Header("Music")]
    [SerializeField] public AudioSource backgroundMusic;
    public List<AudioSource> musicAudioSources;

    [Header("In Game Sound Effects")]
    [SerializeField] public AudioSource toolGRNS;
    [SerializeField] public AudioSource toolImager;
    [SerializeField] public AudioSource toolEMagnet;
    [SerializeField] public AudioSource toolThrusters;
    [SerializeField] public AudioSource playerJump;
    [SerializeField] public AudioSource playerHurt;
    [SerializeField] public AudioSource pickupElement;
    [SerializeField] public AudioSource pickupElementTungsten;
    [SerializeField] public AudioSource pickupTool;
    [SerializeField] public AudioSource checkpoint;
    public List<AudioSource> sfxAudioSources;

    /// <summary>
    /// collects the same type audio sources into their respected lists
    /// </summary>
    private void OnEnable()
    {
        uiAudioSources.Add(buttonClick);
        musicAudioSources.Add(backgroundMusic);
        sfxAudioSources.Add(toolGRNS);
        sfxAudioSources.Add(toolImager);
        sfxAudioSources.Add(toolEMagnet);
        sfxAudioSources.Add(toolThrusters);
        sfxAudioSources.Add(playerJump);
        sfxAudioSources.Add(playerHurt);
        sfxAudioSources.Add(pickupElement);
        sfxAudioSources.Add(pickupElementTungsten);
        sfxAudioSources.Add(pickupTool);
        sfxAudioSources.Add(checkpoint);
        foreach (AudioSource audioSource in uiAudioSources)
            audioSource.volume = GameController.Instance.uiVol;
        foreach (AudioSource audioSource in musicAudioSources)
            audioSource.volume = GameController.Instance.musicVol;
        foreach (AudioSource audioSource in sfxAudioSources)
            audioSource.volume = GameController.Instance.sfxVol;
    }

    /// <summary>
    /// play an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void PlayAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }

    /// <summary>
    /// pause an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void PauseAudio(AudioSource audioSource)
    {
        audioSource.Pause();
    }

    /// <summary>
    /// unpause an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void UnpauseAudio(AudioSource audioSource)
    {
        audioSource.UnPause();
    }

    /// <summary>
    /// mute an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void MuteAudio(AudioSource audioSource)
    {
        audioSource.mute = true;
    }

    /// <summary>
    /// unmute an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void UnmuteAudio(AudioSource audioSource)
    {
        audioSource.mute = false;
    }

    /// <summary>
    /// stop an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void StopAudio(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    /// <summary>
    /// toggle an audio source mute
    /// </summary>
    /// <param name="audioSource"></param>
    public void ToggleAudioMute(AudioSource audioSource)
    {
        audioSource.mute = !audioSource.mute;
    }

    /// <summary>
    /// toggle an audio source mute for all in a list
    /// </summary>
    /// <param name="audioSources"></param>
    public void ToggleAudioMuteForAll(List<AudioSource> audioSources)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = !audioSource.mute;
        }
    }

    /// <summary>
    /// Toggles mute for all UI audio sources
    /// </summary>
    public void ToggleAudioMuteForAllUi()
    {
        ToggleAudioMuteForAll(uiAudioSources);
    }

    /// <summary>
    /// Toggles mute for all music audio sources
    /// </summary>
    public void ToggleAudioMuteForAllMusic()
    {
        ToggleAudioMuteForAll(musicAudioSources);
    }

    /// <summary>
    /// Toggles mute for all sfx audio sources
    /// </summary>
    public void ToggleAudioMuteForAllSfx()
    {
        ToggleAudioMuteForAll(sfxAudioSources);
    }

    /// <summary>
    /// set an audio source volume
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolume(AudioSource audioSource, float desiredVolume)
    {
        audioSource.volume = desiredVolume;
    }

    /// <summary>
    /// set an audio source volume for all in a list
    /// </summary>
    /// <param name="audioSources"></param>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolumeForAll(List<AudioSource> audioSources, float desiredVolume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = desiredVolume;
        }
    }

    /// <summary>
    /// set an audio source volume for all ui
    /// </summary>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolumeForAllUi(float desiredVolume)
    {
        SetAudioVolumeForAll(uiAudioSources, desiredVolume);
    }

    /// <summary>
    /// set an audio source volume for all music
    /// </summary>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolumeForAllMusic(float desiredVolume)
    {
        SetAudioVolumeForAll(musicAudioSources, desiredVolume);
    }

    /// <summary>
    /// set an audio source volume for all sfx
    /// </summary>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolumeForAllSfx(float desiredVolume)
    {
        SetAudioVolumeForAll(sfxAudioSources, desiredVolume);
    }

}
