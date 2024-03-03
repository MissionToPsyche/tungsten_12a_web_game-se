using UnityEngine;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// </summary>
/// Author: jmolive8, blopezro
public class OutroController : MonoBehaviour {

    public GameObject outro1, outro2, outro3, outro4;
    int curScreen = 1;

    /// <summary>
    /// Gets rid of objects not needed anymore
    /// </summary>
    void Awake() {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));
    }

    /// <summary>
    /// Shows next screen on Left Click
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("EMagnet"))
        {
            switch (curScreen)
            {
                case 1:
                    curScreen++;
                    outro1.SetActive(false);
                    outro2.SetActive(true);
                    break;

                case 2:
                    curScreen++;
                    outro2.SetActive(false);
                    outro3.SetActive(true);
                    break;

                case 3:
                    curScreen++;
                    outro3.SetActive(false);
                    outro4.SetActive(true);
                    break;

                default:
                    // in case there's some error in the outro, the last image still shows
                    if (!outro4.activeSelf)
                    {
                        outro1.SetActive(false);
                        outro2.SetActive(false);
                        outro3.SetActive(false);
                        outro4.SetActive(true);
                    };
                    break;
            }
        }
    }

    /// <summary>
    /// Returns to Title Screen when the button is pressed
    /// </summary>
    public void QuitGame()
    {
        GameController.Instance.audioManager.buttonClick.Play();
        string scene = GameController.Instance.gameStateManager.MatchScene(GameStateManager.Scene.Title);
        GameController.Instance.sceneTransitionManager.devControl = true;
        GameController.Instance.sceneTransitionManager.OnInitiateTransition(scene);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Title_Screen");
    }

}