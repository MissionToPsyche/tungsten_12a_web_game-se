using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCamera : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Transform m_Player;
    [SerializeField] private float xMax;
    [SerializeField] private float yMax;

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorPoint = m_Camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 midPoint = (m_Player.position + cursorPoint) / 2;

        midPoint.x = Mathf.Clamp(midPoint.x, -xMax + m_Player.position.x, xMax + m_Player.position.x);
        midPoint.y = Mathf.Clamp(midPoint.y, -yMax + m_Player.position.y, yMax + m_Player.position.y);

        this.transform.position = midPoint;
    }
}
