/*
 * Description: Moving Platforms
 * Authors: mcmyers4
 * Version: 20240202
 */
using UnityEngine;

/// <summary>
/// Basic moving platform that moves between a list of points
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public Transform pointsObject;
    public float moveSpeed;

    private Vector2[] points;

    private int pointsIndex = 1;

    void Start()
    {
        points = new Vector2[pointsObject.childCount];
        int i = 0;
        foreach (Transform child in pointsObject)
        {
            points[i] = child.position;
            i++;
        }

        transform.position = points[0];
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);

        /**
         * Sets the next target position when current target is reached
         */
        if ((Vector2)transform.position == points[pointsIndex])
        {
            pointsIndex++;

            /**
             * When final position reached starts moving towards the first position
             */
            if (pointsIndex == points.Length)
                pointsIndex = 0;
        }
    }

    /// <summary>
    /// Keeps the player on the platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
