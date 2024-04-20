/** 
Description: Audio manager script
Author: blopezro
Version: 20240410
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// AudioManager class which handles all audio within game.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public Slider Slider;

    [Header("User Interface")]
    [SerializeField] public AudioSource ButtonClick;
    public List<AudioSource> UiAudioSources;

    [Header("Music")]
    [SerializeField] public AudioSource BackgroundMusic;
    public List<AudioSource> MusicAudioSources;

    [Header("In Game Sound Effects")]
    [SerializeField] public AudioSource ToolGRNS;
    [SerializeField] public AudioSource ToolImager;
    [SerializeField] public AudioSource ToolEMagnet;
    [SerializeField] public AudioSource ToolThrusters;
    [SerializeField] public AudioSource PlayerJump;
    [SerializeField] public AudioSource PlayerHurt;
    [SerializeField] public AudioSource PickupElement;
    [SerializeField] public AudioSource PickupElementTungsten;
    [SerializeField] public AudioSource PickupTool;
    [SerializeField] public AudioSource Checkpoint;
    public List<AudioSource> SfxAudioSources;

    /// <summary>
    /// collects the same type audio sources into their respected lists
    /// </summary>
    private void OnEnable()
    {
        UiAudioSources.Add(ButtonClick);
        MusicAudioSources.Add(BackgroundMusic);
        SfxAudioSources.Add(ToolGRNS);
        SfxAudioSources.Add(ToolImager);
        SfxAudioSources.Add(ToolEMagnet);
        SfxAudioSources.Add(ToolThrusters);
        SfxAudioSources.Add(PlayerJump);
        SfxAudioSources.Add(PlayerHurt);
        SfxAudioSources.Add(PickupElement);
        SfxAudioSources.Add(PickupElementTungsten);
        SfxAudioSources.Add(PickupTool);
        SfxAudioSources.Add(Checkpoint);
        foreach (AudioSource AudioSource in UiAudioSources)
            AudioSource.volume = GameController.Instance.uiVol;
        foreach (AudioSource AudioSource in MusicAudioSources)
            AudioSource.volume = GameController.Instance.musicVol;
        foreach (AudioSource AudioSource in SfxAudioSources)
            AudioSource.volume = GameController.Instance.sfxVol;
    }

    /// <summary>
    /// play an audio source
    /// </summary>
    /// <param name="AudioSource"></param>
    public void PlayAudio(AudioSource AudioSource)
    {
        AudioSource.Play();
    }

    /// <summary>
    /// pause an audio source
    /// </summary>
    /// <param name="AudioSource"></param>
    public void PauseAudio(AudioSource AudioSource)
    {
        AudioSource.Pause();
    }

    /// <summary>
    /// unpause an audio source
    /// </summary>
    /// <param name="AudioSource"></param>
    public void UnpauseAudio(AudioSource AudioSource)
    {
        AudioSource.UnPause();
    }

    /// <summary>
    /// mute an audio source
    /// </summary>
    /// <param name="AudioSource"></param>
    public void MuteAudio(AudioSource AudioSource)
    {
        AudioSource.mute = true;
    }

    /// <summary>
    /// unmute an audio source
    /// </summary>
    /// <param name="AudioSource"></param>
    public void UnmuteAudio(AudioSource AudioSource)
    {
        AudioSource.mute = false;
    }

    /// <summary>
    /// stop an audio source
    /// </summary>
    /// <param name="AudioSource"></param>
    public void StopAudio(AudioSource AudioSource)
    {
        AudioSource.Stop();
    }

    /// <summary>
    /// toggle an audio source mute
    /// </summary>
    /// <param name="AudioSource"></param>
    public void ToggleAudioMute(AudioSource AudioSource)
    {
        AudioSource.mute = !AudioSource.mute;
    }

    /// <summary>
    /// toggle an audio source mute for all in a list
    /// </summary>
    /// <param name="AudioSources"></param>
    public void ToggleAudioMuteForAll(List<AudioSource> AudioSources)
    {
        foreach (AudioSource AudioSource in AudioSources)
        {
            AudioSource.mute = !AudioSource.mute;
        }
    }

    /// <summary>
    /// Toggles mute for all UI audio sources
    /// </summary>
    public void ToggleAudioMuteForAllUi()
    {
        ToggleAudioMuteForAll(UiAudioSources);
    }

    /// <summary>
    /// Toggles mute for all music audio sources
    /// </summary>
    public void ToggleAudioMuteForAllMusic()
    {
        ToggleAudioMuteForAll(MusicAudioSources);
    }

    /// <summary>
    /// Toggles mute for all sfx audio sources
    /// </summary>
    public void ToggleAudioMuteForAllSfx()
    {
        ToggleAudioMuteForAll(SfxAudioSources);
    }

    /// <summary>
    /// set an audio source volume
    /// </summary>
    /// <param name="AudioSource"></param>
    /// <param name="DesiredVolume"></param>
    public void SetAudioVolume(AudioSource AudioSource, float DesiredVolume)
    {
        AudioSource.volume = DesiredVolume;
    }

    /// <summary>
    /// set an audio source volume for all in a list
    /// </summary>
    /// <param name="AudioSources"></param>
    /// <param name="DesiredVolume"></param>
    public void SetAudioVolumeForAll(List<AudioSource> AudioSources, float DesiredVolume)
    {
        foreach (AudioSource AudioSource in AudioSources)
        {
            AudioSource.volume = DesiredVolume;
        }
    }

    /// <summary>
    /// set an audio source volume for all ui
    /// </summary>
    /// <param name="DesiredVolume"></param>
    public void SetAudioVolumeForAllUi(float DesiredVolume)
    {
        SetAudioVolumeForAll(UiAudioSources, DesiredVolume);
    }

    /// <summary>
    /// set an audio source volume for all music
    /// </summary>
    /// <param name="DesiredVolume"></param>
    public void SetAudioVolumeForAllMusic(float DesiredVolume)
    {
        SetAudioVolumeForAll(MusicAudioSources, DesiredVolume);
    }

    /// <summary>
    /// set an audio source volume for all sfx
    /// </summary>
    /// <param name="DesiredVolume"></param>
    public void SetAudioVolumeForAllSfx(float DesiredVolume)
    {
        SetAudioVolumeForAll(SfxAudioSources, DesiredVolume);
    }

}
