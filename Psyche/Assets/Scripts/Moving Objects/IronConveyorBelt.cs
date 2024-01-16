using UnityEngine;

public class IronConveyorBelt : MonoBehaviour
{
    public MovingIronVein movingIronVeinPrefab;
    public Transform[] pointPositions;
    public Vector2[] points;

    public float moveSpeed = 2.5f;
    public int numOfIrons = 1;

    void Start()
    {
        points = new Vector2[pointPositions.Length];
        for (int i = 0; i < pointPositions.Length; i++)
        {
            points[i] = pointPositions[i].position;
        }

        Instantiate(movingIronVeinPrefab, transform).belt = this;
    }

    void Update()
    {
        
    }
}
