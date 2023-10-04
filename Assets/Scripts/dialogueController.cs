using TMPro;
using UnityEngine;

public class dialogueController : MonoBehaviour
{
    public TMP_Text Text;
    public void SetText(string dial)
    {
        Text.SetText(dial);
    }
}
