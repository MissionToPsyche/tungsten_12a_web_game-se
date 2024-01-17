using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the transitions between scenes
/// </summary>
public class SceneManager : MonoBehaviour {
    //Private variables
    private GameController _gameController;
    private Vector3 _landingPosition;
    private bool transition = false;
    private String _travelToSceneName;
    private String _currScene;
    
    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(GameController gameController) {
        _gameController = gameController;
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
            StartCoroutine(EnterCave(travelToSceneName));
        }
    }

    /// <summary>
    /// Executes the animation for the player entering the cave, then transitions the scene,
    /// then does the animation for the player exiting the cave.
    /// </summary>
    /// <param name="travelToSceneName"></param>
    /// <returns></returns>
    public IEnumerator EnterCave(string travelToSceneName)
    {
        //block player controls
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.enteringCave = true;

        //wait for the animation to be completed
        yield return new WaitForSeconds(0.5f);

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
        PlayerController.Instance.enteringCave = false;

        switch (travelToSceneName)
        {
            case "Landing_Scene":
            case "Tool_Intro_Thruster":
            case "Tool_Intro_GRS":
            case "Tool_Intro_Imager":
            case "Tool_Intro_eMagnet":
                _travelToSceneName = travelToSceneName;
                UnityEngine.SceneManagement.SceneManager.LoadScene(travelToSceneName);
                transition = true;
                break;

            case "SceneTransition_Game_End":
                UIController.Instance.EndGame(true);
                break;

            default:
                Debug.LogError("Invalid Scene Transition");
                break;
        }

        //block player controls
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.exitingCave = true;

        //wait for the animation to be completed
        yield return new WaitForSeconds(0.5f);

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
        PlayerController.Instance.exitingCave = false;
    }

    /// <summary>
    /// Subscribes to the SceneManager.sceneLoaded event
    /// </summary>
    void Awake() {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from event to prevent memory loss
    /// </summary>
    void OnDestroy() {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Runs each time a new scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        _currScene = scene.name;
        if (transition) {
            RepositionPlayer(_travelToSceneName);
        }
        //Debug.Log("_currScene: "+_currScene);
        //Debug.Log("_prevScene: "+_prevScene);
        transition = false;
    }

    /// <summary>
    /// Repositions player based on landing position 
    /// </summary>
    /// <param name="travelToSceneName"></param>
    public void RepositionPlayer(string travelToSceneName) {
        _landingPosition = GetTransitionObjectPosition(travelToSceneName);
        PlayerController.Instance.playerCharacter.transform.position = _landingPosition;
    }

    /// <summary>
    /// Gets the position of the tagged game object
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public Vector3 GetTransitionObjectPosition(string sceneName) {
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded) {
            GameObject obj = GameObject.FindGameObjectWithTag("TransitionObjectIn");
                if (obj != null) {
                    return obj.transform.position;
                } else {
                    Debug.LogWarning("TransitionObjectIn not found.");
                }
        } else {
            Debug.LogWarning("Scene "+sceneName+" is not loaded.");
        }
        return Vector3.zero;
    }

}
