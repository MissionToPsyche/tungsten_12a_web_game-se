using UnityEngine;

/// <summary>
/// Basic moving hazard that moves between a list of points
/// </summary>
/// Author: jmolive8
public class MovingHazard : MonoBehaviour
{
    public GameObject Hazard;
    public Transform PointsObject;

    [Tooltip("The first point to start moving towards.")]
    public int PointsIndex = 1;
    public float MoveSpeed = 2.5f;
    public bool WarpToStart = false;

    private Vector2[] Points;

    void Start()
    {
        Points = new Vector2[PointsObject.childCount];
        int i = 0;
        foreach (Transform child in PointsObject)
        {
            Points[i] = child.position;
            i++;
        }
    }

    void Update()
    {
        Vector2 moveVect = Vector2.MoveTowards(Hazard.transform.position, Points[PointsIndex], MoveSpeed * Time.deltaTime);
        Hazard.transform.position = new Vector3(moveVect.x, moveVect.y, Hazard.transform.position.z);

        /**
         * Sets the next target position when current target is reached
         */
        if ((Vector2)Hazard.transform.position == Points[PointsIndex])
        {
            PointsIndex++;

            /**
             * When final position reached starts moving towards the first position
             */
            if (PointsIndex == Points.Length)
            {
                PointsIndex = 0;

                if (WarpToStart)
                {
                    Hazard.transform.position = new Vector3(Points[0].x, Points[0].y, Hazard.transform.position.z);
                }
            }
        }
    }
}
