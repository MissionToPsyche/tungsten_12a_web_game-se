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

    //Make cursor invisible
    void Start()
    {
        Cursor.visible = false;
    }

    //Follow cursor movement
    void Update()
    {
        cursorPoint = Input.mousePosition;
        cursorPoint.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(cursorPoint);
    }
}
