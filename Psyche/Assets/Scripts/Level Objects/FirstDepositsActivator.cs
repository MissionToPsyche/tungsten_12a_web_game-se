using UnityEngine;

public class FirstDepositsActivator : MonoBehaviour
{
    public GameObject firstDeposits;

    private void OnDestroy()
    {
        if (firstDeposits != null)
            firstDeposits.SetActive(true);
    }
}