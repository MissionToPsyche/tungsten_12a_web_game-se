using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagerCursor : MonoBehaviour
{
    Vector3 cursorPoint;
    public float speed = 1f;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        cursorPoint = Input.mousePosition;
        cursorPoint.z = speed;
        transform.position = Camera.main.ScreenToWorldPoint(cursorPoint);
    }
}
