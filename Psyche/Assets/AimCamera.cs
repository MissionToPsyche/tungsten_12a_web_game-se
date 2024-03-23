using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCamera : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private CinemachineVirtualCamera m_PlayerCamera;
    [SerializeField] private CinemachineVirtualCamera m_AimCamera;
    [SerializeField] private Transform m_Player;
    [SerializeField] private float xMax;
    [SerializeField] private float yMax;
    [SerializeField] private float xCushion;
    [SerializeField] private float yCushion;

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorPoint = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 midPoint = (m_Player.position + cursorPoint) / 2;

        midPoint.x = Mathf.Clamp(midPoint.x, -xMax + m_Player.position.x, xMax + m_Player.position.x);
        midPoint.y = Mathf.Clamp(midPoint.y, -yMax + m_Player.position.y, yMax + m_Player.position.y);

        //this.transform.position = midPoint;

        if ((Mathf.Abs(cursorPoint.x - m_Player.position.x) > xCushion)
            || (Mathf.Abs(cursorPoint.y - m_Player.position.y) > yCushion))
        {
            m_AimCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            m_PlayerCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            this.transform.position = midPoint;
        }
        else
        {
            m_AimCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            m_PlayerCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
        }
    }
}
