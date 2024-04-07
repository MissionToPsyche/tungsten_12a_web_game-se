using System.Collections;
using UnityEngine;

/// <summary>
/// Code for Conveyor Belt that has Magnetized Deposits moving along it for the player to use the ElectroMagnet on
/// </summary>
/// Author: jmolive8
public class ConveyorBelt : MonoBehaviour
{
    public GameObject DepositPrefab;
    public Transform PointsObject;

    public float MoveSpeed = 2.5f, SpacingBetweenDeposits = 2;
    public int NumOfDeposits = 3;

    private Vector2[] Points;
    private WaitForSeconds SpawnDelay;

    void Start()
    {
        SpawnDelay = new WaitForSeconds(SpacingBetweenDeposits);

        Points = new Vector2[PointsObject.childCount];
        int i = 0;
        foreach (Transform child in PointsObject)
        {
            Points[i] = child.position;
            i++;
        }

        StartCoroutine(spawnDeposit());
    }

    /// <summary>
    /// Spawns the specified number of Magnetized Deposits with a delay to ensure there is space between them while they are moving
    /// </summary>
    private IEnumerator spawnDeposit()
    {
        for (int i = 0; i < NumOfDeposits; i++)
        {
            GameObject deposit = Instantiate(DepositPrefab, transform);
            deposit.transform.position = Points[0];
            StartCoroutine(moveDeposit(deposit));
            yield return SpawnDelay;
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
            Vector2 moveVect = Vector2.MoveTowards(deposit.transform.position, Points[pointsIndex], MoveSpeed * Time.deltaTime);
            deposit.transform.position = new Vector3(moveVect.x, moveVect.y, deposit.transform.position.z);

            /**
             * Sets the next target position when current target is reached
             */
            if ((Vector2)deposit.transform.position == Points[pointsIndex])
            {
                pointsIndex++;

                /**
                 * When final position reached makes Magnetized Deposit disappear for a few seconds and reappear at the first position
                 */
                if (pointsIndex == Points.Length)
                {
                    deposit.SetActive(false);

                    yield return SpawnDelay;

                    deposit.transform.position = new Vector3(Points[0].x, Points[0].y, deposit.transform.position.z);
                    pointsIndex = 1;
                    deposit.SetActive(true);
                }
            }

            yield return null;
        }
    }
}
