using UnityEngine;

public class MovingHazard : MonoBehaviour //add more comments
{
    [SerializeField] private Transform[] pointPositions;
    [SerializeField] private float moveSpeed;

    private Vector2[] points;

    private int pointsIndex = 1;

    void Start()
    {
        points = new Vector2[pointPositions.Length];
        for (int i = 0; i < pointPositions.Length; i++)
        {
            points[i] = pointPositions[i].position;
        }

        transform.position = points[0];
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == points[pointsIndex])
            pointsIndex++;

        if (pointsIndex == points.Length)
            pointsIndex = 0;
    }
}
