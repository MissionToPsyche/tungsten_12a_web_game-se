/*
 * Description: Disappearing Platform Script
 * Authors: mcmyers4
 * Version: 20240215
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toggles a platform on and off
/// </summary>
public class DisappearingPlatform : MonoBehaviour
{
    public float offDuration = 1;
    public float onDuration = 4;
    private float now = 0;
    private new bool enabled = true;

    // Start is called before the first frame update
    void Start()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        now += Time.deltaTime;
        if (enabled && now >= onDuration)
        {
            now = 0;
            TogglePlatform();
        } else if (!enabled && now >= offDuration) 
        {
            now = 0;
            TogglePlatform();
        }
    }
    
    /// <summary>
    /// Toggles on and off all child objects except for the player
    /// </summary>
    void TogglePlatform()
    {
        enabled = !enabled;
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag != "Player")
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }
}
