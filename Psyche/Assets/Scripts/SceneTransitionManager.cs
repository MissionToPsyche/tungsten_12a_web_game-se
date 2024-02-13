/*
 * Description: Scene Transitions
 * Authors: joshbenn, blopezro, mcmyers4
 * Version: 20240119
 */

using Cinemachine;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the transitions between scenes
/// </summary>
public class SceneTransitionManager : MonoBehaviour {
    private SceneTransitionManager _instance;
    //Private variables
    private GameController  _gameController;
    private Vector3         _landingPosition;
    private bool            _transition = false;
    private string          _travelToSceneName;
    private string          _directionTag;
    private bool            _inputBlocked = false;
    public bool             devControl = false;

    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(GameController gameController) {
        _gameController = gameController;
    }

    // #####################################################  Events #####################################################

    public void LoadPlayer()
    {
        PlayerController.Instance.playerCollisionManager.InitiateTransition += OnInitiateTransition;
    }

    public void LoadDevConsole()
    {
        _gameController.developerConsole.OnDevConsoleTransition += OnInitiateTransition;
    }

    /// <summary>
    /// Unsubscribes from event to prevent memory loss
    /// </summary>
    void OnDestroy()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.playerCollisionManager.InitiateTransition -= OnInitiateTransition;
        }
        //if (_gameController.developerConsole != null)
        //{
        //    _gameController.developerConsole.OnDevConsoleTransition -= OnInitiateTransition;
        //}
    }

    // ################################################  Scene Management ################################################

    /// <summary>
    /// Wrapper which handles event calls to initiate transition loading
    /// </summary>
    /// <param name="arg"></param>
    public void OnInitiateTransition(string arg)
    {
        StartCoroutine(CheckTransition(arg));
    }

    /// <summary>
    /// Transitions the player to the respective scene if the necessary parameters are met
    /// </summary>
    /// <param name="objectLabel"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public IEnumerator CheckTransition(string objectLabel) {
        // objectLabel = "Scene_Name Direction"; --> sceneInfo = ["Scene_Name", "Direction"];
        string[] sceneInfo = objectLabel.Split(' ');

        // Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        // Cross reference scene -- None == Non-Scene Input
        GameStateManager.Scene scene = _gameController.gameStateManager.MatchScene(sceneInfo[0]);
        if (scene == GameStateManager.Scene.None)
        {
            Debug.LogError($"Invalid Scene Transition {sceneInfo[0]}");
            yield break;
        }
        
        // If a positive vertical button is pressed (w or up), then transition
        if ((!_inputBlocked && Input.GetButton("Vertical") && verticalAxis > 0) || devControl) {
            devControl = false;
            // Block the input until the scene is loaded
            _inputBlocked = true;

            // -- Travel To Scene --
            _travelToSceneName = sceneInfo[0];
            // Load the scene
            yield return SceneManager.LoadSceneAsync(_gameController.gameStateManager.MatchScene(scene));
            // Set the scene
            _gameController.gameStateManager.SetScene(scene);

            //yield return new WaitForSeconds(0.1f); // <-- Necessary anymore?
            if (!_transition)
            {
                _directionTag = (sceneInfo.Count() > 1) ? sceneInfo[1] : "in";
                _transition = true;
            }
            if (_transition && _gameController.gameStateManager.currentState == GameStateManager.GameState.InGame)
            {
                RepositionPlayer(_travelToSceneName);
                LoadBackground();
                LoadCameraBounds();
            }
            
            yield return new WaitForSeconds(0.5f);
            _inputBlocked = false;
        }
    }

    /// <summary>
    /// Finds and passes the unique boundary of the newly loaded scene to each of the game's cameras.
    /// </summary>
    /// Needs a WaitForSeconds() before being called so the scene can fully load, or else
    /// will try to find the camera bounds of the previous scene.
    private void LoadCameraBounds()
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
    private void LoadBackground()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Background");
        Canvas canvas = background.GetComponent<Canvas>();

        canvas.planeDistance = 100;
        canvas.worldCamera = Camera.main;
    }

    /// <summary>
    /// Runs each time a new scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        _transition = false;
    }

    /// <summary>
    /// Repositions player based on landing position 
    /// </summary>
    /// <param name="travelToSceneName"></param>
    public void RepositionPlayer(string travelToSceneName) {
        _landingPosition = GetTransitionObjectPosition(travelToSceneName);
        PlayerController.Instance.playerCharacter.transform.position = _landingPosition;
        _transition = false;
    }

    /// <summary>
    /// Gets the position of the tagged game object
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public Vector3 GetTransitionObjectPosition(string sceneName) {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded) {
            string tag = (_directionTag.ToLower().Contains("in")) ? "TransitionObjectIn" : "TransitionObjectOut";

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