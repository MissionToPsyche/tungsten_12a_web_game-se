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
    [Header("Images")]
    public GameObject Intro1;
    public GameObject Intro2;
    public GameObject Intro3;
    public GameObject Intro4;
    public GameObject Intro5;
    public GameObject Intro6;
    public GameObject Intro7;

    //variables
    private Image Img1, Img2, Img3, Img4, Img5, Img6, Img7;
    private bool IsTransitioning;
    int CurScreen = 1;

    /// <summary>
    /// Gets all the image components of all the intro cutscene GameObjects,
    /// then sets their alpha to 0, basically setting them to invisible.
    /// Then, fades in the first cutscene image.
    /// </summary>
    private void Start()
    {
        //get all the images from the Scene's game objects
        Img1 = Intro1.GetComponentInChildren<Image>();
        Img2 = Intro2.GetComponentInChildren<Image>();
        Img3 = Intro3.GetComponentInChildren<Image>();
        Img4 = Intro4.GetComponentInChildren<Image>();
        Img5 = Intro5.GetComponentInChildren<Image>();
        Img6 = Intro6.GetComponentInChildren<Image>();
        Img7 = Intro7.GetComponentInChildren<Image>();

        //set all their alphas to 0
        Img1.canvasRenderer.SetAlpha(0);
        Img2.canvasRenderer.SetAlpha(0);
        Img3.canvasRenderer.SetAlpha(0);
        Img4.canvasRenderer.SetAlpha(0);
        Img5.canvasRenderer.SetAlpha(0);
        Img6.canvasRenderer.SetAlpha(0);
        Img7.canvasRenderer.SetAlpha(0);

        //fade in the first image
        StartCoroutine(FadeIn(Img1));
    }

    /// <summary>
    /// Handles the transitioning when the player clicks Left Click
    /// </summary>
    void Update()
    {
        //if the player has Left Clicked
        if (Input.GetButtonDown("EMagnet"))
        {
            //check if a cutscene is in the middle of transitioning between two images
            //necessary to ensure transitions behave correctly without overlapping images
            if (!IsTransitioning)
            {
                //handle the transition
                switch (CurScreen)
                {
                    case 1:
                        StartCoroutine(CrossFade(Img1, Img2));
                        break;
                    case 2:
                        StartCoroutine(CrossFade(Img2, Img3));
                        break;
                    case 3:
                        StartCoroutine(CrossFade(Img3, Img4));
                        break;
                    case 4:
                        StartCoroutine(CrossFade(Img4, Img5));
                        break;
                    case 5:
                        StartCoroutine(CrossFade(Img5, Img6));
                        break;
                    case 6:
                        StartCoroutine(CrossFade(Img6, Img7));
                        break;
                    default:
                        // transition into the Landing Scene
                        // Set the value this way so if any changes are made they are accounted for
                        string scene = GameController.Instance.GameStateManager.MatchScene(GameStateManager.Scene.Landing);
                        GameController.Instance.SceneTransitionManager.devControl = true;
                        GameController.Instance.SceneTransitionManager.OnInitiateTransition(scene);
                        break;
                }
                //increment the counter
                CurScreen++;
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
        //trun on the transition flag
        IsTransitioning = true;

        //fades out the first image
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        //fades in the second image
        img2.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);

        //turn off the transition flag
        IsTransitioning = false;
    }

    /// <summary>
    /// Fades an image in, and only in. Used for the first cutscene image.
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    private IEnumerator FadeIn(Image img)
    {
        //turn on the transition flag
        IsTransitioning = true;

        //fade in the image
        img.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);

        //turn off the transition flag
        IsTransitioning = false;
    }
}
