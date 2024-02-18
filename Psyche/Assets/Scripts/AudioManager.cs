/** 
Description: audio manager script
Author: blopezro
Version: 20240206
**/

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// AudioManager class which handles all audio within game.
/// </summary>
public class AudioManager : MonoBehaviour {

    // ui
    [SerializeField] public AudioSource buttonClick;
    public List<AudioSource> uiAudioSources;

    // music
    [SerializeField] public AudioSource backgroundMusic;
    public List<AudioSource> musicAudioSources;

    // in game sfx
    [SerializeField] public AudioSource toolGRS;
    [SerializeField] public AudioSource toolImager;
    [SerializeField] public AudioSource toolEMagnet;
    [SerializeField] public AudioSource toolThrusters;
    [SerializeField] public AudioSource playerJump;
    [SerializeField] public AudioSource playerHurt;
    public List<AudioSource> sfxAudioSources;
    
    //////////////////////////////////////////////////
    //Private linker
    private PlayerController _playerController;
    public void Initialize(PlayerController playerController) {
        _playerController = playerController;
    }
    //////////////////////////////////////////////////
    
    private void OnEnable() {
        uiAudioSources.Add(buttonClick);
        musicAudioSources.Add(backgroundMusic);
        sfxAudioSources.Add(toolGRS);
        sfxAudioSources.Add(toolImager);
        sfxAudioSources.Add(toolEMagnet);
        sfxAudioSources.Add(toolThrusters);
        sfxAudioSources.Add(playerJump);
        sfxAudioSources.Add(playerHurt);
    }

    /// <summary>
    /// play and audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void PlayAudio(AudioSource audioSource) {
        audioSource.Play();
    }

    /// <summary>
    /// stop an audio source
    /// </summary>
    /// <param name="audioSource"></param>
    public void StopAudio(AudioSource audioSource) {
        audioSource.Stop();
    }

    /// <summary>
    /// toggle an audio source mute
    /// </summary>
    /// <param name="audioSource"></param>
    public void ToggleAudioMute(AudioSource audioSource) {
        audioSource.mute = !audioSource.mute;
    }

    /// <summary>
    /// toggle an audio source mute for all in a list
    /// </summary>
    /// <param name="audioSources"></param>
    public void ToggleAudioMuteForAll(List<AudioSource> audioSources) {
        foreach(AudioSource audioSource in audioSources) {
            audioSource.mute = !audioSource.mute;
        }
    }

    public void ToggleAudioMuteForAllUi() {    ToggleAudioMuteForAll(uiAudioSources); }
    public void ToggleAudioMuteForAllMusic() { ToggleAudioMuteForAll(musicAudioSources); }
    public void ToggleAudioMuteForAllSfx() {   ToggleAudioMuteForAll(sfxAudioSources); }

    /// <summary>
    /// set an audio source volume
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolume(AudioSource audioSource, float desiredVolume) {
        audioSource.volume = desiredVolume;
    }

    /// <summary>
    /// set an audio source volume for all in a list
    /// </summary>
    /// <param name="audioSources"></param>
    /// <param name="desiredVolume"></param>
    public void SetAudioVolumeForAll(List<AudioSource> audioSources, float desiredVolume) {
        foreach(AudioSource audioSource in audioSources) {
            audioSource.volume = desiredVolume;
        }
    }

    public void SetAudioVolumeForAllUi(float desiredVolume) {
        SetAudioVolumeForAll(uiAudioSources, desiredVolume);
    }
    
    public void SetAudioVolumeForAllMusic(float desiredVolume) {
        SetAudioVolumeForAll(musicAudioSources, desiredVolume);
    }
    
    public void SetAudioVolumeForAllSfx(float desiredVolume) {
        SetAudioVolumeForAll(sfxAudioSources, desiredVolume);
    }

}
