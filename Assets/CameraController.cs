using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] cameraPositions;
    public Transform playerTransform; // Reference to player's transform
    public float transitionSpeed = 15.0f;
    private Transform targetPosition;

    void Start()
    {
        if (cameraPositions.Length > 0)
        {
            targetPosition = cameraPositions[0];
        }
    }

    void Update()
    {
        MoveCamera();

        CheckPlayerPosition();
    }

    void MoveCamera()
    {
        if (targetPosition)
        {
            Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition.position, transitionSpeed * Time.deltaTime);
            transform.position = new Vector3(smoothPosition.x, smoothPosition.y, transform.position.z);
        }
    }

    void CheckPlayerPosition()
    {
        float leftEdge = transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        float rightEdge = transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;

        if (playerTransform.position.x <= leftEdge || playerTransform.position.x >= rightEdge)
        {
            SwitchCameraSection();
        }
    }

    void SwitchCameraSection()
    {
        for (int i = 0; i < cameraPositions.Length; i++)
        {
            if (cameraPositions[i] == targetPosition && i < cameraPositions.Length - 1)
            {
                targetPosition = cameraPositions[i + 1];
                break;
            }
        }
    }
}
