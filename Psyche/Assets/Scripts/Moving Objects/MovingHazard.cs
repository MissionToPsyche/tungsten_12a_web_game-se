using UnityEngine;

/// <summary>
/// Basic moving hazard that moves between a list of points
/// </summary>
/// Author: jmolive8
public class MovingHazard : MonoBehaviour
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
}
