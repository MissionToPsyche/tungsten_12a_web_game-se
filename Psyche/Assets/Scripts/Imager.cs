/** 
Description: imager on cursor tool script
Author: mcmyers4
Version: 20231027
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Imager tool script which gives the player a flashlight feature controlled by the mouse cursor.
/// </summary>
public class Imager : MonoBehaviour
{
    //Mouse cursor
    Vector3 cursorPoint;
    public float speed = 1f;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D flashlight;

    //Make cursor invisible
    void Start()
    {
        flashlight.intensity = 0f;
        Cursor.visible = false;
        this.enabled = false;
    }

    //Follow cursor movement
    public void Update()
    {
        this.enabled = true;
        flashlight.intensity = 1f;
        cursorPoint = Input.mousePosition;
        cursorPoint.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(cursorPoint);
    }
}
