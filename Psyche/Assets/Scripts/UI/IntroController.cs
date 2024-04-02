using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Basic Intro Cutscene that shows a series of images
/// Crossfading added by dnguye99
/// </summary>
/// Author: jmolive8, dnguye99
public class IntroController : MonoBehaviour
{
    public GameObject intro1, intro2, intro3, intro4, intro5, intro6, intro7;
    private Image img1, img2, img3, img4, img5, img6, img7;
    private bool IsTransitioning;
    int curScreen = 1;

    /// <summary>
    /// Gets all the image components of all the intro cutscene GameObjects,
    /// then sets their alpha to 0, basically setting them to invisible.
    /// Then, fades in the first cutscene image.
    /// </summary>
    private void Start()
    {
        //get all the images from the cs game objects
        img1 = intro1.GetComponentInChildren<Image>();
        img2 = intro2.GetComponentInChildren<Image>();
        img3 = intro3.GetComponentInChildren<Image>();
        img4 = intro4.GetComponentInChildren<Image>();
        img5 = intro5.GetComponentInChildren<Image>();
        img6 = intro6.GetComponentInChildren<Image>();
        img7 = intro7.GetComponentInChildren<Image>();

        //set all their alphas to 0, except img1 (first cs)
        img1.canvasRenderer.SetAlpha(0);
        img2.canvasRenderer.SetAlpha(0);
        img3.canvasRenderer.SetAlpha(0);
        img4.canvasRenderer.SetAlpha(0);
        img5.canvasRenderer.SetAlpha(0);
        img6.canvasRenderer.SetAlpha(0);
        img7.canvasRenderer.SetAlpha(0);

        //fade in the first image
        StartCoroutine(FadeIn(img1));

        IsTransitioning = false;
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
                        StartCoroutine(CrossFade(img3, img4));
                        break;

                    case 4:
                        StartCoroutine(CrossFade(img4, img5));
                        break;

                    case 5:
                        StartCoroutine(CrossFade(img5, img6));
                        break;

                    case 6:
                        StartCoroutine(CrossFade(img6, img7));
                        break;
                    default:
                        // Set the value this way so if any changes are made they are accounted for
                        string scene = GameController.Instance.gameStateManager.MatchScene(GameStateManager.Scene.Landing);
                        GameController.Instance.sceneTransitionManager.devControl = true;
                        GameController.Instance.sceneTransitionManager.OnInitiateTransition(scene);
                        break;
                }
                curScreen++;
            }
        }
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
}
