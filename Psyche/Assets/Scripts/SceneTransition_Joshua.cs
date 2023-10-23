using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the scene transition management for the SceneTransition_Joshua object
/// </summary>
public class SceneTransition_Joshua : MonoBehaviour
{
    // When the playercharacter enters the object's bounds, start the coroutine to check for transition rules
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTrigerEnter2D called");
        if(other.tag == "Player")
        {
            Debug.Log("Player Entered the trigger");
            StartCoroutine(CheckTransition());
        }
    }

    IEnumerator CheckTransition()
    {
        //Polling rate
        yield return new WaitForSeconds(0.1f);

        //Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        //If a positive vertical button is pressed (w or up), then transition
        if(Input.GetButtonDown("Vertical") && verticalAxis > 0)
        {
            SceneManager.LoadScene("DeveloperScene_Joshua");
        }
    }
}
