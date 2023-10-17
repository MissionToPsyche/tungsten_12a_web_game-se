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
    private PlayerMovement playerMovement;
    private PlayerMagnetometer magnetTool;

    //Booleans for the various tools
    private bool hasThrusters;
    private bool hasImager;
    private bool hasMagnetometer = true; //temp assignment
    private bool hasSpectrometer;

    //Booleans to prevent needless code runs
    public bool magnetActive;

    // Start is called before the first frame update
    void Start()
    {
        //Assign the playerCharacter to its in-game object
        playerCharacter = GetComponent<Rigidbody2D>(); 
        playerMovement = GetComponent<PlayerMovement>();
        magnetTool = GetComponent<PlayerMagnetometer>();
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
        if (hasMagnetometer && !magnetActive && Input.GetButton("Fire1"))
            StartCoroutine(magnetTool.handleMagnet());
    }
}
