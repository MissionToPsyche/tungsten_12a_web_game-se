using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
