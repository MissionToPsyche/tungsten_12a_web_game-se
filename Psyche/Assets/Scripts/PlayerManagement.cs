using UnityEngine;

/// <summary>
/// Player Management script controls how the player interacts with the system and various components.
/// </summary>
public class PlayerManagement : MonoBehaviour
{
    //Create the playercharacter assignment
    public Rigidbody2D playerCharacter;

    //Set up environmental checks
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    //Booleans for environmental checks
    public bool isGrounded;

    //Management scripts
    PlayerMovement playerMovement;
    public Imager imager;
    public DialogueController dialBox;
    public InventoryController inventory;

    //Booleans for the various tools
    private bool hasThrusters;
    private bool hasImager;
    private bool hasMagnetometer;
    private bool hasSpectrometer;

    // Start is called before the first frame update
    void Start()
    {
        //Assign the playerCharacter to its in-game object
        playerCharacter = GetComponent<Rigidbody2D>(); 
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check booleans
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        //Handle movement
        playerMovement.handleMovement(playerCharacter, isGrounded);
        
        //Call the requisite tool scripts here:
        //Thruster
        if(hasThrusters && !isGrounded && Input.GetButton("Jump"))
            return; //Thruster script call
        //Imager
        if (hasImager)
            return; //Imager script call
        //Spectrometer
        if (hasSpectrometer)
            return; //Spectrometer script call
        //Magnometer
        if (hasMagnetometer)
            return; //Magnometer script call
    }

    /// <summary>
    /// Activates tool when its pickup is collected
    /// </summary>
    /// <param name="toolName"></param>
    public void toolPickedUp(string toolName) //close dialogue on tab and make sure inv doesn't open
    {
        switch (toolName)
        {
            case "Thruster":
                hasThrusters = true;
                dialBox.SetText("This is a Thruster");
                break;

            case "Imager":
                hasImager = true;
                dialBox.SetText("This is an Imager");
                imager.increaseVision();
                break;

            case "Spectrometer":
                hasImager = true;
                dialBox.SetText("This is a Spectrometer");
                break;

            case "Magnetometer":
                hasMagnetometer = true;
                dialBox.SetText("This is a Magnetometer");
                break;

            default:
                Debug.LogWarning("Tool name '" + toolName + "' not found!");
                break;
        }
    }
}
