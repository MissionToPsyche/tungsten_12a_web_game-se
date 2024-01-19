using System.Collections;
using UnityEngine;

public class IronConveyorBelt : MonoBehaviour
{
    public GameObject ironVeinPrefab;
    public Transform[] pointPositions;

    public float moveSpeed = 2.5f, spacingBetweenIrons = 2;
    public int numOfIrons = 3;

    private Vector2[] points;
    private WaitForSeconds spawnDelay;

    void Start()
    {
        spawnDelay = new WaitForSeconds(spacingBetweenIrons);
        points = new Vector2[pointPositions.Length];
        for (int i = 0; i < pointPositions.Length; i++)
        {
            points[i] = pointPositions[i].position;
        }

        StartCoroutine(spawnIron());
    }

    private IEnumerator spawnIron()
    {
        for (int i = 0; i < numOfIrons; i++)
        {
            StartCoroutine(moveIron(Instantiate(ironVeinPrefab, pointPositions[0])));
            yield return spawnDelay;
        }
    }

    private IEnumerator moveIron(GameObject vein)
    {
        int pointsIndex = 1;

        while (true)
        {
            vein.transform.position = Vector2.MoveTowards(vein.transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);

            if ((Vector2)vein.transform.position == points[pointsIndex])
                pointsIndex++;

            if (pointsIndex == points.Length)
            {
                vein.SetActive(false);

                yield return spawnDelay;

                vein.transform.position = points[0];
                pointsIndex = 1;
                vein.SetActive(true);
            }

            yield return null;
        }
    }
}
