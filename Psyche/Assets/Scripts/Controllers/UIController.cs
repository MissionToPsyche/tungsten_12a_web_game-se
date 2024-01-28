using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// Functions for the inventory and dialog box. Script is a component of the UI gameobject
/// </summary>
/// Author: jmolive8
public class UIController : BaseController<UIController>
{
    //============================================== Initialize/Updates/Destroy ==============================================

    //DevConsole
    private GameObject _devConsolePanel;
    private GameObject _devConsoleFPSPanel;
    private GameObject _devConsoleResourcePanel;
    private TMP_InputField _devConsoleInput;
    private Dictionary<string, TMP_Text> _devConsoleText;  //Stores the text editors

    //Variables
    private float _aspectRatio;
    

    /// <summary>
    /// Initialize the object and parent class
    /// </summary>
    public override void Initialize()
    {
        //Base class
        base.Initialize();

        //DevConsole Objects
        _devConsolePanel = transform.Find("DevConsolePanel").gameObject;
        _devConsoleFPSPanel = transform.Find("DevConsoleFPSPanel").gameObject;
        _devConsoleResourcePanel = transform.Find("DevConsoleResourcePanel").gameObject;
        _devConsoleInput = _devConsolePanel.transform.Find("DevConsoleInput")
            .GetComponent<TMP_InputField>();
        _devConsoleText = new Dictionary<string, TMP_Text>();
        _devConsoleText.Add("DevConsoleFPS",
            _devConsoleFPSPanel.transform.Find("DevConsoleFPS").GetComponent<TMP_Text>());
        _devConsoleText.Add("DevConsoleMenu",
            _devConsolePanel.transform.Find("DevConsoleMenu").GetComponent<TMP_Text>());
        _devConsoleText.Add("DevConsoleResourceMonitor",
            _devConsoleResourcePanel.transform.Find("DevConsoleResourceMonitor").GetComponent<TMP_Text>());
        _devConsolePanel.SetActive(false);
        _devConsoleFPSPanel.SetActive(false);
        _devConsoleResourcePanel.SetActive(false);

        //Specific to this class
        _aspectRatio = (float)Screen.width / Screen.height;               //<-- Use this to adjust UI layout/positioning

        //TODO: add child components
        //battPerText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Runs when scene starts:
    ///   - Calls parent awake
    ///   - Initializes Script
    /// </summary>
    protected override void Awake()
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
        //Insert Logic
    }

