using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the transitions between scenes
/// </summary>
public class TransitionManager : MonoBehaviour
{
    //Private variables
    private PlayerManagement _playerManagement;
    
    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        _playerManagement = playerManagement;
    }

    /// <summary>
    /// Transitions the player to the respective scene if the necessary parameters are met
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public IEnumerator CheckTransition(string sceneName)
    {
        //Polling rate
        yield return new WaitForSeconds(0.1f);

        //Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        //If a positive vertical button is pressed (w or up), then transition
        if (Input.GetButton("Vertical") && verticalAxis > 0)
        {
            switch(sceneName)
            {
                case "SceneTransition_Home":
                    SceneManager.LoadScene("SampleScene"); 
                    break;
                case "SceneTransition_Joshua":
                    SceneManager.LoadScene("DeveloperScene_Joshua");
                    break;
                case "SceneTransition_Dhalia":
                    SceneManager.LoadScene("DeveloperScene_Dhalia"); break;
                case "SceneTransition_Bryant":
                    SceneManager.LoadScene("DeveloperScene_Bryant"); break;
                case "SceneTransition_Matt":
                    SceneManager.LoadScene("DeveloperScene_Matt"); break;
                case "SceneTransition_James":
                    SceneManager.LoadScene("DeveloperScene_James"); break;
                default:
                    Debug.Log("Invalid Scene Transition"); break;
            }   
        }
    }
}
