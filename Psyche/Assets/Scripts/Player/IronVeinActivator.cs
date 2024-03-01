using UnityEngine;

public class IronVeinActivator : MonoBehaviour
{
    public GameObject ironVein;

    private void OnDestroy()
    {
        if (ironVein != null)
        {
            ironVein.SetActive(true);
        }
    }
}