/*
 * Description: Scene Transitions
 * Authors: joshbenn, blopezro, mcmyers4
 * Version: 20240119
 */

using Cinemachine;
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
    private String _directionTag;
    
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
    /// <param name="tag"></param>
    /// <returns></returns>
    public IEnumerator CheckTransition(string travelToSceneName, string directionTag) {
        _directionTag = directionTag;

        //Polling rate
        yield return new WaitForSeconds(0.1f);

        //Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        //If a positive vertical button is pressed (w or up), then transition
        if (Input.GetButton("Vertical") && verticalAxis > 0) {
            switch (travelToSceneName)
            {
                case "Landing_Scene":
                    travelToScene(travelToSceneName);
                    yield return new WaitForSeconds(0.1f);
                    loadBackground();
                    loadCameraBounds();
                    break;
                case "Tool_Intro_Thruster":
                    travelToScene(travelToSceneName);
                    yield return new WaitForSeconds(0.1f);
                    loadBackground();
                    loadCameraBounds();
                    break;
                case "Tool_Intro_GRS":
                    travelToScene(travelToSceneName);
                    yield return new WaitForSeconds(0.1f);
                    loadBackground();
                    loadCameraBounds();
                    break;
                case "Tool_Intro_Imager":
                    travelToScene(travelToSceneName);
                    yield return new WaitForSeconds(0.1f);
                    loadBackground();
                    loadCameraBounds();
                    break;
                case "Tool_Intro_eMagnet":
                    travelToScene(travelToSceneName);
                    yield return new WaitForSeconds(0.1f);
                    loadBackground();
                    loadCameraBounds();
                    break;

                case "SceneTransition_Game_End":
                    UIController.Instance.EndGame(true);
                    break;

                default:
                    Debug.LogError("Invalid Scene Transition");
                    break;
            }
        }
    }

    /// <summary>
    /// Loads scene to be transitioned to.
    /// </summary>
    /// <param name="travelToSceneName">
    /// Name of the scene that the character to traveling to.
    /// </param>
    private void travelToScene(string travelToSceneName)
    {
        _travelToSceneName = travelToSceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene(travelToSceneName);
        transition = true;
    }

    /// <summary>
    /// Finds and passes the unique boundary of the newly loaded scene to each of the game's cameras.
    /// </summary>
    /// Needs a WaitForSeconds() before being called so the scene can fully load, or else
    /// will try to find the camera bounds of the previous scene.
    private void loadCameraBounds()
    {
        //array of the virtual cameras attached to the player
        GameObject[] vcs = GameObject.FindGameObjectsWithTag("VirtualCamera");
        GameObject bounds;
        CompositeCollider2D shape;
        //loops through and assigns the boundary to each of the virtual cameras
        foreach (GameObject vc in vcs)
        {
            vc.GetComponent<CinemachineConfiner2D>().InvalidateCache();
            bounds = GameObject.FindGameObjectWithTag("Boundary");
            shape = bounds.GetComponent<CompositeCollider2D>();
            vc.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = shape;
        }
    }

    /// <summary>
    /// Finds the main camera after loading into a scene and connects the
    /// background to the camera. Thus, the background always follows the camera.
    /// </summary>
    private void loadBackground()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Background");
        Canvas canvas = background.GetComponent<Canvas>();

        canvas.planeDistance = 100;
        canvas.worldCamera = Camera.main;
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
        if (transition) {
            RepositionPlayer(_travelToSceneName);
        }
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
            String tag = "TransitionObjectIn";
            if (_directionTag == tag) {
                tag = "TransitionObjectOut";
            }
            GameObject obj = GameObject.FindGameObjectWithTag(tag);
                if (obj != null) {
                    return obj.transform.position;
                } else {
                    Debug.LogWarning(tag+" not found.");
                }
        } else {
            Debug.LogWarning("Scene "+sceneName+" is not loaded.");
        }
        return Vector3.zero;
    }

}
