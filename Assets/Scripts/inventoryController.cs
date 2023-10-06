/** 
Description: Functions for the inventory
             Script is a component of the UI gameobject
Author: jmolive8
Version: 1
**/

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inventoryController : MonoBehaviour
{
    public GameObject inventory;
    public TMP_Text toolName;
    public TMP_Text toolDescr;

    public void QuitGame()
    {
        SceneManager.LoadScene("Title_Screen");
    }

    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            inventory.SetActive(!inventory.activeInHierarchy);
        }
    }

    public void DisplayImager()
    {
        toolName.SetText("Imager");
        toolDescr.SetText("Imager description");
    }

    public void DisplaySpectrometer()
    {
        toolName.SetText("Spectrometer");
        toolDescr.SetText("Spectrometer description");
    }

    public void DisplayMagnetometer()
    {
        toolName.SetText("Magnetometer");
        toolDescr.SetText("Magnetometer description");
    }

    public void DisplayThruster()
    {
        toolName.SetText("Thruster");
        toolDescr.SetText("Thruster description");
    }
}
