/**
 * Authors: JoshBenn
 */
using System;
using UnityEngine;

/// <summary>
/// GameController class for controlling various aspects of the game.
/// </summary>
public class GameController : BaseController<GameController>
{
    //============================================== Initialize/Updates/Destroy ==============================================
    //Public variables
    public DeveloperConsole DeveloperConsole;
    public GameStateManager GameStateManager;
    public SceneTransitionManager SceneTransitionManager;
    public AudioManager AudioManager;
    public bool ColorBlindMode;

    /// <summary>
    /// Initialize the object and parent class
    /// -- Order: GameState -> Audio -> SceneManager -> DeveloperConsole
    /// </summary>
    public override void Initialize()
    {
        // Set up the gamestate
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        GameStateManager = GetComponent<GameStateManager>();
        GameStateManager.Initialize(this, scene);

        // Set up the audio manager
        AudioManager = GameObject.FindGameObjectWithTag("AudioSources").GetComponent<AudioManager>();
        if (SceneTransitionManager == null)
        {
            SceneTransitionManager = GetComponent<SceneTransitionManager>();
            SceneTransitionManager.Initialize(this);
        }
        DeveloperConsole = GetComponent<DeveloperConsole>();
        DeveloperConsole.Initialize(this);
    }

    /// <inheritdoc/>
    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Initialize();
        }
    }

    /// <inheritdoc />
    public override void Shutdown()
    {
        base.Shutdown();
    }

    //======================================================== Events ========================================================

    //Event Definitions
    public event Action<GameStateManager.GameState> OnGameStateChanged;

    /// <summary>
    /// Called to force entities to load PlayerController interactivity.
    /// </summary>
    public void LoadPlayer()
    {
        GameStateManager.LoadPlayer();
        SceneTransitionManager.LoadPlayer();
    }

    //======================================================= Gamestate ======================================================

    /// <summary>
    /// Event handling for game state changes
    /// </summary>
    public void HandleGameStateChange()
    {
        OnGameStateChanged?.Invoke(GameStateManager.currentState);
        if (GameStateManager.currentState == GameStateManager.GameState.MainMenu)
        {
            GameStateManager.ResetScenes();

        }
    }

    /// <summary>
    /// Swap color blind mode
    /// </summary>
    public void ChangeColorBlindMode(bool mode)
    {
        ColorBlindMode = mode;
    }
}
