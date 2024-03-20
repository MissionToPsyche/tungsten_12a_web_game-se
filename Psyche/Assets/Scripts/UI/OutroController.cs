using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// Crossfading added by dnguye99
/// </summary>
/// Author: jmolive8, blopezro, dnguye99
public class OutroController : MonoBehaviour {

    public GameObject outro1, outro2, outro3, outro4, clickText, quitButton;
    private Image img1, img2, img3, img4;
    int curScreen = 1;

    /// <summary>
    /// Gets rid of objects not needed anymore
    /// </summary>
    void Awake() {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));

        //get all the images from the cutscene game objects
        img1 = outro1.GetComponentInChildren<Image>();
        img2 = outro2.GetComponentInChildren<Image>();
        img3 = outro3.GetComponentInChildren<Image>();
        img4 = outro4.GetComponentInChildren<Image>();

        //set all their alphas to 0, except img1 (first cs)
        img1.canvasRenderer.SetAlpha(0);
        img2.canvasRenderer.SetAlpha(0);
        img3.canvasRenderer.SetAlpha(0);
        img4.canvasRenderer.SetAlpha(0);

        //fade in the first image
        StartCoroutine(FadeIn(img1));
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
                    StartCoroutine(CrossFade(img1, img2));
                    break;

                case 2:
                    curScreen++;
                    StartCoroutine(CrossFade(img2, img3));
                    break;

                case 3:
                    curScreen++;
                    StartCoroutine(CrossFadeLastCS(img3, img4));
                    break;

                default:
                    // in case there's some error in the outro, the last image still shows, and the correct buttons are active
                    //set all their alphas to 0, except img1 (first cs)
                    if (img4.canvasRenderer.GetAlpha() == 0)
                    {
                        img1.canvasRenderer.SetAlpha(0);
                        img2.canvasRenderer.SetAlpha(0);
                        img3.canvasRenderer.SetAlpha(0);
                        img4.canvasRenderer.SetAlpha(1);

                        //disable the "click to continue" text
                        clickText.SetActive(false);

                        //enable the quit button
                        quitButton.SetActive(true);
                    }
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

    /// <summary>
    /// Fades out the first image and fades in the second image
    /// </summary>
    /// <param name="img1"></param>
    /// <param name="img2"></param>
    /// <returns></returns>
    private IEnumerator CrossFade(Image img1, Image img2)
    {
        //fades out the first image
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        //fades in the second image
        img2.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);
    }

    /// <summary>
    /// Fades an image in, and only in. Used for the first cutscene image.
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    private IEnumerator FadeIn(Image img)
    {
        img.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);
    }

    private IEnumerator CrossFadeLastCS(Image img1, Image img2)
    {
        //disable the "click to continue" text
        clickText.SetActive(false);

        //fades out the first image
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        //fades in the second image
        img2.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(1.0f);

        //enable the quit button
        quitButton.SetActive(true);
    }
}