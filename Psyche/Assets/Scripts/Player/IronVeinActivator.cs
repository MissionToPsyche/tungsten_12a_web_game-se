using UnityEngine;

public class IronVeinActivator : MonoBehaviour
{
    public GameObject firstVeins;

    private void OnDestroy()
    {
        if (firstVeins != null)
            firstVeins.SetActive(true);
    }
}