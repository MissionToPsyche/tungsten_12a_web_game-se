/*
 * Description: Disappearing Platform Script
 * Authors: mcmyers4
 * Version: 20240402
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggles a platform on and off
/// </summary>
public class DisappearingPlatform : MonoBehaviour
{
    // Variables for timing
    public float OffDuration = 1;
    public float OnDuration = 4;
    private float Now = 0;
    private bool Enabled = true;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Enabled = true;
    }

    /// <summary>
    /// Update is called once per frame.
    /// Waits for the set on or off duration and
    /// toggles the platform on or off accordingly.
    /// </summary>
    void Update()
    {
        Now += Time.deltaTime;
        if (Enabled && Now >= OnDuration)
        {
            Now = 0;
            TogglePlatform();
        }
        else if (!Enabled && Now >= OffDuration) 
        {
            Now = 0;
            TogglePlatform();
        }
    }
    
    /// <summary>
    /// Toggles on and off all child objects of the
    /// platform parent except for the player.
    /// </summary>
    void TogglePlatform()
    {
        Enabled = !Enabled;
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag != "Player")
            {
                child.gameObject.SetActive(Enabled);
            }
        }
    }
}
