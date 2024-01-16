using UnityEngine;

public class MovingIronVein : MonoBehaviour
{
    public IronConveyorBelt belt;

    private int pointsIndex = 1;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, belt.points[pointsIndex], belt.moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == belt.points[pointsIndex])
            pointsIndex++;

        if (pointsIndex == belt.points.Length)
        {
            transform.position = belt.points[0];
            pointsIndex = 1;
        }
    }
}