    /// <summary>
    /// Runs every frame
    ///   - Empty
    /// </summary>
    public void Update()
    {
        //MOVE TO GAMECONTROLLER WHEN NO LONGER STARTING FROM SAMPLESCENE
        if(Input.GetButtonDown("DevConsole"))  //Activate the dev Console
        {
            _devConsolePanel.SetActive(!_devConsolePanel.activeSelf); //shows panel
            PlayerController.Instance.inputBlocked = _devConsolePanel.activeSelf; //blocks movement
            _devConsoleText["DevConsoleMenu"].text = "Choices:\n" +
                "toggle [arg]\t\tExample: toggle fps  --or--  toggle resource_monitor\n" +
                "set [arg] [value]";
            _devConsoleInput.ActivateInputField();
        }

        //Grab input
        if (_devConsolePanel.activeSelf && Input.GetButtonDown("Submit"))
        {
            //Take in the commands from the input box
            ArrayList commands = new ArrayList { "Game", "DeveloperConsole", "UI" };
            commands.AddRange(_devConsoleInput.text.ToLower().Split(" "));
            //Process the command
            SendMessage(commands);
            _devConsoleInput.text = "";
            _devConsoleInput.ActivateInputField();
        }
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
    
    //Events Declarations
    public event Action<ArrayList> OnUpdateUIToGame;
    public event Action<ArrayList> OnUpdateUIToPlayer;
    public event Action<ArrayList> OnUpdateInventoryUpdate;
    public event Action<ArrayList> OnUpdateToolModify;

    /// <summary>
    /// Invokes events for this and any subclasses.
    /// - Takes in an arraylist -- Requirements:
    ///   - ArrayList[0] = destination
    ///   - ArrayList[1] = subdesination
    ///   - ArrayList[2] = source
    /// </summary>
    /// <param name="args"></param>
    public override void SendMessage(ArrayList args)
    {
        string destination = args[0].ToString();
        args.RemoveAt(0);

        //Send out events depending on the invokee
        switch (destination)
        {
            case "Game":
                OnUpdateUIToGame?.Invoke(args);
                break;
            case "Player":
                OnUpdateUIToPlayer?.Invoke(args);
                break;
            default:
                Debug.Log("Incorrect invocation -- UIController");
                break;
        }
    }

    /// <summary>
    /// Processes any event passed to this class
    /// Requirements:
    ///   - ArrayList[0] = sub-destination ('None' if Controller)
    ///   - ArrayList[1] = source
    /// </summary>
    /// <param name="args"></param>
    protected override void ReceiveMessage(ArrayList args) /////////////////////////////////////////////////////////////////////////////
    {
        string subdestination = args[0].ToString();
        args.RemoveAt(0);
        string directive;

        switch (subdestination)
        {
            case "None":
                string source = args[0].ToString();
                args.RemoveAt(0);

                switch (source)
                {
                    case "DeveloperConsole":
                        ProcessDevConsole(args);
                        break;
                    case "BatteryManager":
                        UpdateBattery(args);
                        break;
                    case "ToolManager": /////////////////////////////////////////////////////////////////////////////
                        directive = args[0].ToString();
                        args.RemoveAt(0);

                        switch(directive)
                        {
                            case "ToolInfo":
                                ToolInfoGather(args);
                                break;
                        }
                        break;
                    case "InventoryManager":
                        directive = args[0].ToString();
                        args.RemoveAt(0);
                        switch(directive)
                        {
                            case "element_update":
                                ElementUpdate(args);
                                break;
                            case "tool_update":
                                EnableToolButton(args);
                                break;
                        }
                        break;
                    default:
                        Debug.Log("Incorrect source provided: " + source  + " -- UI ProcessEvent");
                        break;
                }
                break;
            default:
                Debug.Log("Incorrect subdestination -- UI ProcessEvent");
                break;
        }
    }


    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// </summary>
    private void OnEnable()
    {
        GameController.Instance.OnUpdateGameToUI += ReceiveMessage;
        PlayerController.Instance.OnUpdatePlayerToUI += ReceiveMessage;
        PlayerController.Instance.inventoryManager.OnUpdateInventoryElement += ElementUpdate;
        PlayerController.Instance.inventoryManager.OnUpdateInventoryTool += EnableToolButton;
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnUpdatePlayerToUI -= EnableToolButton;
            //PlayerController.Instance.inventoryManager.OnUpdateInventoryElement -= //Insert correct function here
            //PlayerController.Instance.inventoryManager.OnUpdateInventoryTool -= //Insert correct function here
        }
        if (GameController.Instance != null)
        {
            GameController.Instance.OnUpdateGameToUI -= ReceiveMessage;
        }
    }

    /// <summary>
    /// When called, will enable the tool button
    /// </summary>
    /// <param name="toolName"></param>
    public void EnableToolButton(ArrayList args)
    {
        string toolName = args[0].ToString();
        bool value = (bool)args[1];
        if(toolName == "battery" || toolName == "health")
        {
            return;
        }

        switch (toolName)
        {
            case "thruster":
                setDialogText("This is a Thruster");
                thrusterButton.SetActive(true);
                thrusterLevel.transform.parent.gameObject.SetActive(true);
                break;
            case "imager":
                setDialogText("This is an Imager");
                imagerButton.SetActive(true);
                imagerLevel.transform.parent.gameObject.SetActive(true);
                break;
            case "spectrometer":
                setDialogText("This is a Spectrometer\n\n Press G Key to activate");
                spectrometerButton.SetActive(true);
                break;
            case "electromagnet":
                setDialogText("This is an ElectroMagnet");
                eMagnetButton.SetActive(true);
                eMagnetLevel.transform.parent.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("Incorrect tool name passed: " + args[0].ToString());
                break;
        }
    }

