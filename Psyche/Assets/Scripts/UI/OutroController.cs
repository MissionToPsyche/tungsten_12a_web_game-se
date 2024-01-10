using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroController : MonoBehaviour
{
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Screen");
    }
}
