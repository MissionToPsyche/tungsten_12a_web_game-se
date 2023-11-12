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
    private Vector2 _landingPosition;
    private AsyncOperation _loadScene;
    
    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerManagement playerManagement)
    {
        _playerManagement = playerManagement;
        _landingPosition = new Vector2(-2, -4);
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
                case "SceneTransition_Home":                                   //Home
                    {
                        //Turn the player off - Unused until script detached from player
                        //_playerManagement.playerCharacter.gameObject.SetActive(false);
                        //Make the scene load before doing anything else
                        _loadScene = SceneManager.LoadSceneAsync("SampleScene");
                        yield return new WaitUntil( () => _loadScene.isDone);
                        //Place the playercharacter at the new position
                        _playerManagement.playerCharacter.transform.position = _landingPosition;
                        //Turn the player back on
                        //_playerManagement.playerCharacter.gameObject.SetActive(true);
                        break; 
                    }
                case "SceneTransition_Joshua":
                    SceneManager.LoadScene("DeveloperScene_Joshua");    break; //Josh
                case "SceneTransition_Dhalia":
                    SceneManager.LoadScene("DeveloperScene_Dhalia");    break; //Dhalia
                case "SceneTransition_Bryant":
                    SceneManager.LoadScene("DeveloperScene_Bryant");    break; //Bryant
                case "SceneTransition_Matt":
                    SceneManager.LoadScene("DeveloperScene_Matt");      break; //Matt
                case "SceneTransition_James":
                    SceneManager.LoadScene("DeveloperScene_James");     break; //James
                case "SceneTransition_End":
                    UIController.Instance.EndGame(true);                break;
                default:
                    Debug.Log("Invalid Scene Transition");              break; //Default
            }   
        }
    }
}