    /// <summary>
    /// Function to update the battery text     ///TODO: Relocate battpertext to UIController object
    /// </summary>
    /// <param name="newPercentage"></param>
    private void UpdateBattery(ArrayList args)
    {
        battPerText.text = args[0].ToString() + "%";
        switch (float.Parse(args[0].ToString())) {
            case > 83.35f:
                activeBattSpriteRenderer.sprite = batterySprite6; break;
            case > 66.68f:
                activeBattSpriteRenderer.sprite = batterySprite5; break;
            case > 50.01f:                
                activeBattSpriteRenderer.sprite = batterySprite4; break;
            case > 33.34f:                
                activeBattSpriteRenderer.sprite = batterySprite3; break;
            case > 16.67f:                
                activeBattSpriteRenderer.sprite = batterySprite2; break;
            case > 0f:                
                activeBattSpriteRenderer.sprite = batterySprite1; break;
            case <= 0f:            
                activeBattSpriteRenderer.sprite = batterySprite0; break;
        }
    }

    /// <summary>
    /// Processes all information passed to the Dev UI
    /// </summary>
    /// <param name="args"></param>
    private void ProcessDevConsole(ArrayList args)
    {
        switch (args[0].ToString())
        {
            case "update":
                if (_devConsoleFPSPanel.activeSelf)
                {
                    _devConsoleText["DevConsoleFPS"].text = "FPS: " + args[1].ToString();
                }
                if(_devConsoleResourcePanel.activeSelf)
                {
                    _devConsoleText["DevConsoleResourceMonitor"].text = "CPU: " + args[2].ToString() + "%\n" +
                                                                        "RAM: " + args[3].ToString() + "MB";
                }
                break;
            case "toggle":
                switch(args[1].ToString())
                {
                    case "fps":
                        _devConsoleFPSPanel.SetActive(!_devConsoleFPSPanel.activeSelf);
                        _devConsoleText["DevConsoleFPS"].text = "FPS";
                        break;
                    case "resource_monitor":
                        
                        _devConsoleResourcePanel.SetActive(!_devConsoleResourcePanel.activeSelf);
                        _devConsoleText["DevConsoleResourceMonitor"].text = "Loading...";
                        break;
                    default:
                        Debug.Log("Invalid command");
                        break;
                }
                break;
            default:
                Debug.Log("Unable to process args - UI");
                break;
        }
    }

    /// <summary>
    /// Opens the link to the nasa information page when the tool image is clicked in the UI.
    ///   - TODO: UPDATE EMAGNET AND DECOUPLE FROM MAGNETOMETER
    /// </summary>
    /// <param name="toolName"></param>
    public void OpenToolLink(string toolName)
    {
        switch (toolName)
        {
            case "Spectrometer1":
                Application.OpenURL("https://psyche.asu.edu/gallery/meet-the-nasa-psyche-team-who-will-map-psyches-elemental-composition/");
                break;
            case "Spectrometer2":
                Application.OpenURL("https://www.jpl.nasa.gov/images/pia24892-psyches-gamma-ray-and-neutron-spectrometer-up-close");
                break;    
            case "Imager":
                Application.OpenURL("https://www.jpl.nasa.gov/images/pia24894-psyches-imager-in-progress");
                break;
            case "Thruster":
                Application.OpenURL("https://psyche.asu.edu/2018/01/19/electric-thrusters-psyche-spacecraft-work/");
                break;
            case "EMagnet":
                Application.OpenURL("https://psyche.asu.edu/gallery/meet-nasas-psyche-team-who-will-measure-the-asteroids-magnetic-field/");
                break;
            default:
                Debug.LogError("Tool link not set or incorrect name passed");
                break;

        }
    }

