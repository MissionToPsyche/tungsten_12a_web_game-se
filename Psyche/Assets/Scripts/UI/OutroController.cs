using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// Crossfading added by dnguye99
/// </summary>
/// Author: jmolive8, blopezro, dnguye99
public class OutroController : MonoBehaviour {

    public GameObject outro1, outro2, outro3, outro4, clickText, quitButton, link;
    private Image img1, img2, img3;
    private bool IsTransitioning;
    private CanvasGroup outro4Group;
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

        //set all their alphas to 0, except img1 (first cs)
        img1.canvasRenderer.SetAlpha(0);
        img2.canvasRenderer.SetAlpha(0);
        img3.canvasRenderer.SetAlpha(0);

        //fade in the first image
        StartCoroutine(FadeIn(img1));

        IsTransitioning = false;

        outro4Group = outro4.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Shows next screen on Left Click
    /// </summary>
    void Update()
    {
        if (Input.GetButtonDown("EMagnet"))
        {
            if (!IsTransitioning)
            {
                switch (curScreen)
                {
                    case 1:
                        StartCoroutine(CrossFade(img1, img2));
                        break;

                    case 2:
                        StartCoroutine(CrossFade(img2, img3));
                        break;

                    case 3:
                        StartCoroutine(CrossFadeLastCS(img3));
                        break;

                    default:
                        break;
                }
                curScreen++;
            }
        }
    }

    /// <summary>
    /// Opens a link to psyche.asu.edu when the player clicks the link on outro4
    /// </summary>
    public void OpenLink()
    {
        Application.OpenURL("https://psyche.asu.edu/");
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
        IsTransitioning = true;

        //fades out the first image
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        //fades in the second image
        img2.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);

        IsTransitioning = false;
    }

    /// <summary>
    /// Fades an image in, and only in. Used for the first cutscene image.
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    private IEnumerator FadeIn(Image img)
    {
        IsTransitioning = true;

        img.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);

        IsTransitioning = false;
    }

    private IEnumerator CrossFadeLastCS(Image img1)
    {
        //disable the "click to continue" text
        clickText.SetActive(false);

        //fades out the first image
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);


        //fades in the second image, the link, and the quit button
        CanvasGroup outro4Group = outro4.GetComponent<CanvasGroup>();
        
        while (outro4Group.alpha < 1)
        {
            outro4Group.alpha += 0.0000002f;
        }
        
        yield return new WaitForSeconds(2.0f);
        
    }
}