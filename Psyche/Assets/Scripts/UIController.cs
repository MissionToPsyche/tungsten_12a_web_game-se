using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functions for the inventory and dialog box. Script is a component of the UI gameobject
/// </summary>
/// Author: jmolive8
public class UIController : MonoBehaviour
{
    [HideInInspector] public static UIController Instance;

    /// <summary>
    /// When transitioning between scenes, ensures UI state remains
    /// </summary>
    void Awake()
    {
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

    public GameObject inventoryBox;
    public GameObject dialogBox;
    public TMP_Text dialogText;
    public TMP_Text confirmBoxText;

    [Header("Buttons")]
    public GameObject imagerButton;
    public GameObject spectrometerButton;
    public GameObject magnetometerButton;
    public GameObject thrusterButton;

    /// <summary>
    /// Closes Dialog Box if it is open. If not opens Inventory
    /// </summary>
    public void handleUI()
    {
        if (dialogBox.activeInHierarchy)
            dialogBox.SetActive(false);
        else
        {
            Cursor.visible = !inventoryBox.activeInHierarchy;
            inventoryBox.SetActive(!inventoryBox.activeInHierarchy);
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

    public void enableImagerButton() { imagerButton.SetActive(true); }
    public void enableSpectrometerButton() { spectrometerButton.SetActive(true); }
    public void enableMagnetometerButton() { magnetometerButton.SetActive(true); }
    public void enableThrusterButton() { thrusterButton.SetActive(true); }

    /// <summary>
    /// Respawn button functionality. 
    /// </summary>
    public void checkpointWarp()
    {
        inventoryBox.SetActive(false);
        PlayerManagement.Instance.deathCon.Die();
    }

    /// <summary>
    /// Title Screen button functionality. Destroys Player and UI so they don't spawn on the title screen
    /// </summary>
    public void quitGame()
    {
        Destroy(PlayerManagement.Instance.gameObject);
        Destroy(gameObject);
        Cursor.visible = true;
        SceneManager.LoadScene("Title_Screen");
    }

    private bool shouldRespawn;

    /// <summary>
    /// When Respawn/Title Screen button is clicked activates Confirmation Box and sets behavior for Yes button
    /// </summary>
    /// <param name="respawn"></param>
    public void handleConfirmBox(bool respawn)
    {
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
        ///If Respawn button opened the Confirmation Box
        if (shouldRespawn)
        {
            confirmBoxText.transform.parent.gameObject.SetActive(false);
            inventoryBox.SetActive(false);
            PlayerManagement.Instance.deathCon.Die();
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
}
