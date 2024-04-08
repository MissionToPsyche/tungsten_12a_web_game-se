/*
 * Description: Camera aiming script
 * Authors: mcmyers4
 * Version: 20240404
 */

using Cinemachine;
using UnityEngine;

/// <summary>
/// Allows user to bias the camera direction using the mouse cursor.
/// </summary>
public class AimCamera : MonoBehaviour
{
    //======================================== Initialize/Update/Destroy =========================================

    // Private variables
    [SerializeField] private Camera Camera;
    [SerializeField] private CinemachineVirtualCamera PlayerCamera;
    [SerializeField] private CinemachineVirtualCamera MouseCamera;
    [SerializeField] private Transform Player;
    [SerializeField] private float XMax = 4;
    [SerializeField] private float YMax = 2;
    [SerializeField] private float XCushion = 8;
    [SerializeField] private float YCushion = 4;

    /// <summary>
    /// Update is called once per frame.
    /// If the mouse cursor is moved more than the 'cushion' away from the player,
    /// then the camera will switch from one focusing on the player character,
    /// to a camera that will focus on a midpoint between the player and the cursor.
    /// This allows the user to move the camera allowing them to see just off screen,
    /// but only when the mouse cusor is brought to the edge of the screen.
    /// </summary>
    void Update()
    {
        Vector3 cursorPoint = Camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 midPoint = (Player.position + cursorPoint) / 2;

        midPoint.x = Mathf.Clamp(midPoint.x, -XMax + Player.position.x, XMax + Player.position.x);
        midPoint.y = Mathf.Clamp(midPoint.y, -YMax + Player.position.y, YMax + Player.position.y);

        if ((Mathf.Abs(cursorPoint.x - Player.position.x) > XCushion)
            || (Mathf.Abs(cursorPoint.y - Player.position.y) > YCushion))
        {
            MouseCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            PlayerCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            transform.position = midPoint;
        }
        else
        {
            MouseCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            PlayerCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
        }
    }
}
