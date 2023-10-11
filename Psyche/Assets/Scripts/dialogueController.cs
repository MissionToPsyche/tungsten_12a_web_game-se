/** 
Description: Functions for the dialogue box
Author: jmolive8
Version: 1
**/

using TMPro;
using UnityEngine;

public class dialogueController : MonoBehaviour
{
    public TMP_Text Text;
    public void SetText(string dial)
    {
        Text.SetText(dial);
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            gameObject.SetActive(false);
        }
    }
}
