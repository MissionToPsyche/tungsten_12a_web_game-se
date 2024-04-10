using UnityEngine;

/// <summary>
/// Turns on the first few Magnetized Deposits on the EMagnet level when the Magnetometer is picked up to show that it allows you to see Deposits.
/// Component of ElectroMagnet pickup
/// </summary>
public class FirstDepositsActivator : MonoBehaviour
{
    public GameObject FirstDeposits;

    private void OnDestroy()
    {
        if (FirstDeposits != null)
        {
            FirstDeposits.SetActive(true);
        }
    }
}