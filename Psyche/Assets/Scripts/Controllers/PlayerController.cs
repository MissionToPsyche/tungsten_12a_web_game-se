/*
 * Description: Player Character
 * Authors: joshbenn, blopezro, mcmyers4, jmolive8, dnguye99asu
 * Version: 20240206
 */

using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

/// <summary>
/// Player Management script controls how the player interacts with the 
/// system and various components.
/// </summary>
public class PlayerController : BaseController<PlayerController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //Create the playercharacter assignment
    [Header("Components")]
    public Rigidbody2D playerCharacter;
    public BoxCollider2D playerCollider;
    public GameObject pressUpPopup;

    //Set up environmental checks
    public Transform groundCheck;
    public Vector2 groundCheckSize;
    public LayerMask whatIsGround;
    public float groundCastDistance;

    //Booleans for environmental checks
    [HideInInspector] public bool isGrounded;

    //Management scripts
    [Header("Scripts")]
    public PlayerCollisionManager playerCollisionManager;
    public PlayerHealth playerHealth;
    public SolarArrayManager solarArrayManager;
    public PlayerMovement playerMovement;
    public ImagerManager imagerManager;
    public EMagnetManager eMagnetManager;
    public ThrusterManager thrusterManager;
    public GammaView gammaView;
    public PlayerDeath playerDeath;
    public InventoryManager inventoryManager;

    //Booleans for the various tools
    private bool usingThruster; //for animation purposes  // <--Implement boolean in thruster script

    //Booleans to prevent needless code runs
    [HideInInspector] public bool eMagnetActive, toolInterrupt, beingPulled, inputBlocked, beingWarped, enteringCave, exitingCave; //Create a dictionary or list to track these

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        //Initialize base
        base.Initialize();

        
        //Assign and initialize scripts
        playerCharacter = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Initialize(this);
        playerCollisionManager = GetComponent<PlayerCollisionManager>();
        playerCollisionManager.Initialize(this);
        // ##### Managers #####
        solarArrayManager = GetComponent<SolarArrayManager>();
        solarArrayManager.Initialize(this);
        thrusterManager = GetComponent<ThrusterManager>();
        thrusterManager.Initialize(this);
        eMagnetManager = GetComponent<EMagnetManager>();
        eMagnetManager.Initialize(this);
        imagerManager = GetComponent<ImagerManager>();
        imagerManager.Initialize(this);
        // ##### Object Managers ######

        // ##### Miscellaneous ######
        inventoryManager = GetComponent<InventoryManager>();
        inventoryManager.Initialize(this);
        playerDeath = GetComponent<PlayerDeath>();
        playerDeath.Initialize(this);
        pressUpPopup = transform.Find("Press Up Popup").gameObject;

        // Tell GameController to LoadPlayer() after everything's initialized
        GameController.Instance.LoadPlayer();
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
        playerHealth.UpdateScene();
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
        isGrounded = checkGrounded();

        //Max cap for fall rate
        float maxFallVelocity = -10.0f;
        if (playerCharacter.velocity.y < maxFallVelocity) {
            playerCharacter.velocity = new Vector2(playerCharacter.velocity.x, maxFallVelocity);
        }

        //default states
        usingThruster = false;

        ///Disables player input if Inventory is open
        if (!inputBlocked)
        {
            //Call the requisite tool scripts here:
            //Thruster
            if (inventoryManager.CheckTool("thruster") && Input.GetButton("Jump") && solarArrayManager.BatteryPercent != 0 && !beingPulled && !toolInterrupt) {
                thrusterManager.Activate();
                usingThruster = true;
                solarArrayManager.DrainBatt(1);
            }

            //Imager
            if (inventoryManager.CheckTool("imager") && solarArrayManager.BatteryPercent != 0) {
                //flashlight functionality
                imagerManager.UpdateFlashlightPosition();
            }

            //Spectrometer
            if (inventoryManager.CheckTool("spectrometer") && Input.GetButton("FireGRNS") && solarArrayManager.BatteryPercent != 0) {
                gammaView.ActivateGRNS();
                solarArrayManager.DrainBatt(2);
            }

            //ElectroMagnet
            if (inventoryManager.CheckTool("electromagnet") && Input.GetButton("EMagnet") && solarArrayManager.BatteryPercent != 0 && !eMagnetActive && !toolInterrupt) {
                eMagnetManager.Activate();
            }

            //Passive Battery
            if (!Input.GetButton("Jump") && !Input.GetButton("FireGRNS") && !Input.GetButton("EMagnet") && solarArrayManager.BatteryPercent != 100 && !eMagnetActive && !beingPulled && !usingThruster)
            {
                solarArrayManager.PassiveBatt();
            }
        }

        playerMovement.HandleMovement(usingThruster, beingWarped, enteringCave, exitingCave);

        //Inventory and Dialog Box 
        if (Input.GetButtonDown("Inventory") && !Input.GetButton("FireGRNS")) { UIController.Instance.handleUI(); }
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
    
    // Events definitions
    // ...

    void ModifyTool(ArrayList args)
    {
        string toolName = args[0].ToString();
        switch (toolName.ToLower())
        {
            case "solararray":
                solarArrayManager.Modify();
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
    }


    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnEnable()
    {
        UIController.Instance.OnUpdateToolModify += ModifyTool;
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if(GameController.Instance != null)
        {
            GameController.Instance.GameStateManager.UnloadPlayer();
        }
        if(UIController.Instance != null)
        {
 
        }
    }


    //======================================================= Triggers =======================================================



    /// <summary>
    /// Creates a cast from the groundcheck box object, attached to the player, pointing down.
    /// If it detects a ground object, then it returns true. If not, then false.
    /// </summary>
    /// <returns></returns>
    public bool checkGrounded()
    {
        if (Physics2D.BoxCast(groundCheck.position, groundCheckSize, 0, -groundCheck.up, groundCastDistance, whatIsGround))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Disables EMagnet for a bit when the player gets damaged
    /// </summary>
    public IEnumerator interruptTools()
    {
        toolInterrupt = true;
        yield return new WaitForSeconds(1);
        toolInterrupt = false;
    }

    /// <summary>
    /// For debugging purposes. This draws a box to show where the ground check is/how far the cast is.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position - groundCheck.up * groundCastDistance, groundCheckSize);
    }
}