    /// <summary>
    /// Plays audio for button click
    /// </summary>
    public void playButtonSound()
    {
        GameController.Instance.audioManager.buttonClick.Play();
    }



    //========================================================== UI ==========================================================
    //UI Components
    public TMP_Text battPerText;
    public SpriteRenderer activeBattSpriteRenderer;
    public Sprite batterySprite6;
    public Sprite batterySprite5;
    public Sprite batterySprite4;
    public Sprite batterySprite3;
    public Sprite batterySprite2;
    public Sprite batterySprite1;
    public Sprite batterySprite0;

    [Header("Inventory Menus")]
    public GameObject inventoryMenu;
    public TMP_Text confirmBoxText;
    public GameObject optionsMenu;

    [Header("Dialog Box")]
    public TMP_Text dialogText;

    [Header("Buttons")]
    public GameObject imagerButton;
    public GameObject spectrometerButton;
    public GameObject eMagnetButton;
    public GameObject thrusterButton;

    //Variables
    private GameObject curSubmenu;

    /// <summary>
    /// Closes Dialog Box if it is open. If not opens Inventory
    /// </summary>
    public void handleUI()
    {
        if (dialogText.transform.parent.gameObject.activeInHierarchy)
            dialogText.transform.parent.gameObject.SetActive(false);
        else
        {
            if (curSubmenu != null)
                closeSubmenu();
            else if (inventoryMenu.activeInHierarchy)
            {
                ///enables and disables player collider to deal with being unable to go through doors after opening inventory
                PlayerController.Instance.playerCollider.enabled = false;
                setInventory(false);
                PlayerController.Instance.playerCollider.enabled = true;
            }
            else
                setInventory(true);
        }
    }

    /// <summary>
    /// Opens/closes the Inventory
    /// </summary>
    /// <param name="setActive"></param>
    public void setInventory(bool setActive)
    {
        PlayerController.Instance.inputBlocked = setActive;
        Cursor.visible = setActive;
        inventoryMenu.SetActive(setActive);
    }

    /// <summary>
    /// Opens another menu and hides the Inventory
    /// </summary>
    /// <param name="menu"></param>
    public void openSubmenu(GameObject menu)
    {
        //Play button click sound
        playButtonSound();
        curSubmenu = menu;
        curSubmenu.SetActive(true);
        inventoryMenu.SetActive(false);
    }

    /// <summary>
    /// Closes currently open submenu and returns to Inventory
    /// </summary>
    public void closeSubmenu()
    {
        curSubmenu.SetActive(false);
        curSubmenu = null;
        inventoryMenu.SetActive(true);
    }

