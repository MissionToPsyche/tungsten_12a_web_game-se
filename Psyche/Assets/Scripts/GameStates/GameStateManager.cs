using System.Collections.Generic;
using UnityEngine;

public class GameStateManager 
{
    //Private variables
    private GameController _gameController;
    private Dictionary<GameState, Dictionary<Scene, BaseState>> _gameStateToScene;

    // Public variables
    public GameState currentState { get; private set; } //state of the game

    public GameStateManager(GameController gameController, GameState state)
    {
        _gameController = gameController;
        currentState = state;

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

            //Paused state mappings
            { GameState.Paused, new Dictionary<Scene, BaseState>() {
                // Add mappings for Paused scenes if applicable
            } },

            //GameOver state mappings
            { GameState.GameOver, new Dictionary<Scene, BaseState>() {
                // Add mappings for GameOver scenes if applicable
            } }
        };
    }


    /// <summary>
    /// Holds the various states of the game (Experimental)
    /// - Can be expanded upon or retracted as the game is developed
    /// </summary>
    public enum GameState
    {
        MainMenu,
        InGame,
        GameOver,
        Paused,
        None,
    }

    /// <summary>
    /// Changes the state of the game
    /// </summary>
    /// <param name="state"></param>
    public void ChangeGameState(GameState state)
    {
        currentState = state;
        _gameController.HandleGameStateEvent(currentState);
    }

    /// <summary>
    /// Used to map the state of the game to the respective scenestate
    /// </summary>
    public enum Scene
    {
        Title_Screen,
        Intro_Cutscene,
        Outro_Cutscene,
        Landing_Scene,
        Tool_Intro_eMagnet,
        Tool_Intro_GRS,
        Tool_Intro_Imager,
        Tool_Intro_Thruster,

        None,
    }

    public string MatchScene(Scene scene)
    {
        return scene switch
        {
            Scene.Title_Screen          => "Title_Screen",
            Scene.Intro_Cutscene        => "Intro_Cutscene",
            Scene.Outro_Cutscene        => "Outro_Cutscene",
            Scene.Landing_Scene         => "Landing_Scene",
            Scene.Tool_Intro_eMagnet    => "Tool_Intro_eMagnet",
            Scene.Tool_Intro_GRS        => "Tool_Intro_GRS",
            Scene.Tool_Intro_Imager     => "Tool_Intro_Imager",
            Scene.Tool_Intro_Thruster   => "Tool_Intro_Thruster",
            _                           => null
        };
    }

    public Scene MatchScene(string scene)
    {
        return scene.ToLower() switch
        {
            "title_screen"        or "title"    => Scene.Title_Screen,
            "intro_cutscene"      or "intro"    => Scene.Intro_Cutscene,
            "outro_cutscene"      or "outro"    => Scene.Outro_Cutscene,
            "landing_scene"       or "landing"  => Scene.Landing_Scene,
            "tool_intro_emagnet"  or "emagnet"  => Scene.Tool_Intro_eMagnet,
            "tool_intro_grs"      or "grs"      => Scene.Tool_Intro_GRS,
            "tool_intro_imager"   or "imager"   => Scene.Tool_Intro_Imager,
            "tool_intro_thruster" or "thruster" => Scene.Tool_Intro_Thruster,
            _                                   => Scene.None,
        };
    }

    /// <summary>
    /// Takes in a GameState and Scene then uses that information to call LoadState() for that scene.
    /// -- LoadState() returns a dictionary of every tracked object and its latest saved state
    /// 
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="scene"></param>
    public void LoadSceneState(GameState gameState, Scene scene)
    {
        var stateManager = _gameStateToScene[gameState];
        stateManager[scene].LoadState();
    }

    public void SaveSceneState(GameState gameState, Scene scene)
    {
        var stateManager = _gameStateToScene[gameState];
        stateManager[scene].SaveState();
    }

    public void SetObjectState(GameState gameState, Scene scene, string key, object value)
    {
        var stateManager = _gameStateToScene[gameState];
        stateManager[scene].SetObjectState(key, value);
    }
}
