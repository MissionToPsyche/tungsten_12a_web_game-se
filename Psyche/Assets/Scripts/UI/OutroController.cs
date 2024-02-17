using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// </summary>
/// Author: jmolive8, blopezro
public class OutroController : MonoBehaviour {

    /// <summary>
    /// Gets rid of objects not needed anymore
    /// </summary>
    void Awake() {
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
    }

    /// <summary>
    /// Returns to Title Screen when the button is pressed
    /// </summary>
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Screen");
        GameController.Instance.audioManager.buttonClick.Play();
    }

}