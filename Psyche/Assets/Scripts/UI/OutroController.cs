using UnityEngine;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// </summary>
/// Author: jmolive8, blopezro
public class OutroController : MonoBehaviour {

    /// <summary>
    /// Gets rid of objects not needed anymore
    /// </summary>
    void Awake() {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
    }

    /// <summary>
    /// Returns to Title Screen when the button is pressed
    /// </summary>
    public void QuitGame()
    {
        GameController.Instance.audioManager.buttonClick.Play();
        string scene = GameController.Instance.gameStateManager.MatchScene(GameStateManager.Scene.Title_Screen);
        GameController.Instance.sceneTransitionManager.devControl = true;
        GameController.Instance.sceneTransitionManager.OnInitiateTransition(scene);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Screen");
    }

}