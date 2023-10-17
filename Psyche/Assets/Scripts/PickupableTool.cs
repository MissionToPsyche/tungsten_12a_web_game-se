using UnityEngine;

/// <summary>
/// codefor pickupable items to activate a tool
/// </summary>
/// Author: jmolive8
public class PickupableTool : MonoBehaviour {
    public PlayerManagement pManage;
    public string toolName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        pManage.toolPickedUp(toolName);
        Destroy(gameObject);
    }
}
