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
    

    //Set up environmental checks
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    //Booleans for environmental checks
    [HideInInspector] public bool isGrounded;

    //Management scripts
    [Header("Scripts")]
    public BatteryManager batteryManager;
    public PlayerMovement playerMovement;
    public ImagerManager imagerManager;
    public ElectromagnetManager electromagnetManager;
    public ThrusterManager thrusterManager;
    public ImagerCursor flashlight;
    public GammaView gammaView;
    public AudioManager audioManager;
    private TransitionManager sceneTransition;
    private ElementManager elementManagement;
    public PlayerDeath deathCon;

    //Booleans for the various tools
    private bool hasImager; //Eliminate if necessary
    private bool hasElectromagnet;
    private bool hasThrusters;
    private bool hasSpectrometer;
    private bool usingThruster; //for animation purposes

    //Booleans to prevent needless code runs
    [HideInInspector] public bool magnetActive, inputBlocked;

    /// <summary>
    /// Awake() is the first function to be called in any script as it's being initialized
    /// </summary>
    public void Awake()
    {
        //Assign and initialize scripts
        playerCharacter = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(this);
        // ##### Tool Managers #####
        thrusterManager = GetComponent<ThrusterManager>();
        thrusterManager.Initialize(this);
        batteryManager = GetComponent<BatteryManager>();
        batteryManager.Initialize(this);
        electromagnetManager = GetComponent<ElectromagnetManager>();
        electromagnetManager.Initialize(this);
        imagerManager = GetComponent<ImagerManager>();
        imagerManager.Initialize(this);
        // ##### Object Managers ######
        elementManagement = GetComponent<ElementManager>();
        elementManagement.Initialize(this);
        // ##### Miscellaneous ######
        sceneTransition = GetComponent<TransitionManager>();
        sceneTransition.Initialize(this);
        deathCon = GetComponent<PlayerDeath>();
        audioManager = GameObject
            .FindGameObjectWithTag("AudioSources")
            .GetComponent<AudioManager>();

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
    {
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

        //Handle movement
        if (!inputBlocked)
        {
            playerMovement.handleMovement(usingThruster);
        }

        //Call the requisite tool scripts here:
        //Thruster
        if (hasThrusters && Input.GetButton("Jump") && batteryManager.batteryPercent != 0) {
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
        //Magnetometer
        if (hasElectromagnet && !magnetActive && Input.GetButton("Fire1") && batteryManager.batteryPercent != 0) {
            StartCoroutine(electromagnetManager.handleMagnet(audioManager));
            batteryManager.DrainBatt(500);
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
        if (hasElectromagnet && Input.GetButtonDown("Electromagnet_Increase"))
        {
            elementManagement.ModifyTool(electromagnetManager); //Button 3
        }
        if (hasImager && Input.GetButtonDown("Imager_Increase")) 
        {
            elementManagement.ModifyTool(imagerManager); //Button 4
        }


        //Inventory and Dialog Box
        if (Input.GetKeyDown("tab"))
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
                imager.increaseVision(audioManager);
                battery.DrainBatt(500);
                break;

            case "ImagerCursor":
                //hasImager = true; //eliminate warnings until used
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
                hasElectromagnet = true;
                UIController.Instance.setDialogText("This is a Magnetometer");
                UIController.Instance.enableMagnetometerButton();
                break;

            case "Battery":
                Debug.Log("Battery charge!");
                batteryManager.ChargeBatt(500);
                break;    

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
