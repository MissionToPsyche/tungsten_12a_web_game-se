using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroController : MonoBehaviour
{
    public void QuitGame()
    {
        SceneManager.LoadScene("Title_Screen");
    }
}
