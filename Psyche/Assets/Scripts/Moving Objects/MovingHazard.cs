using UnityEngine;

/// <summary>
/// Basic moving hazard that moves between a list of points
/// </summary>
/// Author: jmolive8
public class MovingHazard : MonoBehaviour
{
    public GameObject hazard;
    public Transform pointsObject;

    [Tooltip("The first point to start moving towards.")]
    public int pointsIndex = 1;
    public float moveSpeed;
    public bool warpToStart = false;

    private Vector2[] points;


    void Start()
    {
        points = new Vector2[pointsObject.childCount];
        int i = 0;
        foreach (Transform child in pointsObject)
        {
            points[i] = child.position;
            i++;
        }
    }

    void Update()
    {
        hazard.transform.position = Vector2.MoveTowards(hazard.transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);

        /**
         * Sets the next target position when current target is reached
         */
        if ((Vector2)hazard.transform.position == points[pointsIndex])
        {
            pointsIndex++;

            /**
             * When final position reached starts moving towards the first position
             */
            if (pointsIndex == points.Length)
            {
                pointsIndex = 0;

                if (warpToStart)
                    hazard.transform.position = points[0];
            }
        }
    }
}
