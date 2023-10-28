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
    public PlayerDeath deathCon;

    //Booleans for the various tools
    private bool batteryDrained;
    private bool hasImager;
    private bool hasMagnetometer;
    private bool hasThrusters;
    private bool hasSpectrometer;

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
        
        //Set up initial battery
        battery.batteryPercentage = 100;
        battery.rate = 1;
    }

    void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (!inputBlocked)
        {
            //Handle movement
            playerMovement.handleMovement(playerCharacter, isGrounded, audioManager);

            //Call the requisite tool scripts here:
            //Thruster
            if (hasThrusters && Input.GetButton("Jump")) {
                thruster.activateThruster(playerCharacter);
                battery.DrainBatt(1);
            }
            //Imager
            if (hasImager) {
                //Imager script call
            }
            //Spectrometer
            if (hasSpectrometer && Input.GetKeyDown(KeyCode.G)) {
                gammaView.ActivateGRS(audioManager);
                battery.DrainBatt(500);
            }
            if (hasSpectrometer && Input.GetKeyUp(KeyCode.G)) {
                gammaView.DeactivateGRS(audioManager);
            }
            //Magnetometer
            if (hasMagnetometer && !magnetActive && Input.GetButton("Fire1")) {
                StartCoroutine(magnetTool.handleMagnet(audioManager));
                battery.DrainBatt(500);
            }
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
        if (other.tag == "TransitionObject")
        {
            StartCoroutine(CheckTransition(other.name));
        }
    }

    /// <summary>
    /// Transitions the player to the respective scene if the necessary parameters are met
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumerator CheckTransition(string name)
    {
        //Polling rate
        yield return new WaitForSeconds(0.1f);

        //Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        //If a positive vertical button is pressed (w or up), then transition
        if (Input.GetButton("Vertical") && verticalAxis > 0)
        {
            if(name == "SceneTransition_Joshua")
                SceneManager.LoadScene("DeveloperScene_Joshua");
            if (name == "SceneTransition_Dhalia")
                SceneManager.LoadScene("DeveloperScene_Dhalia");
            if (name == "SceneTransition_Bryant")
                SceneManager.LoadScene("DeveloperScene_Bryant");
            if (name == "SceneTransition_Matt")
                SceneManager.LoadScene("DeveloperScene_Matt");
            if (name == "SceneTransition_James")
                SceneManager.LoadScene("DeveloperScene_James");
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

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
