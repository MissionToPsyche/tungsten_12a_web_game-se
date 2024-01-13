/** 
Description: imager tool script
Author: mcmyers4
Version: 20231031
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Imager tool script to enable light that follows mouse cursor.
/// </summary>
public class Flashlight : MonoBehaviour
{
    //Mouse cursor location
    Vector3 cursorPoint;
    //Speed to match cursor
    public float speed = 1f;
    //2d spotlight to put on mouse cursor
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D flashlight;

    //Start with everything invisible
    void Start()
    {
        flashlight.intensity = 0f;
        Cursor.visible = false;
        this.enabled = false;
    }

    //Turns on functionality
    public void Update()
    {
        this.enabled = true;
        flashlight.intensity = 1f;
        cursorPoint = Input.mousePosition;
        cursorPoint.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(cursorPoint);
    }
}
