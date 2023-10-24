using UnityEngine;

/// <summary>
/// Player Management script controls how the player interacts with the system and various components.
/// </summary>
public class PlayerManagement : MonoBehaviour
{
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
    public PlayerMovement playerMovement;
    public UIController UICon; //handle with static/instance variable?
    public Imager imager;
    public Magnetometer magnetTool;
    private Thruster thruster;
    public GammaView gammaView;
    private AudioManager audioManager;

    //Booleans for the various tools
    private bool hasImager;
    private bool hasMagnetometer;
    private bool hasThrusters;
    private bool hasSpectrometer;

    //Booleans to prevent needless code runs
    [HideInInspector] public bool magnetActive;
    // Start is called before the first frame update
    void Start()
    {
        //Assign the playerCharacter to it in-game objects
        playerCharacter = GetComponent<Rigidbody2D>(); 
        playerMovement = GetComponent<PlayerMovement>();
        thruster = GetComponent<Thruster>();
        audioManager = GameObject
            .FindGameObjectWithTag("AudioSources")
            .GetComponent<AudioManager>();
        
        //Testing purposes
        hasThrusters = true;
    }

    void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //Handle movement
        playerMovement.handleMovement(playerCharacter, isGrounded, audioManager);

        //Call the requisite tool scripts here:
        //Thruster
        if (hasThrusters)
            thruster.activateThruster(playerCharacter);
        //Imager
        if (hasImager)
            //Imager script call
        //Spectrometer
        if (hasSpectrometer && Input.GetKeyDown(KeyCode.G))
            gammaView.ActivateGRS(audioManager);
        if (hasSpectrometer && Input.GetKeyUp(KeyCode.G))
            gammaView.DeactivateGRS(audioManager);
        //Magnetometer
        if (hasMagnetometer && !magnetActive && Input.GetButton("Fire1"))
            StartCoroutine(magnetTool.handleMagnet(audioManager));

        //Inventory and Dialogue Box
        if (Input.GetKeyDown("tab"))
            UICon.handleUI();
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
                UICon.setDialogueText("This is a Thruster");
                UICon.enableThrusterButton();
                break;

            case "Imager":
                hasImager = true;
                UICon.setDialogueText("This is an Imager");
                UICon.enableImagerButton();
                imager.increaseVision(audioManager);
                break;

            case "Spectrometer":
                hasSpectrometer = true;
                UICon.setDialogueText("This is a Spectrometer");
                UICon.enableSpectrometerButton();
                break;

            case "Magnetometer":
                hasMagnetometer = true;
                UICon.setDialogueText("This is a Magnetometer");
                UICon.enableMagnetometerButton();
                break;

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
