/*
 * Description: Scene Transitions
 * Authors: joshbenn, blopezro, mcmyers4
 * Version: 20240404
 */

using Cinemachine;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the transitions between scenes
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private variables
    private GameController  GameController;
    private Vector3         LandingPosition;
    private bool            Transition = false;
    private string          TravelFromSceneName;
    private string          TravelToSceneName;
    private string          DirectionTag;
    private bool            InputBlocked = false;

    // Public variables
    public bool             DevControl = false;

    /// <summary>
    /// Initializes this script
    /// </summary>
    /// <param name="playerManagement"></param>
    public void Initialize(GameController gameController)
    {
        GameController = gameController;
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
    }

    //================================================== Events ==================================================

    /// <summary>
    /// Loads player
    /// </summary>
    public void LoadPlayer()
    {
        PlayerController.Instance.playerCollisionManager.InitiateTransition += OnInitiateTransition;
    }

    /// <summary>
    /// Loads dev console
    /// </summary>
    public void LoadDevConsole()
    {
        GameController.DeveloperConsole.OnDevConsoleTransition += OnInitiateTransition;
    }

    //============================================== Scene Management ============================================

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
    /// <returns><see cref="IEnumerator"/></returns>
    public IEnumerator CheckTransition(string objectLabel)
    {
        string[] sceneInfo = objectLabel.Split(' ');

        // get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        // cross reference scene -- None == Non-Scene Input
        GameStateManager.Scene scene = GameController.GameStateManager.MatchScene(sceneInfo[0]);

        if (scene == GameStateManager.Scene.None)
        {
            Debug.LogError($"Invalid Scene Transition {sceneInfo[0]}");
            yield break;
        }
        
        // if a positive vertical button is pressed (w or up), then transition
        if ((!InputBlocked && Input.GetButton("Vertical") && verticalAxis > 0) || DevControl) 
        {
            DevControl = false;
            // block the input until the scene is loaded
            InputBlocked = true;
            // -- travel from scene --
            TravelFromSceneName = SceneManager.GetActiveScene().name;
            // -- travel to scene --
            TravelToSceneName = sceneInfo[0];
            // load the scene
            yield return SceneManager.LoadSceneAsync(GameController.GameStateManager.MatchScene(scene));
            // set the scene
            GameController.GameStateManager.SetScene(scene);

            if (!Transition)
            {
                DirectionTag = (sceneInfo.Count() > 1) ? sceneInfo[1] : "out";
                Transition = true;
            }

            if (Transition && GameController.GameStateManager.CurrentState == GameStateManager.GameState.InGame)
            {
                RepositionPlayer(TravelToSceneName);
                LoadBackground();
                LoadCameraBounds();
            }
            
            yield return new WaitForSeconds(0.5f);
            InputBlocked = false;
        }
    }

    /// <summary>
    /// Finds and passes the unique boundary of the newly loaded scene to each of the game's cameras.
    /// Needs a WaitForSeconds() before being called so the scene can fully load, or else
    /// will try to find the camera bounds of the previous scene.
    /// </summary>
    private void LoadCameraBounds()
    {
        // array of the virtual cameras attached to the player
        GameObject[] vcs = GameObject.FindGameObjectsWithTag("VirtualCamera");
        GameObject bounds;
        CompositeCollider2D shape;

        // loops through and assigns the boundary to each of the virtual cameras
        foreach (GameObject vc in vcs)
        {
            vc.GetComponent<CinemachineConfiner2D>().InvalidateCache();
            bounds = GameObject.FindGameObjectWithTag("Boundary");
            shape = bounds.GetComponent<CompositeCollider2D>();
            vc.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = shape;
        }
    }

    /// <summary>
    /// Finds the main camera after loading into a scene and connects the background to the camera.
    /// Thus, the background always follows the camera.
    /// </summary>
    private void LoadBackground()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Background");
        Canvas canvas = background.GetComponent<Canvas>();

        canvas.planeDistance = 100;
        canvas.worldCamera = Camera.main;
    }

    /// <summary>
    /// Repositions player based on landing position 
    /// </summary>
    /// <param name="travelToSceneName"></param>
    public void RepositionPlayer(string travelToSceneName)
    {
        LandingPosition = GetTransitionObjectPosition(travelToSceneName);
        PlayerController.Instance.playerCharacter.transform.position = LandingPosition;
        Transition = false;
    }

    /// <summary>
    /// Gets the position of the tagged game object
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns><see cref="Vector3"/></returns>
    public Vector3 GetTransitionObjectPosition(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.isLoaded)
        {
            // if the tag is "out" as in if "out" is part of the game object the player is interacting with
            // then we want to "look for" the TransitionObjectIn (in) on the scene we are going into, else
            // we must be going in reverse so then we look for the TransitionObjectOut (out)
            string tag = DirectionTag.ToLower().Contains("out") ? "TransitionObjectIn" : "TransitionObjectOut";

            // there should only be one transition object in and one transition object out for each scene
            // with the exception of two "outs" and "ins" for the Landing scene:
            // Landing scene out to Imager level    // Landing scene out to Outro Cutscene scene
            // Landing scene in from Title Screen   // Landing scene in from Combo 3
            GameObject[] objectsWithTransitionTag = GameObject.FindGameObjectsWithTag(tag);
            GameObject caveObject = null;

            // We differentiate the proper transition objects for the player with the layer as well
            // Default caves use the default layer while special case transition objects use other layers
            foreach (GameObject obj in objectsWithTransitionTag)
            {

                //Debug.Log(GameController.Instance.GameStateManager.MatchScene(TravelFromSceneName) == GameStateManager.Scene.Combo3);
                //Debug.Log(GameController.Instance.GameStateManager.MatchScene(sceneName) == GameStateManager.Scene.Landing);
                //Debug.Log(obj.layer == 8);

                // If the object in the list is the cave layered transition object and we ARE going from Combo 3 to Landing
                if (GameController.Instance.GameStateManager.MatchScene(TravelFromSceneName) == GameStateManager.Scene.Combo3
                    && GameController.Instance.GameStateManager.MatchScene(sceneName) == GameStateManager.Scene.Landing) // cave layer
                {
                    if (obj.layer == 8)
                    {
                        caveObject = obj;
                        break; // break since we only need one GameObject
                    }
                }
                // If the object in the list is the default transition object and we ARE NOT going from Combo 3 to Landing
                else if (obj.layer == 0) // default layer
                {
                    caveObject = obj;                   
                    break; // break since we only need one GameObject
                }
            }

            // return proper cave object position
            if (caveObject != null) 
            {
                return caveObject.transform.position;
            }
            else
            {
                Debug.LogWarning(tag+" not found.");
            }

        } 
        else 
        {
            Debug.LogWarning("Scene "+sceneName+" is not loaded.");
        }

        return Vector3.zero;
    }
}