    /// <summary>
    /// Sets text of Dialog Box and opens it
    /// </summary>
    /// <param name="text"></param>
    public void setDialogText(string text)
    {
        dialogText.SetText(text);
        dialogText.transform.parent.gameObject.SetActive(true);
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
            confirmBoxText.SetText("Are you sure you want return to the last checkpoint?");
        else
            confirmBoxText.SetText("Are you sure you want to quit to the title screen?");

        openSubmenu(confirmBoxText.transform.parent.gameObject);
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
            inventoryMenu.SetActive(false);
            PlayerController.Instance.inputBlocked = false;
            StartCoroutine(PlayerController.Instance.deathCon.Warp());
        }
        ///If Title Screen button opened the Confirmation Box
        else
            EndGame(false);
    }

    /// <summary>
    /// Destroys Player and UI so they do not spawn on the start screen or cutscene
    /// </summary>
    /// <param name="playCutscene"></param>
    public void EndGame(bool playCutscene)
    {
        Destroy(PlayerController.Instance.gameObject);
        Destroy(gameObject);
        Cursor.visible = true;
        if (playCutscene)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Outro_Cutscene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Screen");
    }




    //========================================================== Upgrades Menu ==========================================================
    [Header("Upgrades Menu")]
    public TMP_Text errorText;

    [Header("Element Amounts")]
    public TMP_Text copperAmount;
    public TMP_Text ironAmount;
    public TMP_Text nickelAmount;
    public TMP_Text goldAmount;
    public TMP_Text platinumAmount;

    [Header("Tool Levels")]
    public TMP_Text thrusterLevel;
    public TMP_Text eMagnetLevel;
    public TMP_Text batteryLevel;
    public TMP_Text imagerLevel;

    [Header("Element Requirements")]
    public TMP_Text thrusterRequirements;
    public TMP_Text eMagnetRequirements;
    public TMP_Text batteryRequirements;
    public TMP_Text imagerRequirements;

    /// <summary>
    /// Modifies the tool when its upgrade button is pressed
    /// </summary>
    public void UpgradeInterface(string toolName)
    {
        ArrayList args = new ArrayList { toolName };
        //Send the message
        OnUpdateToolModify(args);
    }

    /// <summary>
    /// Gathers the info and is called when info is being passed or if an upgrade is being requested
    ///     - Check ToolManager for when this is called but any of the specific toolmanagers (like battery)
    ///       if you want to see what their respective dictionaries of level requirements contains
    ///     - Dictionary contains a list of elements and their required values
    /// </summary>
    /// <param name="args"></param>
    public void ToolInfoGather(ArrayList args) /////////////////////////////////////////////////////////////////////////////
    {
        //passes upgrade or info
        string directive = args[0].ToString().ToLower();
        //Whether the upgrade was successful
        bool upgradeSuccess = (bool)args[1];
        //Toolname
        string toolName = args[2].ToString().ToLower();
        //Dictionary that contains everything required for the tool's next level requirements - see any toolmanager
        Dictionary<string, int> levelRequirements = (Dictionary<string, int>)args[3];
        string level = args[4].ToString();

        switch (directive)
        {
            case "upgrade":
                if (!upgradeSuccess)
                {
                    string requirement_display = "Must Have: ";
                    foreach (var requirement in levelRequirements)
                    {
                        if (requirement.Value > 0)
                        {
                            requirement_display += requirement.Key + " " + requirement.Value + ",";
                        }
                    }
                    errorText.SetText(requirement_display);
                    errorText.gameObject.SetActive(true);
                    return;
                }

                errorText.gameObject.SetActive(false);
                switch (toolName.ToLower()) //rework to show all element requirements?
                {
                    case "thruster":
                        thrusterLevel.SetText(level);
                        thrusterRequirements.SetText(levelRequirements["element_copper"].ToString());
                        break;
                    case "battery":
                        batteryLevel.SetText(level);
                        batteryRequirements.SetText(levelRequirements["element_nickel"].ToString());
                        break;
                    case "imager":
                        imagerLevel.SetText(level);
                        imagerRequirements.SetText(levelRequirements["element_gold"].ToString());
                        break;
                    case "electromagnet":
                        eMagnetLevel.SetText(level);
                        eMagnetRequirements.SetText(levelRequirements["element_iron"].ToString());
                        break;
                    default:
                        break;
                }
                break;
            case "info":
                switch (toolName.ToLower())
                {
                    case "thruster":
                        errorText.gameObject.SetActive(false);
                        break;
                    case "battery":

                        errorText.gameObject.SetActive(false);
                        break;
                    case "imager":

                        errorText.gameObject.SetActive(false);
                        break;
                    case "electromagnet":
                        errorText.gameObject.SetActive(false);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// When the player walks over the object, pick up the object
    /// </summary>
    public void ElementUpdate(ArrayList args)
    {
        string element = args[0].ToString().ToLower();
        string value = args[1].ToString();

        switch (element)
        {
            case "element_copper":
                copperAmount.SetText(value);
                break;
            case "element_iron":
                ironAmount.SetText(value);
                break;
            case "element_nickel":
                nickelAmount.SetText(value);
                break;
            case "element_gold":
                goldAmount.SetText(value);
                break;
            case "element_platinum":
                platinumAmount.SetText(value);
                break;
            default:
                Debug.Log("Element type not implemented");
                break;
        }
    }
}
