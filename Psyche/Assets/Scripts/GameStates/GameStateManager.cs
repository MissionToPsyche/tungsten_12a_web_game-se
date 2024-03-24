using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameStateManager;

public class GameStateManager : MonoBehaviour
{
    //Private variables
    private GameController _gameController;
    private Dictionary<Scene, BaseState> _gameStateToScene;

    // Public variables
    public GameState currentState { get; private set; } //state of the game
    public Scene currentScene { get; private set; }     // Current scene of the game
    public int checkpoint { get; set; }                 // Last checkpoint
    public Vector3 respawnPoint { get; set; }           // Last respawn point
    public Vector3 startPoint { get; set; }             //Initial character location when level first begins



    public void Initialize(GameController gameController, string scene)
    {
        _gameController = gameController;
        _gameStateToScene = new Dictionary<Scene, BaseState>()
        {
            //InGame state mappings
            { Scene.Landing,    new Landing_State() },
            { Scene.Imager,     new Imager_State() },
            { Scene.GRNS,       new GRNS_State() },
            { Scene.eMagnet,    new eMagnet_State() },
            { Scene.Thruster,   new Thruster_State() },
            { Scene.Combo1,     new Combo1_State() },
            { Scene.Combo2,     new Combo2_State() },
            { Scene.Combo3,     new Combo3_State() }
        };
        
        // Set up the current scene
        currentScene = MatchScene(scene);
        SetScene(currentScene);
    }

    /// <summary>
    /// When the PlayerCnotroller loads, it will call this function to set up the communication events
    /// </summary>
    public void LoadPlayer()
    {
        PlayerController.Instance.inventoryManager.SetObjectState += SetObjectState;
        PlayerController.Instance.playerCollisionManager.SetObjectState += SetObjectState;
        PlayerController.Instance.playerCollisionManager.SaveSceneState += SaveSceneState;
    }

    public void UnloadPlayer()
    {
        PlayerController.Instance.inventoryManager.SetObjectState -= SetObjectState;
        PlayerController.Instance.playerCollisionManager.SetObjectState += SetObjectState;
        PlayerController.Instance.playerCollisionManager.SaveSceneState += SaveSceneState;
    }

    /// <summary>
    /// Holds the various states of the game (Experimental)
    /// - Can be expanded upon or retracted as the game is developed
    /// </summary>
    public enum GameState
    {
        MainMenu, InGame, None,
    }

    /// <summary>
    /// Used to map the state of the game to the respective scenestate
    /// </summary>
    public enum Scene
    {
        // Main Menu
        Title, Intro, Outro, End,

        // In Game
        Landing, eMagnet, GRNS, Imager, Thruster,
        Combo1,  Combo2,  Combo3,

        // Error state or incorrect value passed
        None,
    }

    /// <summary>
    /// Matches the passed scene enum value with a string output
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public string MatchScene(Scene scene)
    {
        return scene switch
        {
            Scene.Title      => "00_Title_Screen",
            Scene.Intro      => "01_Cutscene_Intro",
            Scene.Landing    => "02_Landing",
            Scene.Imager     => "03_Imager",
            Scene.GRNS       => "04_GRNS",
            Scene.eMagnet    => "05_eMagnet",
            Scene.Thruster   => "06_Thruster",
            Scene.Combo1     => "07_Combo_1",
            Scene.Combo2     => "08_Combo_2",
            Scene.Combo3     => "09_Combo_3",
            Scene.Outro      => "10_Cutscene_Outro",
            Scene.End        => "Game_End",
            _                => null
        };
    }

    /// <summary>
    /// Matches the given string with the respective Scene enum value
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public Scene MatchScene(string scene)
    {
        return scene.ToLower() switch
        {
            "00_title_screen"   or "title"    => Scene.Title,
            "01_cutscene_intro" or "intro"    => Scene.Intro,
            "02_landing"        or "landing"  => Scene.Landing,
            "03_imager"         or "imager"   => Scene.Imager,
            "04_grns"           or "grns"     => Scene.GRNS,
            "05_emagnet"        or "emagnet"  => Scene.eMagnet,
            "06_thruster"       or "thruster" => Scene.Thruster,
            "07_combo_1"        or "combo1"   => Scene.Combo1,
            "08_combo_2"        or "combo2"   => Scene.Combo2,
            "09_combo_3"        or "combo3"   => Scene.Combo3,
            "10_cutscene_outro" or "outro"    => Scene.Outro,
            "game_end"          or "end"      => Scene.End,
            _                                 => Scene.None,
        };
    }

    /// <summary>
    /// Sets the current scene for the game state
    /// </summary>
    /// <param name="scene"></param>
    public void SetScene(Scene scene)
    {
        // Check for None
        if (scene == Scene.None) { return; }

        // Set current scene
        currentScene = scene;

        // Set current gamestate based on passed scene
        switch (scene)
        {
            case Scene.Title or Scene.Intro or Scene.Outro or Scene.End:
                currentState = GameState.MainMenu;
                break;
            
            default:
                currentState = GameState.InGame;
                break;
        }

        _gameController.HandleGameStateEvent();

        // Scenes without a viable scene state are ignored
        if (currentState == GameState.InGame)
        {
            LoadSceneState();
        }
    }

    /// <summary>
    /// Takes in a GameState and Scene then uses that information to call LoadState() for that scene.
    /// -- LoadState() returns a dictionary of every tracked object and its latest saved state
    /// 
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="scene"></param>
    public void LoadSceneState()
    {
        // If not in game, return
        if (currentState != GameState.InGame)
        {
            return;
        }

        _gameStateToScene[currentScene].LoadState();
    }

    /// <summary>
    /// Saves the scene state of the current scene
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="scene"></param>
    public void SaveSceneState()
    {
        // If not in game, return
        if (currentState != GameState.InGame) { 
            return; 
        }

        _gameStateToScene[currentScene].SaveState();
    }

    /// <summary>
    /// Sets the state of an object within the current scene
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetObjectState(string key, object value)
    {
        // If not in game, return
        if (currentState != GameState.InGame)
        {
            return;
        }

        _gameStateToScene[currentScene].SetObjectState(key, value);
    }

    /// <summary>
    /// Resets the state of all InGame scenes when called
    /// </summary>
    public void ResetScenes()
    {
        foreach (var scene in _gameStateToScene)
        {
            scene.Value.LoadDefaultState();
        }
    }

    /// <summary>
    /// This co-routine forces the game to wait for the player's warping
    /// animation to complete before continuing on.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Warp()
    {
        //block player controls
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.beingWarped = true;

        //wait for the animation to be completed
        yield return new WaitForSeconds(1.2f);

        //check if the player is at the starting point
        if (startPoint.Equals(respawnPoint))
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            //changed so that camera bounds would load on player repawn
            StartCoroutine(GameController.Instance.sceneTransitionManager.CheckTransition(SceneManager.GetActiveScene().name));
        }

        //move the player to their respawn point
        PlayerController.Instance.transform.position = respawnPoint;

        PlayerController.Instance.playerHealth.HealthUp(100);
        PlayerController.Instance.batteryManager.Activate();

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
        PlayerController.Instance.beingWarped = false;
    }
}