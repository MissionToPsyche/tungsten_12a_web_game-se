using System.Collections;
using UnityEngine;

/// <summary>
/// Code for Conveyor Belt that has Iron Veins moving along it for the player to use the ElectroMagnet on
/// </summary>
/// Author: jmolive8
public class IronConveyorBelt : MonoBehaviour
{
    public GameObject ironVeinPrefab;
    public Transform pointsObject;

    public float moveSpeed = 2.5f, spacingBetweenIrons = 2;
    public int numOfIrons = 3;

    private Vector2[] points;
    private WaitForSeconds spawnDelay;

    void Start()
    {
        spawnDelay = new WaitForSeconds(spacingBetweenIrons);

        points = new Vector2[pointsObject.childCount];
        int i = 0;
        foreach (Transform child in pointsObject)
        {
            points[i] = child.position;
            i++;
        }

        StartCoroutine(spawnIron());
    }

    /// <summary>
    /// Spawns the specified number of Iron Veins with a delay to ensure there is space between them while they are moving
    /// </summary>
    private IEnumerator spawnIron()
    {
        for (int i = 0; i < numOfIrons; i++)
        {
            StartCoroutine(moveIron(Instantiate(ironVeinPrefab, pointsObject.GetChild(0))));
            yield return spawnDelay;
        }
    }

    /// <summary>
    /// Handles moving Iron Veins along the Belt's path
    /// </summary>
    private IEnumerator moveIron(GameObject vein)
    {
        int pointsIndex = 1;

        while (true)
        {
            vein.transform.position = Vector2.MoveTowards(vein.transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);

            /**
             * Sets the next target position when current target is reached
             */
            if ((Vector2)vein.transform.position == points[pointsIndex])
            {
                pointsIndex++;

                /**
                 * When final position reached makes Iron Vein disappear for a few seconds and reappear at the first position
                 */
                if (pointsIndex == points.Length)
                {
                    vein.SetActive(false);

                    yield return spawnDelay;

                    vein.transform.position = points[0];
                    pointsIndex = 1;
                    vein.SetActive(true);
                }
            }

            yield return null;
        }
    }
}
