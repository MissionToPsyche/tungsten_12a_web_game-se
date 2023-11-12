using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functions for the inventory and dialog box. Script is a component of the UI gameobject
/// </summary>
/// Author: jmolive8
public class UIController : BaseController<UIController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //UI Components
    public GameObject inventoryBox;
    public GameObject dialogBox;
    public TMP_Text dialogText;
    public TMP_Text confirmBoxText;
    public TMP_Text battPerText;

    [Header("Buttons")]
    public GameObject imagerButton;
    public GameObject spectrometerButton;
    public GameObject eMagnetButton;
    public GameObject thrusterButton;

    //Variables

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        //TODO: add child components
        //battPerText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Runs when scene starts:
    ///   - Calls parent awake
    ///   - Initializes Script
    /// </summary>
    void Awake()
    {
        base.Awake();
        Initialize();
    }

    /// <summary>
    /// Runs just before first frame:
    ///   - Empty
    /// </summary>
    public void Start()
    {
        //Insert logic
    }

    /// <summary>
    /// Called when necessary - not every frame
    /// </summary>
    public override void UpdateController()
    {
        base.UpdateController();
    }

    /// <summary>
    /// Runs every frame
    ///   - Empty
    /// </summary>
    public void Update()
    {
        //Insert Logic
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
    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// </summary>
    private void OnEnable()
    {
        PlayerController.Instance.OnToolPickedUp += EnableToolButton;
        PlayerController.Instance.batteryManager.onBatteryPercentageChanged += UpdateBatteryText;
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if(PlayerController.Instance != null)
        {
            PlayerController.Instance.OnToolPickedUp -= EnableToolButton;
        }
        PlayerController.Instance.batteryManager.onBatteryPercentageChanged -= UpdateBatteryText;
        
    }

    /// <summary>
    /// When called, will enable the tool button
    /// </summary>
    /// <param name="toolName"></param>
    public void EnableToolButton(string toolName)
    {
        switch (toolName)
        {
            case "Thruster":
                setDialogText("This is a Thruster");
                thrusterButton.SetActive(true);
                break;
            case "ImagerCursor":
                setDialogText("This is an Imager");
                imagerButton.SetActive(true);
                break;
            case "Spectrometer":
                setDialogText("This is a Spectrometer");
                spectrometerButton.SetActive(true);
                break;
            case "Magnetometer":
                setDialogText("This is an ElectroMagnet");
                eMagnetButton.SetActive(true);
                break;
            default:
                Debug.LogWarning("Incorrect tool name passed: " + toolName);
                break;
        }
    }

    /// <summary>
    /// Function to update the battery text     ///TODO: Relocate battpertext to UIController object
    /// </summary>
    /// <param name="newPercentage"></param>
    private void UpdateBatteryText(float newPercentage)
    {
        battPerText.text = Mathf.RoundToInt(newPercentage).ToString() + "%";
    }

    //========================================================== UI ==========================================================



    /// <summary>
    /// Closes Dialog Box if it is open. If not opens Inventory
    /// </summary>
    public void handleUI()
    {
        if (dialogBox.activeInHierarchy)
            dialogBox.SetActive(false);
        else
        {
            bool invToggle = !inventoryBox.activeInHierarchy;
            PlayerController.Instance.inputBlocked = invToggle;
            Cursor.visible = invToggle;
            inventoryBox.SetActive(invToggle);
        }
    }

    /// <summary>
    /// Sets text of Dialog Box and opens it
    /// </summary>
    /// <param name="text"></param>
    public void setDialogText(string text)
    {
        dialogText.SetText(text);
        dialogBox.SetActive(true);
    }



    /// <summary>
    /// Opens the link to the nasa information page when the tool image is clicked in the UI.
    ///   - TODO: UPDATE EMAGNET AND DECOUPLE FROM MAGNETOMETER
    /// </summary>
    /// <param name="toolName"></param>
    public void OpenToolLink(string toolName)
    {
        switch(toolName)
        {
            case "Imager":
                Application.OpenURL("https://www.jpl.nasa.gov/images/pia24894-psyches-imager-in-progress");
                break;
            case "EMagnet":
                Application.OpenURL("https://psyche.asu.edu/gallery/meet-nasas-psyche-team-who-will-measure-the-asteroids-magnetic-field/");
                break;
            default:
                Debug.LogError("Tool link not set or incorrect name passed");
                break;

        }
    }

    private bool shouldRespawn;
    /// <summary>
    /// When Respawn/Title Screen button is clicked activates Confirmation Box and sets behavior for Yes button
    /// </summary>
    /// <param name="respawn"></param>
    public void handleConfirmBox(bool respawn)
    {
        //Play button click sound
        playButtonSound();

        shouldRespawn = respawn;
        if (shouldRespawn)
        {
            confirmBoxText.SetText("Are you sure you want return to the last checkpoint?");
            confirmBoxText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            confirmBoxText.SetText("Are you sure you want to quit to the title screen?");
            confirmBoxText.transform.parent.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Handles behavior when Yes button is clicked on Confirmation Box
    /// </summary>
    public void handleYesClicked()
    {
        //Play button click sound
        playButtonSound();
        
        ///If Respawn button opened the Confirmation Box
        if (shouldRespawn)
        {
            confirmBoxText.transform.parent.gameObject.SetActive(false);
            inventoryBox.SetActive(false);
            PlayerController.Instance.inputBlocked = false;
            PlayerController.Instance.deathCon.GetHurt();
        }
        ///If Title Screen button opened the Confirmation Box
        else
        {
            ///Destroys Player and UI so they do not spawn on the start screen
            Destroy(PlayerController.Instance.gameObject);
            Destroy(gameObject);
            Cursor.visible = true;
            SceneManager.LoadScene("Title_Screen");
        }
    }

    /// <summary>
    /// Plays audio for button click
    /// </summary>
    public void playButtonSound()
    {
        PlayerController.Instance.audioManager.PlayAudio(PlayerController.Instance.audioManager.buttonClick);
    }
}
