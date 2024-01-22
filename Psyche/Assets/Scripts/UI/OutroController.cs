using UnityEngine;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// </summary>
/// Author: jmolive8
public class OutroController : MonoBehaviour
{
    /// <summary>
    /// Returns to Title Screen when the button is pressed
    /// </summary>
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Screen");
    }
}
