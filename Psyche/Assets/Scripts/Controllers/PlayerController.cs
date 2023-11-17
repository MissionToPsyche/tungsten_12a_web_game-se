using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Player Management script controls how the player interacts with the 
/// system and various components.
/// </summary>
public class PlayerController : BaseController<PlayerController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //TEMPORARY <-----------------------------------------REMOVE WHEN NO LONGER NECESSARY
    GameController gameController;

    //Create the playercharacter assignment
    [Header("Components")]
    public Rigidbody2D playerCharacter;
    public BoxCollider2D playerCollider;

    //Set up environmental checks
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    //Booleans for environmental checks
    [HideInInspector] public bool isGrounded;

    //Management scripts
    [Header("Scripts")]
    public PlayerHealth playerHealth;
    public BatteryManager batteryManager;
    public PlayerMovement playerMovement;
    public ImagerManager imagerManager;
    public EMagnetManager eMagnetManager;
    public ThrusterManager thrusterManager;
    public ImagerCursor flashlight;
    public GammaView gammaView;
    public AudioManager audioManager;
    private TransitionManager sceneTransition;
    public PlayerDeath deathCon;

    //Booleans for the various tools
    private Dictionary<ToolManager, bool> _toolStates;
    private bool hasSpectrometer;                         // <--Where is the spectrometer script? Add to _toolStates
    private bool usingThruster; //for animation purposes  // <--Implement boolean in thruster script

    //Booleans to prevent needless code runs
    [HideInInspector] public bool eMagnetActive, beingPulled, inputBlocked; //Create a dictionary or list to track these

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        //Initialize base
        base.Initialize();

        //TEMPORARY <-----------------------------------------REMOVE WHEN NO LONGER NECESSARY
        gameController = FindAnyObjectByType<GameController>();

        //Dictionary for tool states
        _toolStates = new Dictionary<ToolManager, bool>();

        //Assign and initialize scripts
        playerCharacter = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(this);
        // ##### Tool Managers #####
        batteryManager = GetComponent<BatteryManager>();
        batteryManager.Initialize(this);
        thrusterManager = GetComponent<ThrusterManager>();
        thrusterManager.Initialize(this);
        eMagnetManager = GetComponent<EMagnetManager>();
        eMagnetManager.Initialize(this);
        imagerManager = GetComponent<ImagerManager>();
        imagerManager.Initialize(this);
        _toolStates[batteryManager] = batteryManager.toolEnabled;
        _toolStates[thrusterManager] = thrusterManager.toolEnabled;
        _toolStates[eMagnetManager] = eMagnetManager.toolEnabled;
        _toolStates[imagerManager] = imagerManager.toolEnabled;
        // ##### Object Managers ######
        //playerHealth.Initialize(this); <-- this initializes the script and creates a cross reference between the two
        audioManager = GameObject
            .FindGameObjectWithTag("AudioSources")
            .GetComponent<AudioManager>();
        audioManager.Initialize(this);
        // ##### Miscellaneous ######
        sceneTransition = GetComponent<TransitionManager>();
        sceneTransition.Initialize(this);
        deathCon = GetComponent<PlayerDeath>();
    }

    /// <summary>
    /// Runs when scene starts
    ///   - Calls base.Awake()
    ///   - Initializes script
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    /// <summary>
    /// Runs just before first frame:
    ///   - Initializes player health (Should be moved)
    /// </summary>
    public void Start()
    {   //Set up initial player health (Should be initialized through the playerhealth script instead of here)
        playerHealth.playerHealth = 5;
        playerHealth.amount = 1;
        playerHealth.UpdateSceneText();
    }

    /// <summary>
    /// Called when necessary - not every frame
    /// </summary>
    public override void UpdateController()
    {
        base.UpdateController();
    }
    
    /// <summary>
    /// Runs every frame
    ///   - Checks player status -- sets booleans
    ///   - Player movement handler
    ///   - Tool use handler
    ///   - Tool modification handler
    ///   - Handles UI                             <------- Move to event
    /// </summary>
    public void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (batteryManager.batteryDrained) {
            imagerManager.turnOff();
        } else {
            imagerManager.turnOn();
        }

        usingThruster = false; //default

        ///Disables player input if Inventory is open
        if (!inputBlocked)
        {
            //Call the requisite tool scripts here:
            //Thruster
            if (_toolStates[thrusterManager] && Input.GetButton("Jump") && batteryManager.batteryPercent != 0 && !beingPulled) {
                thrusterManager.ActivateThruster();
                usingThruster = true;
                batteryManager.DrainBatt(1);
            }

            //Spectrometer
            if (hasSpectrometer && Input.GetKeyDown(KeyCode.G) && batteryManager.batteryPercent != 0) {
                gammaView.ActivateGRS(audioManager);
                batteryManager.DrainBatt(500);
            }
            if (hasSpectrometer && Input.GetKeyUp(KeyCode.G)) {
                gammaView.DeactivateGRS(audioManager);
            }

            //ElectroMagnet
            if (_toolStates[eMagnetManager] && Input.GetButton("Fire1") && batteryManager.batteryPercent != 0 && !eMagnetActive) {
                StartCoroutine(eMagnetManager.handleEMagnet());
                batteryManager.DrainBatt(500);
            }

            playerMovement.handleMovement(usingThruster);
        }

        //Inventory and Dialog Box 
        if (Input.GetKeyDown("tab") && !Input.GetKey(KeyCode.G)) // <-- Change to getbutton and getbuttondown
            UIController.Instance.handleUI();
    }

    /// <summary>
    /// Shuts down instance when called
    ///   - Shut down base
    /// </summary>
    public override void Shutdown()
    {
        base.Shutdown();
    }

    //======================================================== Events ========================================================
    //Events definitions
    public event Action<string> OnToolPickedUp;  //When tools are picked up
    public event Action<ArrayList> OnUpdatePlayerToUI;
    public event Action<ArrayList> OnUpdatePlayerToGame;
    


    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnEnable()
    {
        //Insert logic
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        //Insert logic
    }

    //TODO: INSERT EVENT FUNCTIONS HERE


    //======================================================= Triggers =======================================================

    /// <summary>
    /// When the player is in range of a 2d collider, it will activate this function
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "TransitionObject") //for Transition objects
        {
            StartCoroutine(sceneTransition.CheckTransition(other.name));
        }
    }

    /// <summary>
    /// When the player enters the 2D collider, this function is triggered
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Element") //for picking up the elements
        {
            UIController.Instance.elementPickUp(other.name); //pick up the element
            Destroy(other.gameObject); //remove the element from the screen
        }
    }

    /// <summary>
    /// Activates tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void ToolPickUp(string toolName)
    {
        if(toolName != "Battery" || toolName != "Health")
        {
            //Update UI controller
            OnToolPickedUp?.Invoke(toolName);
        }

        //Other actions
        switch (toolName)
        {
            case "Thruster":
                thrusterManager.EnableTool();
                _toolStates[thrusterManager] = thrusterManager.toolEnabled;
                 break;

            case "Imager":
                imagerManager.Modify();
                break;

            case "ImagerCursor":
                imagerManager.EnableTool();
                _toolStates[imagerManager] = imagerManager.toolEnabled;
                imagerManager.Modify();
                flashlight.Update();
                break;

            case "Spectrometer":
                //_toolStates[spectrometerManager] ...
                hasSpectrometer = true;
                break;

            case "Magnetometer":
                eMagnetManager.EnableTool();
                _toolStates[eMagnetManager] = eMagnetManager.toolEnabled;
                break;

            case "Battery":
                Debug.Log("Battery charge!");
                batteryManager.EnableTool();
                _toolStates[batteryManager] = batteryManager.toolEnabled;
                batteryManager.ChargeBatt(500);
                break;

            case "Health":        
                Debug.Log("Health increase!");
                playerHealth.HealthUp(1);
                break;

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
