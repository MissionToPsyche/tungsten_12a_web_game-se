using UnityEngine;

public class IronVeinActivator : MonoBehaviour
{
    public GameObject ironVein;

    private void OnDestroy()
    {
        ironVein.SetActive(true);
    }
}
