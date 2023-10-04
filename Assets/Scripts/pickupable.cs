/** 
Description: Test pickupable item to demonstrate dialogue box functions
Author: jmolive8
Version: 1
**/

using UnityEngine;

public class pickupable : MonoBehaviour {
    public dialogueController dialBox;
    public string text;

    private void OnTriggerEnter2D(Collider2D other)
    {
        dialBox.SetText(text);
    }
}
