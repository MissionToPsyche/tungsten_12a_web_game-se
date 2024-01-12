using UnityEngine;

public class MovingIronVeins : MonoBehaviour //add more comments
{
    [SerializeField] private GameObject ironVein;
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

        ironVein.transform.position = points[0];
    }

    void Update()
    {
        ironVein.transform.position = Vector2.MoveTowards(ironVein.transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);

        if ((Vector2)ironVein.transform.position == points[pointsIndex])
            pointsIndex++;

        if (pointsIndex == points.Length)
        {
            ironVein.transform.position = points[0];
            pointsIndex = 1;
        }
    }
}
