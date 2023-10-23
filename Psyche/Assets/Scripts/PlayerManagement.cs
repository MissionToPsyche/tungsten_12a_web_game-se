using UnityEngine;

/// <summary>
/// Player Management script controls how the player interacts with the system and various components.
/// </summary>
public class PlayerManagement : MonoBehaviour
{
    //Create the playercharacter assignment
    [Header("Components")]
    public Rigidbody2D playerCharacter;
    public static PlayerManagement Instance; //For persistence through scenes

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

    //Booleans for the various tools
    private bool hasThrusters;
    private bool hasImager;
    private bool hasMagnetometer;
    private bool hasSpectrometer;

    //Booleans to prevent needless code runs
    [HideInInspector] public bool magnetActive;
    // Start is called before the first frame update
    void Start()
    {
        //Assign the playerCharacter to its in-game object
        playerCharacter = GetComponent<Rigidbody2D>(); 
        playerMovement = GetComponent<PlayerMovement>();
        thruster = GetComponent<Thruster>();
        
        //Testing purposes
        hasThrusters = true;
    }

    void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //Handle movement
        playerMovement.handleMovement(playerCharacter, isGrounded);

        //Call the requisite tool scripts here:
        //Thruster
        if (hasThrusters)
            thruster.activateThruster(playerCharacter);
        //Imager
        if (hasImager)
            {}//Imager script call
        //Spectrometer
        if (hasSpectrometer)
            {}//Spectrometer script call
        //Magnometer
        if (hasMagnetometer && !magnetActive && Input.GetButton("Fire1"))
            StartCoroutine(magnetTool.handleMagnet());

        //Inventory and Dialogue Box
        if (Input.GetKeyDown("tab"))
            UICon.handleUI();
    }

    /// <summary>
    /// When transitioning between scenes, ensures playerstate remains
    /// </summary>
    public void Awake()
    {
        //Use singleton to ensure no duplicates are created
        if(Instance == null)
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
                imager.increaseVision();
                break;

            case "Spectrometer":
                hasImager = true;
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
