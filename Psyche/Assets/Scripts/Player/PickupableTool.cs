using UnityEngine;

/// <summary>
/// Code for pickupable items to activate a tool.
/// To make a new tool pickup use the Base Pickup prefab and set it's toolName and Sprite from the inspector
/// </summary>
/// Author: jmolive8
public class PickupableTool : MonoBehaviour {
    public string toolName;

    /// <summary>
    /// Passes name of tool to PlayerManagement to determine which tool to activate.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController.Instance.ToolPickUp(toolName);
        Destroy(gameObject);
    }
}
