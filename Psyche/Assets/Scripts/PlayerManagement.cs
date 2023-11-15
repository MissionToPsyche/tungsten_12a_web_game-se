// Ignore Spelling: Collider

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Player Management script controls how the player interacts with the 
/// system and various components.
/// </summary>
public class PlayerManagement : MonoBehaviour
{
    [HideInInspector] public static PlayerManagement Instance; //For persistence through scenes

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
    private ElementManager elementManagement;
    public PlayerDeath deathCon;

    //Booleans for the various tools
    private bool hasImager; //Eliminate if necessary
    private bool hasEMagnet;
    private bool hasThrusters;
    private bool hasSpectrometer;
    private bool usingThruster; //for animation purposes

    //Booleans to prevent needless code runs
    [HideInInspector] public bool eMagnetActive, beingPulled, inputBlocked;

    /// <summary>
    /// Awake() is the first function to be called in any script as it's being initialized
    /// </summary>
    public void Awake()
    {
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
        elementManagement = GetComponent<ElementManager>();
        elementManagement.Initialize(this);
        audioManager = GameObject
            .FindGameObjectWithTag("AudioSources")
            .GetComponent<AudioManager>();
        // ##### Miscellaneous ######
        sceneTransition = GetComponent<TransitionManager>();
        sceneTransition.Initialize(this);
        deathCon = GetComponent<PlayerDeath>();
        

        //Use singleton to ensure no duplicates are created
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start() is called just before the first frame of the scene
    /// </summary>
    void Start()
    {   //Set up initial player health (Should be initialized through the playerhealth script instead of here)
        playerHealth.playerHealth = 5;
        playerHealth.amount = 1;
        playerHealth.UpdateSceneText();

        //Don't destroy audio manager (should be removed)
        DontDestroyOnLoad(audioManager);
    }

    void Update()
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
            if (hasThrusters && Input.GetButton("Jump") && batteryManager.batteryPercent != 0 && !beingPulled) {
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
            if (hasEMagnet && Input.GetButton("Fire1") && batteryManager.batteryPercent != 0 && !eMagnetActive) { //better bool management
                StartCoroutine(eMagnetManager.handleEMagnet());
                batteryManager.DrainBatt(500);
            }

            playerMovement.handleMovement(usingThruster);
        }

        //Modify the tools
        if (hasThrusters && Input.GetButtonDown("Thruster_Increase")) //Button 1
        {
            elementManagement.ModifyTool(thrusterManager);
        }
        if (Input.GetButtonDown("Battery_Increase")) //Button 2
        {
            elementManagement.ModifyTool(batteryManager);
        }
        if (hasEMagnet && Input.GetButtonDown("Electromagnet_Increase"))
        {
            elementManagement.ModifyTool(eMagnetManager); //Button 3
        }
        if (hasImager && Input.GetButtonDown("Imager_Increase")) 
        {
            elementManagement.ModifyTool(imagerManager); //Button 4
        }


        //Inventory and Dialog Box
        if (Input.GetKeyDown("tab") && !Input.GetKey(KeyCode.G))
            UIController.Instance.handleUI();
    }

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
            elementManagement.ElementPickUp(other.name); //pick up the element
            Destroy(other.gameObject); //remove the element from the screen
        }
    }

    /// <summary>
    /// Activates tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void toolPickedUp(string toolName)
    {
        switch (toolName)
        {
            case "Thruster":
                hasThrusters = true;
                UIController.Instance.setDialogText("This is a Thruster");
                UIController.Instance.enableThrusterButton();
                break;

            case "Imager":
                //hasImager = true;
                imagerManager.Modify();
                batteryManager.DrainBatt(500);
                break;

            case "ImagerCursor":
                hasImager = true; //eliminate warnings until used
                UIController.Instance.setDialogText("This is an Imager");
                UIController.Instance.enableImagerButton();
                imagerManager.Modify();
                batteryManager.DrainBatt(500);
                flashlight.Update();
                batteryManager.DrainBatt(500);
                break;

            case "Spectrometer":
                hasSpectrometer = true;
                UIController.Instance.setDialogText("This is a Spectrometer");
                UIController.Instance.enableSpectrometerButton();
                break;

            case "Magnetometer":
                hasEMagnet = true;
                UIController.Instance.setDialogText("This is a Magnetometer");
                UIController.Instance.enableMagnetometerButton();
                break;

            case "Battery":
                //Debug.Log("Battery charge!");
                batteryManager.ChargeBatt(500);
                break;

            case "Health":        
                //Debug.Log("Health increase!");
                playerHealth.HealthUp(1);
                break;

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
