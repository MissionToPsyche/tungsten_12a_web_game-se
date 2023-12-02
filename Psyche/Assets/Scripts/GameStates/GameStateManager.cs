using System.Collections.Generic;

public class GameStateManager 
{
    //Private variables
    private GameController _gameController;
    private Dictionary<GameState, Dictionary<SceneState, BaseState>> _gameStateToScene;

    // Public variables
    public GameState currentState { get; private set; } //state of the game

    public GameStateManager(GameController gameController, GameState state)
    {
        _gameController = gameController;
        currentState = state;

        //Expand upon later
        _gameStateToScene = new Dictionary<GameState, Dictionary<SceneState, BaseState>>()
        {
            //MainMenu state mappings
            { GameState.MainMenu, new Dictionary<SceneState, BaseState>() {
                /*{ SceneState.Title_Screen, new MainMenuState() },*/
            } },

            //InGame state mappings
            { GameState.InGame, new Dictionary<SceneState, BaseState>() {
                /*{ SceneState.Intro_Cutscene, new IntroCutsceneState() },
                { SceneState.Landing_Scene, new LandingSceneState() },*/
                { SceneState.Tool_Intro_eMagnet, new Tool_Intro_eMagnetState() },
            } },

            //Paused state mappings
            { GameState.Paused, new Dictionary<SceneState, BaseState>() {
                // Add mappings for Paused scenes if applicable
            } },

            //GameOver state mappings
            { GameState.GameOver, new Dictionary<SceneState, BaseState>() {
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
        Paused,
        GameOver,
    }

    /// <summary>
    /// Used to map the state of the game to the respective scenestate
    /// </summary>
    public enum SceneState
    {
        Title_Screen,
        Intro_Cutscene,
        Outro_Cutscene,
        Landing_Scene,
        Tool_Intro_eMagnet,
        Tool_Intro_GRS,
        Tool_Intro_Imager,
        Tool_Intro_Thruster,
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
}
