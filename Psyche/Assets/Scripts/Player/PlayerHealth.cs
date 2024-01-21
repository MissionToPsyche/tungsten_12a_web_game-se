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
    public Sprite healthSpriteNormal;
    public Sprite healthSpriteDamaged;
    public Sprite healthSpriteCritical;

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
        UpdateScene();
        //Debug.Log("Player Health: "+playerHealth);
    }

    // player health up by amount
    public void HealthUp(int amount) {
        playerHealth += amount;
        playerHealth = Mathf.Clamp(playerHealth, 0, 5); // keeps between 0-5
        if (playerHealth == 0) {
            playerHealthZero = true;
        }
        UpdateScene();
        //Debug.Log("Player Health: "+playerHealth);
    }

    // update text for scene
    public void UpdateScene() {
        healthText.text = playerHealth.ToString();
        switch (healthText.text) {
            case "5": case "4":
                gameObject.GetComponent<SpriteRenderer>().sprite = healthSpriteNormal;
                break;
            case "3": case "2":
                gameObject.GetComponent<SpriteRenderer>().sprite = healthSpriteDamaged;
                break;
            case "1": case "0":
                gameObject.GetComponent<SpriteRenderer>().sprite = healthSpriteCritical;
                break;    
        }
    }

}
