using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    //Private variables
    private GameController _gameController;
    private Dictionary<GameState, Dictionary<Scene, BaseState>> _gameStateToScene;

    // Public variables
    public GameState currentState { get; private set; } //state of the game
    public Scene currentScene { get; private set; }     // Current scene of the game

    public void Initialize(GameController gameController, string scene)
    {
        _gameController = gameController;
        currentScene = MatchScene(scene);
        SetScene(currentScene);

        //Expand upon later
        _gameStateToScene = new Dictionary<GameState, Dictionary<Scene, BaseState>>()
        {
            //MainMenu state mappings
            { GameState.MainMenu, new Dictionary<Scene, BaseState>() {
                /*{ SceneState.Title_Screen, new MainMenuState() },*/
            } },

            //InGame state mappings
            { GameState.InGame, new Dictionary<Scene, BaseState>() {
                /*{ SceneState.Intro_Cutscene, new IntroCutsceneState() },
                { SceneState.Landing_Scene, new LandingSceneState() },*/
                { Scene.Tool_Intro_eMagnet, new Tool_Intro_eMagnetState() },
            } },
        };
    }

    public void LoadPlayer()
    {
        PlayerController.Instance.SetObjectState += SetObjectState;
    }

    public void OnEnable()
    {
        if (PlayerController.Instance != null)
        {
            LoadPlayer();
        }
    }

    public void OnDisable()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetObjectState -= SetObjectState;
        }
    }


    /// <summary>
    /// Holds the various states of the game (Experimental)
    /// - Can be expanded upon or retracted as the game is developed
    /// </summary>
    public enum GameState
    {
        MainMenu,
        InGame,

        None,
    }

    /// <summary>
    /// Changes the state of the game
    /// </summary>
    /// <param name="state"></param>
    public void SetGameState(GameState state)
    {
        currentState = state;
        _gameController.HandleGameStateEvent(currentState);
    }

    /// <summary>
    /// Used to map the state of the game to the respective scenestate
    /// </summary>
    public enum Scene
    {
        // Main Menu
        Title_Screen,
        Intro_Cutscene,
        Outro_Cutscene,
        SceneTransition_Game_End,

        // In Game
        Landing_Scene,
        Tool_Intro_eMagnet,
        Tool_Intro_GRS,
        Tool_Intro_Imager,
        Tool_Intro_Thruster,
        Tool_Combo_1,
        Tool_Combo_2,
        

        None,
    }

    public string MatchScene(Scene scene)
    {
        return scene switch
        {
            Scene.Title_Screen              => "Title_Screen",
            Scene.Intro_Cutscene            => "Intro_Cutscene",
            Scene.Outro_Cutscene            => "Outro_Cutscene",
            Scene.Landing_Scene             => "Landing_Scene",
            Scene.Tool_Intro_eMagnet        => "Tool_Intro_eMagnet",
            Scene.Tool_Intro_GRS            => "Tool_Intro_GRS",
            Scene.Tool_Intro_Imager         => "Tool_Intro_Imager",
            Scene.Tool_Intro_Thruster       => "Tool_Intro_Thruster",
            Scene.Tool_Combo_1              => "Tool_Comb_1",
            Scene.Tool_Combo_2              => "Combo_2",
            Scene.SceneTransition_Game_End  => "SceneTransition_Game_End",
            _                               => null
        };
    }

    public Scene MatchScene(string scene)
    {
        return scene.ToLower() switch
        {
            "title_screen"              or "title"    => Scene.Title_Screen,
            "intro_cutscene"            or "intro"    => Scene.Intro_Cutscene,
            "outro_cutscene"            or "outro"    => Scene.Outro_Cutscene,
            "landing_scene"             or "landing"  => Scene.Landing_Scene,
            "tool_intro_emagnet"        or "emagnet"  => Scene.Tool_Intro_eMagnet,
            "tool_intro_grs"            or "grs"      => Scene.Tool_Intro_GRS,
            "tool_intro_imager"         or "imager"   => Scene.Tool_Intro_Imager,
            "tool_intro_thruster"       or "thruster" => Scene.Tool_Intro_Thruster,
            "tool_comb_1"               or "combo1"   => Scene.Tool_Combo_1,
            "combo_2"                   or "combo2"   => Scene.Tool_Combo_2,
            "scenetransition_game_end"  or "end"      => Scene.SceneTransition_Game_End,
            _                                         => Scene.None,
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
            case Scene.Title_Screen or Scene.Intro_Cutscene or Scene.Outro_Cutscene or Scene.SceneTransition_Game_End:
                SetGameState(GameState.MainMenu);
                break;
            
            default:
                SetGameState(GameState.InGame);
                break;
            
        }
        
        // Scenes without a viable scene state are ignored
        if (currentState == GameState.InGame || scene == Scene.Title_Screen)
        {
            // Temporary check until the respective states are added
            if (scene != Scene.Tool_Intro_eMagnet)
            {
                Debug.Log($"Scene state loader for {scene} not yet implemented");
            }
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
        // Temporary until others are fully implemented
        if (currentScene != Scene.Tool_Intro_eMagnet) { return; }

        var stateManager = _gameStateToScene[currentState];
        stateManager[currentScene].LoadState();
    }

    /// <summary>
    /// Saves the scene state of the current scene
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="scene"></param>
    public void SaveSceneState()
    {
        // Temporary until others are fully implemented
        if (currentScene != Scene.Tool_Intro_eMagnet) { return; }

        var stateManager = _gameStateToScene[currentState];
        stateManager[currentScene].SaveState();
    }

    /// <summary>
    /// Sets the state of an object within the current scene
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetObjectState(string key, object value)
    {
        // Temporary until others are fully implemented
        if (currentScene != Scene.Tool_Intro_eMagnet) { return; }

        var stateManager = _gameStateToScene[currentState];
        stateManager[currentScene].SetObjectState(key, value);
    }
}
