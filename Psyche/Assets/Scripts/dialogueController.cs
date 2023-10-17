using TMPro;
using UnityEngine;

/// <summary>
/// Functions for the dialogue box
/// </summary>
/// Author: jmolive8
public class DialogueController : MonoBehaviour
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
