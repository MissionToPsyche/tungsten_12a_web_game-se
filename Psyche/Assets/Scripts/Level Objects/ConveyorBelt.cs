using System.Collections;
using UnityEngine;

/// <summary>
/// Code for Conveyor Belt that has Magnetized Deposits moving along it for the player to use the ElectroMagnet on
/// </summary>
/// Author: jmolive8
public class ConveyorBelt : MonoBehaviour
{
    public GameObject depositPrefab;
    public Transform pointsObject;

    public float moveSpeed = 2.5f, spacingBetweenDeposits = 2;
    public int numOfDeposits = 3;

    private Vector2[] points;
    private WaitForSeconds spawnDelay;

    void Start()
    {
        spawnDelay = new WaitForSeconds(spacingBetweenDeposits);

        points = new Vector2[pointsObject.childCount];
        int i = 0;
        foreach (Transform child in pointsObject)
        {
            points[i] = child.position;
            i++;
        }

        StartCoroutine(spawnDeposit());
    }

    /// <summary>
    /// Spawns the specified number of Magnetized Deposits with a delay to ensure there is space between them while they are moving
    /// </summary>
    private IEnumerator spawnDeposit()
    {
        for (int i = 0; i < numOfDeposits; i++)
        {
            GameObject vein = Instantiate(depositPrefab, transform);
            vein.transform.position = points[0];
            StartCoroutine(moveDeposit(vein));
            yield return spawnDelay;
        }
    }

    /// <summary>
    /// Handles moving Magnetized Deposits along the Belt's path
    /// </summary>
    private IEnumerator moveDeposit(GameObject deposit)
    {
        int pointsIndex = 1;

        while (true)
        {
            Vector2 moveVect = Vector2.MoveTowards(deposit.transform.position, points[pointsIndex], moveSpeed * Time.deltaTime);
            deposit.transform.position = new Vector3(moveVect.x, moveVect.y, deposit.transform.position.z);

            /**
             * Sets the next target position when current target is reached
             */
            if ((Vector2)deposit.transform.position == points[pointsIndex])
            {
                pointsIndex++;

                /**
                 * When final position reached makes Magnetized Deposit disappear for a few seconds and reappear at the first position
                 */
                if (pointsIndex == points.Length)
                {
                    deposit.SetActive(false);

                    yield return spawnDelay;

                    deposit.transform.position = new Vector3(points[0].x, points[0].y, deposit.transform.position.z);
                    pointsIndex = 1;
                    deposit.SetActive(true);
                }
            }

            yield return null;
        }
    }
}
