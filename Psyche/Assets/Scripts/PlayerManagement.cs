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
    public Battery battery;
    public PlayerMovement playerMovement;
    public Imager imager;
    public Magnetometer magnetTool;
    private Thruster thruster;
    public GammaView gammaView;
    private AudioManager audioManager;
    private SceneTransition sceneTransition;
    private ElementManagement elementManagement;
    public PlayerDeath deathCon;

    //Booleans for the various tools
    private bool hasImager;
    private bool hasMagnetometer;
    private bool hasThrusters;
    private bool hasSpectrometer;
    private bool usingThruster; //for animation purposes

    //Booleans to prevent needless code runs
    [HideInInspector] public bool magnetActive, inputBlocked;

    /// <summary>
    /// When transitioning between scenes, ensures playerstate remains
    /// </summary>
    public void Awake()
    {
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

    // Start is called before the first frame update
    void Start()
    {
        //Assign the playerCharacter to it in-game objects
        playerCharacter = GetComponent<Rigidbody2D>(); 
        playerMovement = GetComponent<PlayerMovement>();
        thruster = GetComponent<Thruster>();
        deathCon = GetComponent<PlayerDeath>();
        audioManager = GameObject
            .FindGameObjectWithTag("AudioSources")
            .GetComponent<AudioManager>();
        sceneTransition = GetComponent<SceneTransition>();
        elementManagement = GetComponent<ElementManagement>();
        DontDestroyOnLoad(audioManager);    
        
        //Set up initial battery
        battery.batteryPercentage = 100;
        battery.rate = 1;
    }

    void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (battery.batteryDrained) {
            imager.turnOff();
        } else {
            imager.turnOn();
        }

        usingThruster = false; //default

        //Call the requisite tool scripts here:
        //Thruster
        if (hasThrusters && Input.GetButton("Jump") && battery.batteryPercentage != 0) {
            thruster.activateThruster(playerCharacter);
            usingThruster = true;
            battery.DrainBatt(1);
        }
        //Spectrometer
        if (hasSpectrometer && Input.GetKeyDown(KeyCode.G) && battery.batteryPercentage != 0) {
            gammaView.ActivateGRS(audioManager);
            battery.DrainBatt(500);
        }
        if (hasSpectrometer && Input.GetKeyUp(KeyCode.G)) {
            gammaView.DeactivateGRS(audioManager);
        }
        //Magnetometer
        if (hasMagnetometer && !magnetActive && Input.GetButton("Fire1") && battery.batteryPercentage != 0) {
            StartCoroutine(magnetTool.handleMagnet(audioManager));
            battery.DrainBatt(500);
        }

        if (!inputBlocked)
        {
            //Handle movement
            playerMovement.handleMovement(playerCharacter, isGrounded, audioManager, usingThruster);
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
                hasImager = true;
                UIController.Instance.setDialogText("This is an Imager");
                UIController.Instance.enableImagerButton();
                imager.increaseVision(audioManager);
                battery.DrainBatt(500);
                break;

            case "Spectrometer":
                hasSpectrometer = true;
                UIController.Instance.setDialogText("This is a Spectrometer");
                UIController.Instance.enableSpectrometerButton();
                break;

            case "Magnetometer":
                hasMagnetometer = true;
                UIController.Instance.setDialogText("This is a Magnetometer");
                UIController.Instance.enableMagnetometerButton();
                break;

            case "Battery":
                Debug.Log("Battery charge!");
                battery.ChargeBatt(500);
                break;    

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
