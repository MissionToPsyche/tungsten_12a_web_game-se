using System;
using System.Collections;
using UnityEngine;

public class GameController : BaseController<GameController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //Private variables


    //Public variables
    public DeveloperConsole developerConsole;
    public GameStateManager gameStateManager;
    public SceneTransitionManager sceneTransitionManager;
    public AudioManager audioManager;

    public bool colorBlindMode;

    /// <summary>
    /// Initialize the object and parent class
    /// -- Order: GameState -> Audio -> SceneManager -> DeveloperConsole
    /// </summary>
    public override void Initialize()
    {
        // Set up the gamestate
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        gameStateManager = GetComponent<GameStateManager>();
        gameStateManager.Initialize(this, scene);

        // Set up the audio manager
        audioManager = GameObject.FindGameObjectWithTag("AudioSources").GetComponent<AudioManager>();
        if (sceneTransitionManager == null)
        {
            sceneTransitionManager = GetComponent<SceneTransitionManager>();
            sceneTransitionManager.Initialize(this);
        }
        developerConsole = GetComponent<DeveloperConsole>();
        developerConsole.Initialize(this);
    }

    /// <summary>
    /// Runs when scene starts
    ///   - Calls base.Awake()
    ///   - Initializes script
    /// </summary>
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

    /// <summary>
    /// Called just before first frame
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Called when necessary - not every frame
    /// </summary>
    public override void UpdateController()
    {
        //Insert Logic
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    void Update()
    {
        //Implement update logic

    }

    /// <summary>
    /// For shutting down and cleaning up
    /// </summary>
    public override void Shutdown()
    {
        base.Shutdown();
    }

    //======================================================== Events ========================================================

    //Event Definitions
    //Gamestate changed
    public event Action<GameStateManager.GameState> OnGameStateChanged;



    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnEnable()
    {
        if (UIController.Instance != null)
        {

        }
        if (PlayerController.Instance != null)
        {

        }
    }

    /// <summary>
    /// Temporary workaround for loading the UI and dev console if the TitleScreen was loaded into first
    /// </summary>
    public void LoadUI() { }

    /// <summary>
    /// Temporary workaround for loading the PlayerController if TitleScreen was loaded into first
    /// </summary>
    public void LoadPlayer()
    {
        gameStateManager.LoadPlayer();
        sceneTransitionManager.LoadPlayer();
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if(UIController.Instance != null)
        {

        }
        if(PlayerController.Instance != null)
        {

        }
    }

    

    //======================================================= Gamestate ======================================================

    /// <summary>
    /// Event handling for game state changes
    /// </summary>
    /// <param name="gameState"></param>
    public void HandleGameStateEvent()
    {
        OnGameStateChanged?.Invoke(gameStateManager.currentState);
        // Handle game state updates
        switch (gameStateManager.currentState)
        {
            case GameStateManager.GameState.MainMenu:
                gameStateManager.ResetScenes();
                break;
            case GameStateManager.GameState.InGame:
                // Update pause logic
                break;
            default:
                Debug.LogError("Game in unknown state!");
                break;

        }
    }

    /// <summary>
    /// Swap color blind mode
    /// </summary>
    public void ChangeColorBlindMode(bool mode)
    {
        colorBlindMode = mode;
    }
}
