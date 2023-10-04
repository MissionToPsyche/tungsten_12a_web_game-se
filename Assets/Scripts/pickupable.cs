using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("GRABBED");
    }
}
