/** 
Description: Player health script
Author: blopezro
Version: 20240410
**/

using TMPro;
using UnityEngine;

/// <summary>
/// PlayerHealth class in which health can be tracked, increased, and decreased. 
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public int Health;
    public bool PlayerHealthZero;
    public TMP_Text HealthText;
    public Sprite HealthSpriteNormal;
    public Sprite HealthSpriteDamaged;
    public Sprite HealthSpriteCritical;

    /// <summary>
    /// player health down by amount
    /// </summary>
    /// <param name="Amount"></param>
    public void HealthDown(int Amount)
    {
        Health -= Amount;
        Health = Mathf.Clamp(Health, 0, 5); // keeps between 0-5
        if (Health == 0)
        {
            PlayerHealthZero = true;
        }
        UpdateScene();
    }

    /// <summary>
    /// player health up by amount
    /// </summary>
    /// <param name="Amount"></param>
    public void HealthUp(int Amount)
    {
        Health += Amount;
        Health = Mathf.Clamp(Health, 0, 5); // keeps between 0-5
        if (Health == 0)
        {
            PlayerHealthZero = true;
        }
        UpdateScene();
    }

    /// <summary>
    /// update text for scene
    /// </summary>
    public void UpdateScene()
    {
        HealthText.text = Health.ToString();
        switch (HealthText.text)
        {
            case "5":
            case "4": gameObject.GetComponent<SpriteRenderer>().sprite = HealthSpriteNormal; break;
            case "3":
            case "2": gameObject.GetComponent<SpriteRenderer>().sprite = HealthSpriteDamaged; break;
            case "1":
            case "0": gameObject.GetComponent<SpriteRenderer>().sprite = HealthSpriteCritical; break;
        }
    }

}
