/** 
Description: player health script
Author: blopezro
Version: 20231101
**/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// PlayerHealth class in which health can be tracked, increased, and decreased. 
/// </summary>
public class PlayerHealth : MonoBehaviour {
    public int playerHealth;
    public int amount;
    public bool playerHealthZero;
    public TMP_Text healthText;

    //private PlayerManagement _playerManagement;

    //Should initialize the script here and then run playerHealth.Initialize(this); in PlayerManagement
    /* 
    public void Initialize(PlayerManagement _playerManagement)
    {
         _playerManagement = playerManagement;
    }*/

    // player health down by amount
    public void HealthDown(int amount) {
        playerHealth -= amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 5); // keeps between 0-5
        if (playerHealth == 0) {
            playerHealthZero = true;
        }
        UpdateSceneText();
        //Debug.Log("Player Health: "+playerHealth);
    }

    // player health up by amount
    public void HealthUp(int amount) {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 5); // keeps between 0-5
        if (playerHealth == 0) {
            playerHealthZero = true;
        }
        UpdateSceneText();
        //Debug.Log("Player Health: "+playerHealth);
    }

    // update text for scene
    public void UpdateSceneText() {
        healthText.text = playerHealth.ToString();
    }

}
