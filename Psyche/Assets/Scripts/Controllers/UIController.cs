using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ToolManager;

/// <summary>
/// Functions for the inventory and dialog box. Script is a component of the UI gameobject
/// </summary>
/// Author: jmolive8, blopezro
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
        _devConsoleInput = _devConsolePanel.transform.Find("DevConsoleInput").GetComponent<TMP_InputField>();
        _devConsoleText = new Dictionary<string, TMP_Text>();
        _devConsoleText.Add("DevConsoleFPS", _devConsoleFPSPanel.transform
            .Find("DevConsoleFPS").GetComponent<TMP_Text>());
        _devConsoleText.Add("DevConsoleMenu", _devConsolePanel.transform
            .Find("DevConsoleMenu").GetComponent<TMP_Text>());
        _devConsoleText.Add("DevConsoleResourceMonitor", _devConsoleResourcePanel.transform
            .Find("DevConsoleResourceMonitor").GetComponent<TMP_Text>());
        _devConsolePanel.SetActive(false);
        _devConsoleFPSPanel.SetActive(false);
        _devConsoleResourcePanel.SetActive(false);

        // Temporary workaround until UI issues fixed -- Tell GameController to load the UI event and DevConsole script
        GameController.Instance.LoadUI();

        //Specific to this class
        _aspectRatio = (float)Screen.width / Screen.height; //<-- Use this to adjust UI layout/positioning

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
        if (Input.GetButtonDown("DevConsole"))  //Activate the dev Console
        {
            _devConsolePanel.SetActive(!_devConsolePanel.activeSelf); //shows panel
            PlayerController.Instance.inputBlocked = _devConsolePanel.activeSelf; //blocks movement
            _devConsoleText["DevConsoleMenu"].text = "Choices:\n" +
                "toggle <arg>\n\tExample: toggle fps  -or-  toggle resource_monitor\n" +
                "set <type> <arg> [value]\n\tExample: set scene thruster end -or- set element copper 30";
            _devConsoleInput.ActivateInputField();
        }

        //Grab input
        if (_devConsolePanel.activeSelf && Input.GetButtonDown("Submit"))
        {
            //Take in the commands from the input box
            ArrayList commands = new ArrayList();
            commands.AddRange(_devConsoleInput.text.ToLower().Split(" "));
            //Process the command
            OnUpdateUIToDevConsole?.Invoke(commands);
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
    public event Action<ArrayList> OnUpdateToolModify;      // Send Tool Modification requests to Inventory

    public event Action<ArrayList> OnUpdateUIToDevConsole;  // Send dev input commands to DevConsole

    /// <summary>
    /// Subscribes to events and activates when event triggered
    /// </summary>
    private void OnEnable()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.developerConsole.OnDevConsoleUIUpdate += ProcessDevConsole;
        }

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.inventoryManager.OnUpdateInventoryElement += ElementUpdate;
            PlayerController.Instance.inventoryManager.OnUpdateInventoryTool += EnableToolButton;
            PlayerController.Instance.solarArrayManager.OnBatteryPercentageChanged += UpdateBattery;

            // ToolInfoGathers
            PlayerController.Instance.solarArrayManager.ToolManagerUpdate += ToolInfoGather;
            PlayerController.Instance.thrusterManager.ToolManagerUpdate += ToolInfoGather;
            PlayerController.Instance.imagerManager.ToolManagerUpdate += ToolInfoGather;
            PlayerController.Instance.eMagnetManager.ToolManagerUpdate += ToolInfoGather;
        }
    }

    /// <summary>
    /// When event call is no longer active, turns off function
    ///   - TODO!! ENABLE EVENT COMMUNICATION FOR INSTANCE CALLS
    /// </summary>
    private void OnDisable()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.developerConsole.OnDevConsoleUIUpdate -= ProcessDevConsole;
        }
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.inventoryManager.OnUpdateInventoryElement -= ElementUpdate;
            PlayerController.Instance.inventoryManager.OnUpdateInventoryTool -= EnableToolButton;
            PlayerController.Instance.solarArrayManager.OnBatteryPercentageChanged -= UpdateBattery;

            // ToolInfoGathers
            PlayerController.Instance.solarArrayManager.ToolManagerUpdate -= ToolInfoGather;
            PlayerController.Instance.thrusterManager.ToolManagerUpdate -= ToolInfoGather;
            PlayerController.Instance.imagerManager.ToolManagerUpdate -= ToolInfoGather;
            PlayerController.Instance.eMagnetManager.ToolManagerUpdate -= ToolInfoGather;
        }
    }

    /// <summary>
    /// When called, will enable the tool button
    /// </summary>
    /// <param name="toolName"></param>
    public void EnableToolButton(InventoryManager.Tool tool, bool value)
    {
        switch (tool)
        {
            case InventoryManager.Tool.SOLARARRAY:
                setDialogText("This is a Solar Array. Psyche had 2 of these in cross formations to generate power.\n\nI can use this to automatically recharge my suit's battery!\n\n<i>Press <b>TAB</b> to learn more.</i>");
                solarArrayInfoButton.SetActive(true);
                solarArrayLevel.transform.parent.gameObject.SetActive(true);
                batteryIndicator.SetActive(true);
                break;

            case InventoryManager.Tool.THRUSTER:
                setDialogText("This is a Hall Thruster. This thruster used electricity to propel Psyche from Earth's orbit to the asteroid.\n\nI can use this to help me reach high up areas!\n\n<i>Hold <b>SPACEBAR</b> to fly upwards.</i>\n\n<i>Press <b>TAB</b> to learn more.</i>");
                thrusterInfoButton.SetActive(true);
                thrusterLevel.transform.parent.gameObject.SetActive(true);
                thrusterIcon.SetActive(true);
                break;

            case InventoryManager.Tool.IMAGER:
                setDialogText("This is Multispectral Imager. It is highly sensitive to light and Psyche used 2 of these to analyze the asteroid's geology and topography.\n\nI can use this to help me see in the dark!\n\n<i>Look around with the spotlight using your mouse.</i>\n\n<i>Press <b>TAB</b> to learn more.</i>");
                imagerInfoButton.SetActive(true);
                imagerLevel.transform.parent.gameObject.SetActive(true);
                break;

            case InventoryManager.Tool.SPECTROMETER:
                setDialogText("This is a Gamma-Ray and Neutron Spectrometer. This system was used on Psyche to map the elemental composition of the asteroid.\n\nI can use this to help me search for the elements I need!\n\n<i>Hold <b>RIGHT CLICK</b> to see through objects.</i>\n\n<i>Press <b>TAB</b> to learn more.</i>");
                spectrometerInfoButton.SetActive(true);
                GRNSIcon.SetActive(true);
                break;

            case InventoryManager.Tool.ELECTROMAGNET:
                setDialogText("This is a Magnetometer. Psyche used 2 of these to detect the asteroid's remanent (leftover) magnetic field.\n\nI can use this to detect surface materials that have remanent magnetism! With my suit's Electro-Magnet, I'll propel myself towards magnetized deposits.\n\n<i>Hold <b>LEFT CLICK</b> to aim the Electro-Magnet with your mouse and pull yourself towards magnetized deposits.</i>\n\n<i>Press <b>TAB</b> to learn more.</i>");
                eMagnetInfoButton.SetActive(true);
                eMagnetLevel.transform.parent.gameObject.SetActive(true);
                eMagnetIcon.SetActive(true);
                break;

            default:
                Debug.Log("Incorrect tool name passed: " + tool.ToString());
                break;
        }
    }

    /// <summary>
    /// Function to update the battery text     ///TODO: Relocate battpertext to UIController object
    /// </summary>
    /// <param name="newPercentage"></param>
    private void UpdateBattery(float percent)
    {
        battPerText.text = percent.ToString() + "%";
        switch (percent)
        {
            case > 83.35f: activeBattSpriteRenderer.sprite = batterySprite6; break;
            case > 66.68f: activeBattSpriteRenderer.sprite = batterySprite5; break;
            case > 50.01f: activeBattSpriteRenderer.sprite = batterySprite4; break;
            case > 33.34f: activeBattSpriteRenderer.sprite = batterySprite3; break;
            case > 16.67f: activeBattSpriteRenderer.sprite = batterySprite2; break;
            case > 0f: activeBattSpriteRenderer.sprite = batterySprite1; break;
            case <= 0f: activeBattSpriteRenderer.sprite = batterySprite0; break;
        }
    }

    public void UpdateCapturedTungstens(GameStateManager.Scene currentScene)
    {
        // Determine which element to update based on the current scene
        int elementIndex = -1;
        switch (currentScene)
        {
            case GameStateManager.Scene.Landing: elementIndex = 0; break;
            case GameStateManager.Scene.Imager: elementIndex = 1; break;
            case GameStateManager.Scene.GRNS: elementIndex = 2; break;
            case GameStateManager.Scene.eMagnet: elementIndex = 3; break;
            case GameStateManager.Scene.Thruster: elementIndex = 4; break;
            case GameStateManager.Scene.Combo1: elementIndex = 5; break;
            case GameStateManager.Scene.Combo2: elementIndex = 6; break;
            case GameStateManager.Scene.Combo3: elementIndex = 7; break;
        }

        // If elementIndex is valid, update the corresponding element's alpha to "unghost"
        if (elementIndex >= 0 && elementIndex < elements.Length)
        {
            Color newColor = elements[elementIndex].color;
            newColor.a = 1f;
            elements[elementIndex].color = newColor;
            GameController.Instance.audioManager.pickupElementTungsten.Play();
        }
    }

    /// <summary>
    /// Processes all information passed to the Dev UI
    /// </summary>
    /// <param name="args"></param>
    private void ProcessDevConsole(ArrayList args)
    {
        var command = GameController.Instance.developerConsole.Match(args[0].ToString());

        switch (command)
        {
            case DeveloperConsole.DevConsoleCommand.UPDATE:
                if (_devConsoleFPSPanel.activeSelf)
                {
                    _devConsoleText["DevConsoleFPS"].text = "FPS: " + args[1].ToString();
                }
                if (_devConsoleResourcePanel.activeSelf)
                {
                    _devConsoleText["DevConsoleResourceMonitor"].text = "RAM: " + args[2].ToString() + "MB";
                }
                break;
            case DeveloperConsole.DevConsoleCommand.TOGGLE:
                var sub_command = GameController.Instance.developerConsole.Match(args[1].ToString());
                Debug.Log($"{sub_command} -- {args[1].ToString()}");
                switch (sub_command)
                {
                    case DeveloperConsole.DevConsoleCommand.FPS:
                        _devConsoleFPSPanel.SetActive(!_devConsoleFPSPanel.activeSelf);
                        _devConsoleText["DevConsoleFPS"].text = "FPS";
                        break;
                    case DeveloperConsole.DevConsoleCommand.RESOURCE_MONITOR:

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
                Application.OpenURL("https://psyche.asu.edu/gallery/meet-the-nasa-psyche-team-who-will-map-psyches-elemental-composition/"); break;
            case "Spectrometer2":
                Application.OpenURL("https://www.jpl.nasa.gov/images/pia24892-psyches-gamma-ray-and-neutron-spectrometer-up-close"); break;
            case "Imager":
                Application.OpenURL("https://www.jpl.nasa.gov/images/pia24894-psyches-imager-in-progress"); break;
            case "Thruster":
                Application.OpenURL("https://psyche.asu.edu/2018/01/19/electric-thrusters-psyche-spacecraft-work/"); break;
            case "EMagnet":
                Application.OpenURL("https://psyche.asu.edu/gallery/meet-nasas-psyche-team-who-will-measure-the-asteroids-magnetic-field/"); break;
            case "SolarArray":
                Application.OpenURL("https://psyche.asu.edu/mission/the-spacecraft/"); break;
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

    public SpriteRenderer[] elements = new SpriteRenderer[8];

    [Header("Inventory Menus")]
    public GameObject inventoryMenu;
    public TMP_Text confirmBoxText;
    public GameObject optionsMenu;

    [Header("Dialog Box")]
    public TMP_Text dialogText;

    [Header("Buttons")]
    public GameObject solarArrayInfoButton;
    public GameObject imagerInfoButton;
    public GameObject spectrometerInfoButton;
    public GameObject eMagnetInfoButton;
    public GameObject thrusterInfoButton;

    [Header("Overlay Icons")]
    public GameObject GRNSIcon;
    public GameObject eMagnetIcon;
    public GameObject thrusterIcon;
    public GameObject batteryIndicator;

    //Variables
    private GameObject curSubmenu;

    /// <summary>
    /// Closes Dialog Box if it is open. If not opens Inventory
    /// </summary>
    public void handleUI()
    {
        if (dialogText.transform.parent.gameObject.activeInHierarchy)
            setDialogText("");

        else if (curSubmenu != null)
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

    /// <summary>
    /// Sets text of Dialog Box and opens it. If text is blank it closes the box
    /// </summary>
    /// <param name="text">Set blank if you want to close the Dialog Box</param>
    public void setDialogText(string text)
    {
        if (text == "")
        {
            PlayerController.Instance.inputBlocked = false;
            dialogText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            PlayerController.Instance.inputBlocked = true;
            dialogText.SetText(text);
            dialogText.transform.parent.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Opens/closes the Inventory
    /// </summary>
    /// <param name="setActive"></param>
    public void setInventory(bool setActive)
    {
        PlayerController.Instance.inputBlocked = setActive;
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
            StartCoroutine(GameController.Instance.gameStateManager.Warp());
        }
        ///If Title Screen button opened the Confirmation Box
        else
        {
            /**
             * Destroys Player and UI so they do not spawn on the start screen
             */
            Destroy(PlayerController.Instance.gameObject);
            Destroy(gameObject);
            GameController.Instance.sceneTransitionManager.devControl = true;
            GameController.Instance.sceneTransitionManager.OnInitiateTransition(
                GameController.Instance.gameStateManager.MatchScene(GameStateManager.Scene.Title)
                );

        }
    }

    //========================================================== Upgrades Menu ==========================================================
    [Header("Upgrades Menu")]
    public TMP_Text errorText;

    [Header("Element Amounts")]
    public TMP_Text copperAmount;
    public TMP_Text ironAmount;
    public TMP_Text nickelAmount;
    public TMP_Text goldAmount;
    public TMP_Text tungstenAmount;

    [Header("Tool Levels")]
    public TMP_Text thrusterLevel;
    public TMP_Text eMagnetLevel;
    public TMP_Text solarArrayLevel;
    public TMP_Text imagerLevel;

    [Header("Element Requirements")]
    public GameObject thrusterRequirementsList;
    public GameObject eMagnetRequirementsList;
    public GameObject solarArrayRequirementsList;
    public GameObject imagerRequirementsList;

    [Header("Upgrade Buttons")]
    public Button thrusterUpgradeButton;
    public Button eMagnetUpgradeButton;
    public Button solarArrayUpgradeButton;
    public Button imagerUpgradeButton;

    [Header("Element Requirement Prefabs")]
    public GameObject copperRequirement;
    public GameObject ironRequirement;
    public GameObject nickelRequirement;
    public GameObject goldRequirement;
    public GameObject tungstenRequirement;

    /// <summary>
    /// Modifies the tool when its upgrade button is pressed
    /// </summary>
    public void UpgradeInterface(string toolName)
    {
        ArrayList args = new ArrayList { toolName };
        // Send the message
        OnUpdateToolModify?.Invoke(args);

        // Play audio
        float soundDuration = 3.0f;
        AudioSource audioSource = PlayerController.Instance.inventoryManager.MatchTool(toolName) switch
        {
            InventoryManager.Tool.IMAGER => GameController.Instance.audioManager.toolImager,
            InventoryManager.Tool.SOLARARRAY => GameController.Instance.audioManager.buttonClick,
            InventoryManager.Tool.THRUSTER => GameController.Instance.audioManager.toolThrusters,
            InventoryManager.Tool.ELECTROMAGNET => GameController.Instance.audioManager.toolEMagnet,
            _ => null,
        };
        if (audioSource != null)
        {
            StartCoroutine(FadeOutSound(audioSource, soundDuration));
            audioSource.Play();
        }
    }

    /// <summary>
    /// Fades out the sound of the audio played when upgrading a tool
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="fadeDuration"></param>
    /// <returns></returns>
    private IEnumerator FadeOutSound(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = 1.0f;
        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            // Calculate the normalized time elapsed since starting the fade
            float normalizedTime = (Time.time - startTime) / fadeDuration;
            // Calculate the new volume based on the fade curve
            audioSource.volume = Mathf.Lerp(startVolume, 0, normalizedTime);
            yield return null;
        }
        // Ensure the volume is fully faded out, stop the playback, then restore volume
        audioSource.volume = 0.0f;
        audioSource.Stop();
        audioSource.volume = 1.0f;
    }

    /// <summary>
    /// Gathers the info and is called when info is being passed or if an upgrade is being requested
    ///     - Check ToolManager for when this is called but any of the specific toolmanagers (like battery)
    ///       if you want to see what their respective dictionaries of level requirements contains
    ///     - Dictionary contains a list of elements and their required values
    /// </summary>
    /// <param name="args"></param>
    public void ToolInfoGather(ToolDirective directive, bool upgraded, string toolName, Dictionary<InventoryManager.Element, ushort> requirements, int level, int maxLevel)
    {
        switch (directive)
        {
            case ToolDirective.Upgrade:
                if (!upgraded)
                {
                    string requirement_display = "Must Have: ";
                    foreach (var requirement in requirements)
                    {
                        if (requirement.Value > 0)
                        {
                            string name = requirement.Key switch
                            {
                                InventoryManager.Element.COPPER => "Copper",
                                InventoryManager.Element.IRON => "Iron",
                                InventoryManager.Element.NICKEL => "Nickel",
                                InventoryManager.Element.GOLD => "Gold",
                                _ => ""
                            };

                            requirement_display += name + " " + requirement.Value + "  ";
                        }
                    }
                    errorText.SetText(requirement_display);
                    errorText.gameObject.SetActive(true);
                    return;
                }

                errorText.gameObject.SetActive(false);
                switch (toolName.ToLower())
                {
                    case "thruster":
                        thrusterLevel.SetText("Current Level: " + level.ToString() + " / " + maxLevel.ToString());
                        UpdateRequirements(requirements, thrusterRequirementsList.transform);
                        break;
                    case "solararray":
                        solarArrayLevel.SetText("Current Level: " + level.ToString() + " / " + maxLevel.ToString());
                        UpdateRequirements(requirements, solarArrayRequirementsList.transform);
                        break;
                    case "imager":
                        imagerLevel.SetText("Current Level: " + level.ToString() + " / " + maxLevel.ToString());
                        UpdateRequirements(requirements, imagerRequirementsList.transform);
                        break;
                    case "electromagnet":
                        eMagnetLevel.SetText("Current Level: " + level.ToString() + " / " + maxLevel.ToString());
                        UpdateRequirements(requirements, eMagnetRequirementsList.transform);
                        break;
                }
                break;

            case ToolDirective.Info:
                switch (toolName.ToLower())
                {
                    case "thruster":
                        UpdateRequirements(requirements, thrusterRequirementsList.transform);
                        break;
                    case "solararray":
                        UpdateRequirements(requirements, solarArrayRequirementsList.transform);
                        break;
                    case "imager":
                        UpdateRequirements(requirements, imagerRequirementsList.transform);
                        break;
                    case "electromagnet":
                        UpdateRequirements(requirements, eMagnetRequirementsList.transform);
                        break;
                }
                break;

            case ToolDirective.MaxLevel:
                errorText.gameObject.SetActive(false);
                switch (toolName.ToLower())
                {
                    case "thruster":
                        thrusterLevel.SetText(level.ToString());
                        thrusterUpgradeButton.interactable = false;
                        break;
                    case "solararray":
                        solarArrayLevel.SetText(level.ToString());
                        solarArrayUpgradeButton.interactable = false;
                        break;
                    case "imager":
                        imagerLevel.SetText(level.ToString());
                        imagerUpgradeButton.interactable = false;
                        break;
                    case "electromagnet":
                        eMagnetLevel.SetText(level.ToString());
                        eMagnetUpgradeButton.interactable = false;
                        break;
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Update the display for the requirements
    /// </summary>
    private void UpdateRequirements(Dictionary<InventoryManager.Element, ushort> levelRequirements, Transform requirementsArea)
    {
        foreach (Transform child in requirementsArea)
        {
            Destroy(child.gameObject);
        }

        int amount = levelRequirements[InventoryManager.Element.COPPER];
        if (amount > 0)
            Instantiate(copperRequirement, requirementsArea).GetComponentInChildren<TMP_Text>().SetText(amount.ToString());

        amount = levelRequirements[InventoryManager.Element.IRON];
        if (amount > 0)
            Instantiate(ironRequirement, requirementsArea).GetComponentInChildren<TMP_Text>().SetText(amount.ToString());

        amount = levelRequirements[InventoryManager.Element.NICKEL];
        if (amount > 0)
            Instantiate(nickelRequirement, requirementsArea).GetComponentInChildren<TMP_Text>().SetText(amount.ToString());

        amount = levelRequirements[InventoryManager.Element.GOLD];
        if (amount > 0)
            Instantiate(goldRequirement, requirementsArea).GetComponentInChildren<TMP_Text>().SetText(amount.ToString());

        /*amount = levelRequirements[];
        if (amount > 0)
            Instantiate(tungstenRequirement, requirementsArea).GetComponentInChildren<TMP_Text>().SetText(amount.ToString());*/
    }

    /// <summary>
    /// When the player walks over the object, pick up the object
    /// </summary>
    public void ElementUpdate(InventoryManager.Element element, ushort amount)
    {
        //string element = args[0].ToString().ToLower();
        string value = amount.ToString();

        switch (element)
        {
            case InventoryManager.Element.COPPER:
                copperAmount.SetText(value);
                break;
            case InventoryManager.Element.IRON:
                ironAmount.SetText(value);
                break;
            case InventoryManager.Element.NICKEL:
                nickelAmount.SetText(value);
                break;
            case InventoryManager.Element.GOLD:
                goldAmount.SetText(value);
                break;
            case InventoryManager.Element.TUNGSTEN:
                tungstenAmount.SetText(value);
                break;
            default:
                Debug.Log("Element type not implemented");
                break;
        }
    }
}
