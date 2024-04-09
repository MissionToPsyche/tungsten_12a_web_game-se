/*
 * Authors: JoshBenn
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy ========================================
    //Private variables
    private GameController GameController;
    private Dictionary<Scene, BaseState> GameStateToScene;

    // Public variables
    public GameState CurrentState { get; private set; } //state of the game
    public Scene CurrentScene { get; private set; }     // Current scene of the game
    public int Checkpoint { get; set; }                 // Last checkpoint
    public Vector3 RespawnPoint { get; set; }           // Last respawn point
    public Vector3 StartPoint { get; set; }             //Initial character location when level first begins


    /// <summary>
    /// Initializes the <see cref="GameStateManager"/> with a starting <see cref="Scene"/>.
    /// </summary>
    /// <param name="gameController"><see cref="GameController"/></param>
    /// <param name="scene"><see cref="string"/> value for the <see cref="Scene"/></param>
    public void Initialize(GameController gameController, string scene)
    {
        GameController = gameController;
        GameStateToScene = new Dictionary<Scene, BaseState>()
        {
            //InGame state mappings
            { Scene.Landing,    new Landing_State() },
            { Scene.Imager,     new Imager_State() },
            { Scene.Grns,       new GRNS_State() },
            { Scene.EMagnet,    new EMagnet_State() },
            { Scene.Thruster,   new Thruster_State() },
            { Scene.Combo1,     new Combo1_State() },
            { Scene.Combo2,     new Combo2_State() },
            { Scene.Combo3,     new Combo3_State() }
        };
        
        // Set up the current scene
        CurrentScene = MatchScene(scene);
        SetScene(CurrentScene);
    }

    //================================================= Events =================================================

    /// <summary>
    /// <see cref="PlayerController"/> initializes the events when loaded.
    /// </summary>
    public void LoadPlayer()
    {
        PlayerController.Instance.inventoryManager.SetObjectState += SetObjectState;
        PlayerController.Instance.playerCollisionManager.SetObjectState += SetObjectState;
        PlayerController.Instance.playerCollisionManager.SaveSceneState += SaveSceneState;
    }

    /// <summary>
    /// <see cref="PlayerController"/> uninitializes the events when unloaded.
    /// </summary>
    public void UnloadPlayer()
    {
        PlayerController.Instance.inventoryManager.SetObjectState -= SetObjectState;
        PlayerController.Instance.playerCollisionManager.SetObjectState += SetObjectState;
        PlayerController.Instance.playerCollisionManager.SaveSceneState += SaveSceneState;
    }

    //================================================= Enums =================================================
    /// <summary>
    /// For representing the state of the game.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// When in <see cref="Scene.Title"/>, <see cref="Scene.Intro"/>, <see cref="Scene.Outro"/>, or <see cref="Scene.End"/>.
        /// </summary>
        MainMenu,

        /// <summary>
        /// When in <see cref="Scene.Landing"/>, <see cref="Scene.Imager"/>, <see cref="Scene.Grns"/>, <see cref="Scene.EMagnet"/> 
        /// <see cref="Scene.Thruster"/>, <see cref="Scene.Combo1"/>, <see cref="Scene.Combo2"/>, or <see cref="Scene.Combo3"/>.
        /// </summary>
        InGame, 

        /// <summary>
        /// For erroneous states.
        /// </summary>
        None,
    }

    /// <summary>
    /// Represents the scenes of the game.
    /// </summary>
    public enum Scene
    {
        // ===== Main Menu =====
        /// <summary>
        /// For Title_Screen Scene.
        /// </summary>
        Title,

        /// <summary>
        /// For Cutscene_Intro Scene.
        /// </summary>
        Intro,

        /// <summary>
        /// For Cutscene_Outro Scene.
        /// </summary>
        Outro,

        // ===== In Game =====
        /// <summary>
        /// For Landing Scene.
        /// </summary>
        Landing,

        /// <summary>
        /// For eMagnet Scene.
        /// </summary>
        EMagnet,

        /// <summary>
        /// For GRNS Scene.
        /// </summary>
        Grns,

        /// <summary>
        /// For Imager Scene.
        /// </summary>
        Imager,

        /// <summary>
        /// For Thruster Scene.
        /// </summary>
        Thruster,

        /// <summary>
        /// For Combo_1 Scene.
        /// </summary>
        Combo1,

        /// <summary>
        /// For Combo_2 Scene.
        /// </summary>
        Combo2,
        
        /// <summary>
        /// For Combo_3 Scene.
        /// </summary>
        Combo3,

        // ===== Error ======
        /// <summary>
        /// For erroneous values.
        /// </summary>
        None,
    }

    /// <summary>
    /// Matches the <see cref="Scene"/> variant with its <see cref="string"/> value.
    /// </summary>
    /// <param name="scene"><see cref="Scene"/> variant</param>
    /// <returns><see cref="string"/> value or <see cref="null"/> if no matches were found</returns>
    public string MatchScene(Scene scene)
    {
        return scene switch
        {
            Scene.Title      => "00_Title_Screen",
            Scene.Intro      => "01_Cutscene_Intro",
            Scene.Landing    => "02_Landing",
            Scene.Imager     => "03_Imager",
            Scene.Grns       => "04_GRNS",
            Scene.EMagnet    => "05_eMagnet",
            Scene.Thruster   => "06_Thruster",
            Scene.Combo1     => "07_Combo_1",
            Scene.Combo2     => "08_Combo_2",
            Scene.Combo3     => "09_Combo_3",
            Scene.Outro      => "10_Cutscene_Outro",
            _                => null
        };
    }

    /// <summary>
    /// Matches a <see cref="string"/> value with its respective <see cref="Scene"/> variant.
    /// </summary>
    /// <param name="scene"><see cref="string"/> scene</param>
    /// <returns><see cref="Scene"/> variant</returns>
    public Scene MatchScene(string scene)
    {
        return scene.ToLower() switch
        {
            "00_title_screen"   or "title"    => Scene.Title,
            "01_cutscene_intro" or "intro"    => Scene.Intro,
            "02_landing"        or "landing"  => Scene.Landing,
            "03_imager"         or "imager"   => Scene.Imager,
            "04_grns"           or "grns"     => Scene.Grns,
            "05_emagnet"        or "emagnet"  => Scene.EMagnet,
            "06_thruster"       or "thruster" => Scene.Thruster,
            "07_combo_1"        or "combo1"   => Scene.Combo1,
            "08_combo_2"        or "combo2"   => Scene.Combo2,
            "09_combo_3"        or "combo3"   => Scene.Combo3,
            "10_cutscene_outro" or "outro"    => Scene.Outro,
            _                                 => Scene.None,
        };
    }

    //============================================= State Management =============================================
    /// <summary>
    /// Sets the current <see cref="Scene"/> for the game state.
    /// </summary>
    /// <param name="scene"><see cref="Scene"/> variant</param>
    public void SetScene(Scene scene)
    {
        // Check for None
        if (scene == Scene.None) 
        { 
            return; 
        }

        // Set current scene
        CurrentScene = scene;

        // Set current gamestate based on passed scene
        CurrentState = scene switch
        {
            Scene.Title or Scene.Intro or Scene.Outro => GameState.MainMenu,
            _ => GameState.InGame,
        };

        // Have the GameController handle the events of Game State change
        GameController.HandleGameStateChange();

        // Load the requisite scene state
        if (CurrentState == GameState.InGame)
        {
            LoadSceneState();
        }
    }

    /// <summary>
    /// Loads the saved <see cref="Scene"/> state.
    /// </summary>
    public void LoadSceneState()
    {
        // If not in game, return
        if (CurrentState != GameState.InGame)
        {
            return;
        }

        GameStateToScene[CurrentScene].LoadState();
    }

    /// <summary>
    /// Saves the state of the current <see cref="Scene"/>.
    /// </summary>
    public void SaveSceneState()
    {
        // If not in game, return
        if (CurrentState != GameState.InGame) 
        { 
            return; 
        }

        GameStateToScene[CurrentScene].SaveState();
    }

    /// <summary>
    /// Sets the state of an object within the current <see cref="Scene"/>.
    /// </summary>
    /// <param name="key"><see cref="string"/> object name</param>
    /// <param name="value"><see cref="object"/> value to set the object to</param>
    public void SetObjectState(string key, object value)
    {
        // If not in game, return
        if (CurrentState != GameState.InGame)
        {
            return;
        }

        GameStateToScene[CurrentScene].SetObjectState(key, value);
    }

    /// <summary>
    /// Resets the state of all <see cref="GameState.Ingame"/> scenes.
    /// </summary>
    public void ResetScenes()
    {
        foreach (KeyValuePair<Scene, BaseState> scene in GameStateToScene)
        {
            scene.Value.LoadDefaultState();
        }
    }

    /// <summary>
    /// Forces the game to wait for the player's warping animation to complete before continuing.
    /// </summary>
    /// <returns><see cref="IEnumerator"/></returns>
    public IEnumerator Warp()
    {
        //block player controls
        PlayerController.Instance.inputBlocked = true;
        PlayerController.Instance.beingWarped = true;

        //wait for the animation to be completed
        yield return new WaitForSeconds(1.2f);

        //check if the player is at the starting point
        if (StartPoint.Equals(RespawnPoint))
        {
            //changed so that camera bounds would load on player repawn
            StartCoroutine(GameController.Instance.SceneTransitionManager.CheckTransition(SceneManager.GetActiveScene().name));
        }

        //move the player to their respawn point
        PlayerController.Instance.transform.position = RespawnPoint;

        PlayerController.Instance.playerHealth.HealthUp(100);
        PlayerController.Instance.solarArrayManager.Activate();

        //unblock player controls
        PlayerController.Instance.inputBlocked = false;
        PlayerController.Instance.beingWarped = false;
    }
}