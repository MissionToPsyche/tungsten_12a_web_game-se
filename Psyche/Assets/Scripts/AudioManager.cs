/** 
Description: audio manager script
Author: blopezro
Version: 20231107
**/

using UnityEngine.Audio;
using UnityEngine;
using UnityEditor.UI;
using UnityEditor.Rendering;

/// <summary>
/// AudioManager class which handles all audio within game.
/// </summary>
public class AudioManager : MonoBehaviour {


    // scenes
    [SerializeField] public AudioSource backgroundMusic;
    [SerializeField] public AudioSource buttonClick;
    
    // tools
    [SerializeField] public AudioSource toolGRS;
    [SerializeField] public AudioSource toolImager;
    [SerializeField] public AudioSource toolEMagnet;
    [SerializeField] public AudioSource toolThrusters;
    
    // player
    [SerializeField] public AudioSource playerJump;

    //Private linker
    private PlayerController _playerController;

    // these would be called from player management, examples:
    // _playerManagement.audioManager.PlayAudio(_playerManagement.audioManager.toolImager);
    // _playerManagement.audioManager.StopAudio(_playerManagement.audioManager.toolImager);

    public void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void PlayAudio(AudioSource audioSource) {
        audioSource.Play();
    }

    public void StopAudio(AudioSource audioSource) {
        audioSource.Stop();
    }

    public void ToggleAudioMute(AudioSource audioSource) {
        audioSource.mute = !audioSource.mute;
    }
}
