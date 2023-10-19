using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Functions for the inventory and dialogue box. Script is a component of the UI gameobject
/// </summary>
/// Author: jmolive8
public class UIController : MonoBehaviour
{
    public GameObject inventoryBox;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    [Header("Buttons")]
    public GameObject imagerButton;
    public GameObject spectrometerButton;
    public GameObject magnetometerButton;
    public GameObject thrusterButton;

    /// <summary>
    /// Closes Dialogue Box if it is open. If not opens Inventory
    /// </summary>
    public void handleUI()
    {
        if (dialogueBox.activeInHierarchy)
            dialogueBox.SetActive(false);
        else
            inventoryBox.SetActive(!inventoryBox.activeInHierarchy);
    }

    /// <summary>
    /// Sets text of Dialogue Box and opens it
    /// </summary>
    /// <param name="text"></param>
    public void setDialogueText(string text)
    {
        dialogueText.SetText(text);
        dialogueBox.SetActive(true);
    }

    public void activateImagerButton() { imagerButton.SetActive(true); }
    public void activateSpectrometerButton() { spectrometerButton.SetActive(true); }
    public void activateMagnetometerButton() { magnetometerButton.SetActive(true); }
    public void activateThrusterButton() { thrusterButton.SetActive(true); }

    /// <summary>
    /// Title Screen button functionality
    /// </summary>
    public void QuitGame()
    {
        SceneManager.LoadScene("Title_Screen");
    }
}
