/** 
Description: imager on cursor tool script
Author: mcmyers4
Version: 20231024
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Imager tool script increases field of vision surrounding mouse cursor with every additional imager pick up.
/// </summary>
public class ImagerCursor : MonoBehaviour
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
