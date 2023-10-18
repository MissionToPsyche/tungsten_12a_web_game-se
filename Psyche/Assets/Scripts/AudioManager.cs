/** 
Description: audio manager script
Author: blopezro
Version: 20231017
**/

using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// AudioManager class which handles all audio within game.
/// </summary>
public class AudioManager : MonoBehaviour {

    // scene
    [SerializeField] private AudioSource backgroundMusic;
    
    // tools
    [SerializeField] private AudioSource toolGRS;
    [SerializeField] private AudioSource toolImager;
    [SerializeField] private AudioSource toolMagnetometer;
    [SerializeField] private AudioSource toolThrusters;
    
    // player
    [SerializeField] private AudioSource playerJump;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            PlayToolGRS();
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            PlayToolImager();
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            PlayToolMagnetometer();
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            PlayToolThrusters();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayPlayerJump();
        }
    }

    void PlayToolGRS() {
        toolGRS.Play();
    }

    void StopToolGRS() {
        toolGRS.Stop();
    }

    void PlayToolImager() {
        toolImager.Play();
    }

    void StopToolImager() {
        toolImager.Stop();
    }

    void PlayToolMagnetometer() {
        toolMagnetometer.Play();
    }

    void StopToolMagnetometer() {
        toolMagnetometer.Stop();
    }

    void PlayToolThrusters() {
        toolThrusters.Play();
    }

    void StopToolThrusters() {
        toolThrusters.Stop();
    }

    void PlayPlayerJump() {
        playerJump.Play();
    }

    void StopPlayerJump() {
        playerJump.Stop();
    }

}
