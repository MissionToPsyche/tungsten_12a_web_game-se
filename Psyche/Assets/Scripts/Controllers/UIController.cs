using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        base.UpdateController();
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
            EventInvocation(commands);
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

    /// <summary>
    /// Invokes events for this and any subclasses.
    /// - Takes in an arraylist -- Requirements:
    ///   - ArrayList[0] = destination
    ///   - ArrayList[1] = source
    /// </summary>
    /// <param name="args"></param>
    public override void EventInvocation(ArrayList args)
    {
        string destination = args[0].ToString();
        args.RemoveAt(0);
        base.EventInvocation(args);  //even necessary?

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
    /// Subscribes to events and activates when event triggered
    /// </summary>
    private void OnEnable()
    {
        GameController.Instance.OnUpdateGameToUI += ProcessEvent;
        PlayerController.Instance.OnToolPickedUp += EnableToolButton; //To be removed
        PlayerController.Instance.batteryManager.onBatteryPercentageChanged += UpdateBatteryText; //To be removed
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnToolPickedUp -= EnableToolButton; 
            PlayerController.Instance.batteryManager.onBatteryPercentageChanged -= UpdateBatteryText;
        }
        if(GameController.Instance != null)
        {
            GameController.Instance.OnUpdateGameToUI -= ProcessEvent;
        }
    }

    /// <summary>
    /// Processes any event passed to this class
    /// Requirements:
    ///   - ArrayList[0] = sub-destination ('None' if Controller)
    ///   - ArrayList[1] = source
    /// </summary>
    /// <param name="args"></param>
    private void ProcessEvent(ArrayList args)
    {
        string subdestination = args[0].ToString(); //Unused, but for future cases
        string source = args[1].ToString();
        args.RemoveAt(0);
        args.RemoveAt(0);

        switch(subdestination)
        {
            case "None":
                switch (source)
                {
                    case "DeveloperConsole":
                        ProcessDevConsole(args);
                        break;
                    case "BatteryMangaer":
                        //Implement
                        break;
                    default:
                        Debug.Log("Incorrect source provided -- UI ProcessEvent");
                        break;
                }
                break;
            default:
                Debug.Log("Incorrect subdestination -- UI ProcessEvent");
                break;
        }

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
                thrusterSection.SetActive(true);
                break;
            case "ImagerCursor":
                setDialogText("This is an Imager");
                imagerButton.SetActive(true);
                imagerSection.SetActive(true);
                break;
            case "Spectrometer":
                setDialogText("This is a Spectrometer");
                spectrometerButton.SetActive(true);
                break;
            case "Magnetometer":
                setDialogText("This is an ElectroMagnet");
                eMagnetButton.SetActive(true);
                eMagnetSection.SetActive(true);
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

    /// <summary>
    /// Processes all information passed to the Dev UI
    /// </summary>
    /// <param name="args"></param>
    private void ProcessDevConsole(ArrayList args)
    {

        switch (args[0].ToString())
        {
            case "update":
                if(_devConsoleFPSPanel.activeSelf)
                {
                    _devConsoleText["DevConsoleFPS"].text = "FPS: " + args[1].ToString();
                }       
                break;
            case "toggle":
                switch(args[1])
                {
                    case "fps":
                        _devConsoleFPSPanel.SetActive(!_devConsoleFPSPanel.activeSelf);
                        _devConsoleText["DevConsoleFPS"].text = "FPS";
                        break;
                    case "resource_monitor":
                        _devConsoleResourcePanel.SetActive(_devConsoleResourcePanel.activeSelf);
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
        switch (toolName)
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

    /// <summary>
    /// Function to update the battery text     ///TODO: Relocate battpertext to UIController object
    /// </summary>
    /// <param name="newPercentage"></param>
    private void UpdateBatteryText(float newPercentage)
    {
        battPerText.text = Mathf.RoundToInt(newPercentage).ToString() + "%";
    }

    /// <summary>
    /// Plays audio for button click
    /// </summary>
    public void playButtonSound()
    {
        PlayerController.Instance.audioManager.PlayAudio(PlayerController.Instance.audioManager.buttonClick);
    }





    //========================================================== UI ==========================================================
    //UI Components
    public TMP_Text battPerText;

    [Header("Inventory Menus")]
    public GameObject inventoryMenu;
    public TMP_Text confirmBoxText;
    public GameObject optionsMenu;

    [Header("Dialog Box")]
    public GameObject dialogBox;
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
        if (dialogBox.activeInHierarchy)
            dialogBox.SetActive(false);
        else
        {
            if (curSubmenu != null)
                closeSubmenu();
            else if (inventoryMenu.activeInHierarchy)
                setInventory(false);
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
        dialogBox.SetActive(true);
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
            PlayerController.Instance.deathCon.GetHurt(); //use different function
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
            SceneManager.LoadScene("Outro_Cutscene");
        else
            SceneManager.LoadScene("Title_Screen");
    }




    //========================================================== Upgrades Menu ==========================================================
    [Header("Upgrades Menu")]
    public TMP_Text copperAmount;
    public TMP_Text ironAmount;
    public TMP_Text nickelAmount;
    public TMP_Text goldAmount;
    public TMP_Text platinumAmount;
    public TMP_Text errorText;
    public GameObject thrusterSection;
    public GameObject eMagnetSection;
    public GameObject imagerSection;

    private int _copper, _iron, _nickel, _gold, _platinum;

    /// <summary>
    /// Modifies the tool when its upgrade button is pressed
    /// </summary>
    public void upgradeTool(string toolName)
    {
        if (toolName == "Thruster")
        {
            if (_copper > 0)
            {
                _copper--;
                copperAmount.SetText(_copper.ToString());
                errorText.gameObject.SetActive(false);
                PlayerController.Instance.thrusterManager.Modify();
            }
            else
            {
                errorText.gameObject.SetActive(true);
                errorText.SetText("Not enough copper!");
            }
        }
        else if (toolName == "Electromagnet")
        {
            if (_iron > 0)
            {
                _iron--;
                ironAmount.SetText(_iron.ToString());
                errorText.gameObject.SetActive(false);
                PlayerController.Instance.eMagnetManager.Modify();
            }
            else
            {
                errorText.gameObject.SetActive(true);
                errorText.SetText("Not enough iron!");
            }
        }
        else if (toolName == "Battery")
        {
            if (_nickel > 0)
            {
                _nickel--;
                nickelAmount.SetText(_nickel.ToString());
                errorText.gameObject.SetActive(false);
                PlayerController.Instance.batteryManager.Modify();
            }
            else
            {
                errorText.gameObject.SetActive(true);
                errorText.SetText("Not enough nickel!");
            }
        }
        else if (toolName == "Imager")
        {
            if (_gold > 0)
            {
                _gold--;
                goldAmount.SetText(_gold.ToString());
                errorText.gameObject.SetActive(false);
                PlayerController.Instance.imagerManager.Modify();
            }
            else
            {
                errorText.gameObject.SetActive(true);
                errorText.SetText("Not enough gold!");
            }
        }
    }

    /// <summary>
    /// When the player walks over the object, pick up the object
    /// </summary>
    public void elementPickUp(string element)
    {
        switch (element)
        {
            case "Element_Copper":
                _copper++;
                copperAmount.SetText(_copper.ToString());
                break;
            case "Element_Iron":
                _iron++;
                ironAmount.SetText(_iron.ToString());
                break;
            case "Element_Nickel":
                _nickel++;
                nickelAmount.SetText(_nickel.ToString());
                break;
            case "Element_Gold":
                _gold++;
                goldAmount.SetText(_gold.ToString());
                break;
            case "Element_Platinum":
                _platinum++;
                platinumAmount.SetText(_platinum.ToString());
                break;
            default:
                Debug.Log("Element type not implemented");
                break;
        }
    }
}
