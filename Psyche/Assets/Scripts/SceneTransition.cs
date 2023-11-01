using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    //Private variables
    private PlayerManagement _playerManagement;
    
    public void Initialize(PlayerManagement playerManagement)
    {
        _playerManagement = playerManagement;
    }

    /// <summary>
    /// Transitions the player to the respective scene if the necessary parameters are met
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerator CheckTransition(string name)
    {
        //Polling rate
        yield return new WaitForSeconds(0.1f);

        //Get the axis (positive or negative) for vertical buttons
        float verticalAxis = Input.GetAxis("Vertical");

        //If a positive vertical button is pressed (w or up), then transition
        if (Input.GetButton("Vertical") && verticalAxis > 0)
        {
            if (name == "SceneTransition_Home") //For going back to the main scene
                SceneManager.LoadScene("SampleScene");
            if (name == "SceneTransition_Joshua")
                SceneManager.LoadScene("DeveloperScene_Joshua");
            if (name == "SceneTransition_Dhalia")
                SceneManager.LoadScene("DeveloperScene_Dhalia");
            if (name == "SceneTransition_Bryant")
                SceneManager.LoadScene("DeveloperScene_Bryant");
            if (name == "SceneTransition_Matt")
                SceneManager.LoadScene("DeveloperScene_Matt");
            if (name == "SceneTransition_James")
                SceneManager.LoadScene("DeveloperScene_James");
        }
    }
}
