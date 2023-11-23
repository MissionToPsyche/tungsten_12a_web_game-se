using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the transitions between scenes
/// </summary>
public class TransitionManager : MonoBehaviour {
    //Private variables
    private PlayerController _playerManagement;
    private Vector3 _landingPosition;
    private GameObject _transitionObject;
    
    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(PlayerController playerManagement) {
        _playerManagement = playerManagement;
    }

    /// <summary>
    /// Transitions the player to the respective scene if the necessary parameters are met
    /// </summary>
    /// <param name="travelToSceneName"></param>
    /// <returns></returns>
    public IEnumerator CheckTransition(string travelToSceneName) {
        //Polling rate
        yield return new WaitForSeconds(0.1f);

        //Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        //If a positive vertical button is pressed (w or up), then transition
        if (Input.GetButton("Vertical") && verticalAxis > 0) {
            switch(travelToSceneName) {
                case "Tool_Intro_Thruster":
                case "Tool_Intro_GRS":
                case "Tool_Intro_Imager":
                case "Tool_Intro_eMagnet":
                    RepositionPlayer(travelToSceneName, verticalAxis);
                    break;

                case "SceneTransition_Back": // return back to previous scene
                    // in work     

                case "SceneTransition_Game_End":
                    UIController.Instance.EndGame(true);
                    break;

                default:
                    Debug.Log("Invalid Scene Transition");
                    break;
            }
        }
    }

    /// <summary>
    /// Repositions player on transition to new scene based on game object with TransitionObjectIn tag
    /// </summary>
    public void RepositionPlayer(string travelToSceneName, float verticalAxis) {
    if (verticalAxis > 0) {
        Debug.Log("travelToSceneName: "+travelToSceneName);
        SceneManager.LoadScene(travelToSceneName, LoadSceneMode.Single);
        Scene travelToScene = SceneManager.GetSceneByName(travelToSceneName);

        if (travelToScene.IsValid()) {
            if (!travelToScene.isLoaded) {
                SceneManager.LoadScene(travelToSceneName, LoadSceneMode.Single);
                Debug.Log(travelToSceneName+" loaded");
            }

            Debug.Log("Finding TransitionObjectIn");
            GameObject[] travelToSceneGameObjects = travelToScene.GetRootGameObjects();
            foreach (GameObject obj in travelToSceneGameObjects) {
                if (obj.CompareTag("TransitionObjectIn")) {
                    _transitionObject = obj;
                    _landingPosition = _transitionObject.transform.position;
                    _playerManagement.playerCharacter.transform.position = _landingPosition;
                    return; // Exit method after repositioning
                }
            }
            Debug.LogError("TransitionObjectIn not found in the scene");
        } else {
            Debug.LogError("travelToScene ("+travelToScene.name+") is not valid");
        }
    }
}

     // Callback for scene loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        RepositionPlayer(scene.name, 1.0f); // Assuming the transition is triggered with a positive vertical axis
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove the callback once repositioning is done
    }

    public void LoadSceneWithCallback(string sceneName) {
        SceneManager.sceneLoaded += OnSceneLoaded; // Add the callback
        SceneManager.LoadScene(sceneName);
    }

}
