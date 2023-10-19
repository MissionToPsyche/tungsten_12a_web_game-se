using UnityEngine;

/// <summary>
/// codefor pickupable items to activate a tool
/// </summary>
/// Author: jmolive8
public class PickupableTool : MonoBehaviour {
    public string toolName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerManagement>().toolPickedUp(toolName);
        Destroy(gameObject);
    }
}
