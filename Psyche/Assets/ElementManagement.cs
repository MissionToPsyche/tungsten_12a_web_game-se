using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the elements and their respective tracking
/// </summary>
public class ElementManagement : MonoBehaviour
{
    public static ElementManagement Instance;
    public int copper = 0;
    public int iron = 0;
    public int nickel = 0;
    public int gold = 0;
    public int silicon = 0;
    
    /// <summary>
    /// Singleton check
    /// </summary>
    private void Awake()
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

    /// <summary>
    /// When the player walks over the object, pick up the object
    /// </summary>
    public void ElementPickUp (string element)
    {
        if (element == "Element_Copper")
            copper++;
        if (element == "Element_Iron")
            iron++;
        if (element == "Element_Nickel")
            nickel++;
        if (element == "Element_Gold")
            gold++;
        if (element == "Element_Silicon")
            silicon++;
        Debug.Log("Copper: " + copper + " Iron: " + iron + "Nickel: " + nickel + "Gold: " + gold + "Silicon: " + silicon);
    }
}
