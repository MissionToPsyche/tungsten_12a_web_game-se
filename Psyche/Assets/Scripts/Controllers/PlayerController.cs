using UnityEngine;
using System.Collections;
using System;

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
    private TransitionManager sceneTransition;
    public PlayerDeath deathCon;
    public InventoryManager inventoryManager;

    //Booleans for the various tools
    private bool usingThruster; //for animation purposes  // <--Implement boolean in thruster script

    //Booleans to prevent needless code runs
    [HideInInspector] public bool eMagnetActive, beingPulled, inputBlocked, beingWarped; //Create a dictionary or list to track these

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        //Initialize base
        base.Initialize();

        //TEMPORARY <-----------------------------------------REMOVE WHEN NO LONGER NECESSARY
        gameController = FindAnyObjectByType<GameController>();

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
        // ##### Object Managers ######
        //playerHealth.Initialize(this); <-- this initializes the script and creates a cross reference between the two
        // ##### Miscellaneous ######
        sceneTransition = GetComponent<TransitionManager>();
        sceneTransition.Initialize(this);
        deathCon = GetComponent<PlayerDeath>();
        inventoryManager = GetComponent<InventoryManager>();
        inventoryManager.Initialize(this);
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
        //Insert Logic
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
        imagerManager.Activate();

        //default states
        usingThruster = false;

        ///Disables player input if Inventory is open
        if (!inputBlocked)
        {
            //Call the requisite tool scripts here:
            //Thruster
            if (inventoryManager.CheckTool("thruster") && Input.GetButton("Jump") && batteryManager.batteryPercent != 0 && !beingPulled) {
                thrusterManager.Activate();
                usingThruster = true;
                batteryManager.DrainBatt(1);
            }

            //Spectrometer
            if (inventoryManager.CheckTool("spectrometer") && Input.GetButtonDown("FireGRS") && batteryManager.batteryPercent != 0) {
                gammaView.ActivateGRS();
                batteryManager.DrainBatt(500);
            }
            if (inventoryManager.CheckTool("spectrometer") && Input.GetButtonUp("FireGRS")) {
                gammaView.DeactivateGRS();
            }

            //ElectroMagnet
            if (inventoryManager.CheckTool("electromagnet") && Input.GetButton("EMagnet") && batteryManager.batteryPercent != 0 && !eMagnetActive) {
                eMagnetManager.Activate();
                batteryManager.DrainBatt(500);
            }
        }

        playerMovement.handleMovement(usingThruster, beingWarped);

        // //needed to ensure the warping animation plays even when input is blocked
        // if (beingWarped)
        //     playerMovement.handleMovement(usingThruster, beingWarped);

        //Inventory and Dialog Box 
        if (Input.GetButtonDown("Inventory") && !Input.GetButton("FireGRS"))
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
    public event Action<ArrayList> OnUpdatePlayerToUI;
    public event Action<ArrayList> OnUpdatePlayerToGame;

    /// <summary>
    /// Invokes events for this and any subclasses.
    /// - Takes in an arraylist -- Requirements:
    ///   - ArrayList[0] = destination
    ///   - ArrayList[1] = sub-destination ('None' if Controller)
    ///   - ArrayList[2] = source
    /// </summary>
    /// <param name="args"></param>
    public override void SendMessage(ArrayList args)
    {
        string destination = args[0].ToString();
        args.RemoveAt(0);

        //Send out events depending on the invokee
        switch (destination)
        {
            case "UI":
                OnUpdatePlayerToUI?.Invoke(args);
                break;
            case "Player":
                OnUpdatePlayerToUI.Invoke(args);
                break;
            default:
                Debug.Log("Incorrect invocation in GameController");
                break;
        }
    }

    /// <summary>
    /// Processes any event passed to this class
    /// Requirements:
    ///   - ArrayList[0] = sub-destination ('None' if Controller)
    ///   - ArrayList[1] = source
    /// </summary>
    /// <param name="args"></param>
    protected override void ReceiveMessage(ArrayList args)
    {
        string subdestination = args[0].ToString();
        args.RemoveAt(0);

        switch (subdestination)
        {
            case "None":
                string source = args[0].ToString();
                args.RemoveAt(0);
                
                switch(source)
                {
                    case "UI":
                        string directive = args[0].ToString();
                        args.RemoveAt(0);
                        
                        switch(directive)
                        {
                            case "tool_upgrade":
                                string toolName = args[0].ToString();
                                args.RemoveAt(0);

                                switch(toolName.ToLower())
                                {
                                    case "battery":
                                        batteryManager.Modify();
                                        break;
                                    case "thruster":
                                        thrusterManager.Modify();
                                        break;
                                    case "electromagnet":
                                        eMagnetManager.Modify();
                                        break;
                                    case "imager":
                                        imagerManager.Modify();
                                        break;
                                }
                                break;
                        }
                        break;
                    case "DeveloperConsole":
                        string item = args[0].ToString();
                        //Set up for whether tool/element is passed
                        break;
                }
                break;
            case "InventoryManager":
                inventoryManager.ReceiveMessage(args);
                break;
            default:
                Debug.Log("Incorrect subdestination -- PlayerController");
                break;
        }
    }


    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnEnable()
    {
        GameController.Instance.OnUpdateGameToPlayer += ReceiveMessage;
        UIController.Instance.OnUpdateUIToPlayer += ReceiveMessage;
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if(GameController.Instance != null)
        {
            GameController.Instance.OnUpdateGameToPlayer -= ReceiveMessage;
        }
        if(UIController.Instance != null)
        {
            UIController.Instance.OnUpdateUIToPlayer -= ReceiveMessage;
        }
    }

    //TODO: INSERT EVENT FUNCTIONS HERE


    //======================================================= Triggers =======================================================

    /// <summary>
    /// When the player is in range of a 2d collider, it will activate this function
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "TransitionObject" || other.tag ==  "TransitionObjectIn") //for Transition objects
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
            inventoryManager.AddElement(other.name, 1);
            Destroy(other.gameObject); //remove the element from the screen
        }
        /*
        if (other.tag == "Tool")
        {
            ToolPickUp(other.name);
            Destroy(other.gameObject);
        }*/
    }

    /// <summary>
    /// Activates tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void ToolPickUp(string toolName)
    {
        //Other actions
        switch (toolName)
        {
            case "Thruster":
                thrusterManager.Enable();
                inventoryManager.SetTool(toolName, true);
                break;

            case "Imager":
                imagerManager.Modify();
                inventoryManager.SetTool(toolName, true);
                break;

            case "ImagerCursor":
                imagerManager.Enable();
                imagerManager.Modify();
                flashlight.Update();
                break;

            case "Spectrometer":
                inventoryManager.SetTool(toolName, true);
                break;

            case "ElectroMagnet":
                eMagnetManager.Enable();
                inventoryManager.SetTool(toolName, true);
                break;

            case "Battery":
                batteryManager.Enable();
                inventoryManager.SetTool(toolName, true);
                batteryManager.ChargeBatt(500);
                break;

            case "Health":
                playerHealth.HealthUp(1);
                break;

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
