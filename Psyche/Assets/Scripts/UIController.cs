using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functions for the inventory and dialog box. Script is a component of the UI gameobject
/// </summary>
/// Author: jmolive8
public class UIController : MonoBehaviour
{
    public bool boolTest = false;

    [HideInInspector] public static UIController Instance;

    public GameObject inventoryScreen;
    public TMP_Text confirmBoxText;

    [Header("Dialog Box")]
    public GameObject dialogBox;
    public TMP_Text dialogText;

    [Header("Options Screen")]
    public GameObject optionsScreen;

    [Header("Buttons")]
    public GameObject imagerButton;
    public GameObject spectrometerButton;
    public GameObject magnetometerButton;
    public GameObject thrusterButton;

    private GameObject submenu;

    /// <summary>
    /// When transitioning between scenes, ensures UI state remains
    /// </summary>
    void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerManagement.Instance.batteryManager.onBatteryPercentageChanged += UpdateBatteryText;
    }

    private void OnDisable()
    {
        if (PlayerManagement.Instance.batteryManager != null)
            PlayerManagement.Instance.batteryManager.onBatteryPercentageChanged -= UpdateBatteryText;
    }

    /// <summary>
    /// Function to update the battery text
    /// </summary>
    /// <param name="newPercentage"></param>
    private void UpdateBatteryText(float newPercentage)
    {
        Debug.Log("Battery ---: " + newPercentage);
        // Assuming battPerText is the text component that shows the battery percentage
        PlayerManagement.Instance.batteryManager.battPerText.text = Mathf.RoundToInt(newPercentage).ToString() + "%";
    }

    /// <summary>
    /// Closes Dialog Box if it is open. If not opens Inventory
    /// </summary>
    public void handleUI()
    {
        if (dialogBox.activeInHierarchy)
            dialogBox.SetActive(false);
        else
        {
            if (submenu != null)
                closeSubmenu();
            else if (inventoryScreen.activeInHierarchy)
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
        PlayerManagement.Instance.inputBlocked = setActive;
        Cursor.visible = setActive;
        inventoryScreen.SetActive(setActive);
    }

    /// <summary>
    /// Opens another menu and hides the Inventory
    /// </summary>
    /// <param name="menu"></param>
    private void openSubmenu(GameObject menu)
    {
        submenu = menu;
        submenu.SetActive(true);
        inventoryScreen.SetActive(false);
    }

    /// <summary>
    /// Closes currently open submenu and returns to Inventory
    /// </summary>
    public void closeSubmenu()
    {
        submenu.SetActive(false);
        submenu = null;
        inventoryScreen.SetActive(true);
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

    public void enableImagerButton() { imagerButton.SetActive(true); }
    public void enableSpectrometerButton() { spectrometerButton.SetActive(true); }
    public void enableMagnetometerButton() { magnetometerButton.SetActive(true); }
    public void enableThrusterButton() { thrusterButton.SetActive(true); }

    public void openImagerLink() { Application.OpenURL("https://www.jpl.nasa.gov/images/pia24894-psyches-imager-in-progress"); }
    public void openMagnetometerLink() { Application.OpenURL("https://psyche.asu.edu/gallery/meet-nasas-psyche-team-who-will-measure-the-asteroids-magnetic-field/"); }

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
            inventoryScreen.SetActive(false);
            PlayerManagement.Instance.inputBlocked = false;
            PlayerManagement.Instance.deathCon.GetHurt();
        }
        ///If Title Screen button opened the Confirmation Box
        else
        {
            ///Destroys Player and UI so they do not spawn on the start screen
            Destroy(PlayerManagement.Instance.gameObject);
            Destroy(gameObject);
            Cursor.visible = true;
            SceneManager.LoadScene("Title_Screen");
        }
    }

    /// <summary>
    /// Opens option screen and syncs color blind toggle
    /// </summary>
    public void openOptionsScreen()
    {
        openSubmenu(optionsScreen);
    }

    /// <summary>
    /// Plays audio for button click
    /// </summary>
    public void playButtonSound()
    {
        PlayerManagement.Instance.audioManager.PlayAudio(PlayerManagement.Instance.audioManager.buttonClick);
    }
}
