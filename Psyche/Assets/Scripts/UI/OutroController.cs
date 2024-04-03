using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// Basic Outro Cutscene that shows an image
/// Crossfading added by dnguye99
/// </summary>
/// Author: jmolive8, blopezro, dnguye99
public class OutroController : MonoBehaviour {

    public GameObject outro1, outro2, outro3, outro4, clickText;
    public Button quitButton, link;
    private Image img1, img2, img3, img4, buttonImg, linkImg;
    private TMP_Text buttonText;
    private bool IsTransitioning;
    private Color buttonColor;
    int curScreen = 1;

    /// <summary>
    /// Gets rid of objects not needed anymore
    /// </summary>
    void Awake() {
        //remove the Player and UI objects so they aren't on screen
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));

        //get all the images from the cutscene game objects
        img1 = outro1.GetComponentInChildren<Image>();
        img2 = outro2.GetComponentInChildren<Image>();
        img3 = outro3.GetComponentInChildren<Image>();
        img4 = outro4.GetComponentInChildren<Image>();

        //get the images/sprites from the buttons
        buttonImg = quitButton.GetComponent<Image>();
        buttonText = quitButton.GetComponentInChildren<TMP_Text>();
        linkImg = link.GetComponent<Image>();

        //save the Quit to Title button color
        buttonColor = buttonImg.color;

        //then make the button not interactable
        quitButton.interactable = false;

        //set all the components' alphas to 0, except img1 (first cs)
        img1.canvasRenderer.SetAlpha(0);
        img2.canvasRenderer.SetAlpha(0);
        img3.canvasRenderer.SetAlpha(0);
        img4.canvasRenderer.SetAlpha(0);
        linkImg.canvasRenderer.SetAlpha(0);
        buttonText.canvasRenderer.SetAlpha(0);
        //the quit button doesn't have an "image", so it has to adjust the color on the "image"
        buttonImg.color = new Color(buttonText.color.r, buttonText.color.b, buttonText.color.b, 0);

        //fade in the first image
        StartCoroutine(FadeIn(img1));

        //set bool for transitioning
        IsTransitioning = true;
    }

    /// <summary>
    /// Shows next screen on Left Click
    /// </summary>
    void Update()
    {
        //checks if the player has clicked
        if (Input.GetButtonDown("EMagnet"))
        {
            //checks if there is a transition between images currently happening
            if (!IsTransitioning)
            {
                //crossfades if there is no transition occurring
                switch (curScreen)
                {
                    case 1:
                        StartCoroutine(CrossFade(img1, img2));
                        break;
                    case 2:
                        StartCoroutine(CrossFade(img2, img3));
                        break;
                    case 3:
                        StartCoroutine(CrossFadeLastCS(img3, img4));
                        break;
                    default:
                        break;
                }
                curScreen++; //increment the counter 
            }
        }
    }

    /// <summary>
    /// Opens a link to psyche.asu.edu when the player clicks the link on the last scene of the outro
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
    }

    /// <summary>
    /// Fades out the first image and fades in the second image
    /// </summary>
    /// <param name="img1"></param>
    /// <param name="img2"></param>
    /// <returns></returns>
    private IEnumerator CrossFade(Image img1, Image img2)
    {
        //turn on the transition flag
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
        //set the transition flag
        IsTransitioning = true;

        //fade in the image
        img.CrossFadeAlpha(1, 2.0f, false);
        yield return new WaitForSeconds(2.0f);

        //turn off the transition flag
        IsTransitioning = false;
    }

    /// <summary>
    /// Fades out the second to last image in the cutscene, then fades in the
    /// last image, as well as the buttons
    /// </summary>
    /// <param name="img1"></param>
    /// <param name="img2"></param>
    /// <returns></returns>
    private IEnumerator CrossFadeLastCS(Image img1, Image img2)
    {
        //variables needed to fade in the Quit Button
        float counter = 0f;
        float duration = 2.0f;

        //disable the "click to continue" text
        clickText.SetActive(false);

        //fades out the first image (cs3)
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        //fades in last cs image and its components
        img2.CrossFadeAlpha(1, 2.0f, false);
        linkImg.CrossFadeAlpha(1, 2.0f, false);
        buttonText.CrossFadeAlpha(1, 2.0f, false);
        
        //fading in the Quit button by adjusting the alpha value little by little
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter/duration);
            buttonColor.a = alpha;
            buttonImg.color = buttonColor;
        }
        yield return new WaitForSeconds(2.0f);

        //set the buttons to interactable once the fading has finished
        quitButton.interactable = true;
        link.interactable = true;
    }
}