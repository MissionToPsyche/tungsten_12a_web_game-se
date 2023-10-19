/** 
Description: audio manager script
Author: blopezro
Version: 20231018
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

    // these keys are temporary until all tools are in the same branch
    // and can call AudioManger as we currently have now with Imager and GRS.
    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) { PlayToolThrusters(); }
        if (Input.GetKeyUp(KeyCode.T)) { StopToolThrusters(); }
        if (Input.GetKeyDown(KeyCode.Space)) { PlayPlayerJump(); }
    }

    public void PlayToolGRS() {
        toolGRS.Play();
    }

    public void StopToolGRS() {
        toolGRS.Stop();
    }

    public void PlayToolImager() {
        toolImager.Play();
    }

    public void StopToolImager() {
        toolImager.Stop();
    }

    public void PlayToolMagnetometer() {
        toolMagnetometer.Play();
    }

    public void StopToolMagnetometer() {
        toolMagnetometer.Stop();
    }

    public void PlayToolThrusters() {
        toolThrusters.Play();
    }

    public void StopToolThrusters() {
        toolThrusters.Stop();
    }

    public void PlayPlayerJump() {
        playerJump.Play();
    }

    public void StopPlayerJump() {
        playerJump.Stop();
    }

}
