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

    [Header("Images")]
    public GameObject Outro1;
    public GameObject Outro2;
    public GameObject Outro3;
    public GameObject Outro4;
    public GameObject ClickToContText;


    [Header("Buttons")]
    public Button QuitButton;
    public Button LinkButton;

    //variables
    private Image Img1, Img2, Img3, Img4, ButtonImg, LinkImg;
    private TMP_Text ButtonText;
    private bool IsTransitioning;
    private Color ButtonColor;
    private int CurScreen = 1;

    /// <summary>
    /// Gets rid of objects not needed anymore, as well as instantiates
    /// objects needed for transitioning and fading.
    /// </summary>
    void Awake() {
        //remove the Player and UI objects so they aren't on screen
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("UI"));

        //get all the images from the cutscene game objects
        Img1 = Outro1.GetComponentInChildren<Image>();
        Img2 = Outro2.GetComponentInChildren<Image>();
        Img3 = Outro3.GetComponentInChildren<Image>();
        Img4 = Outro4.GetComponentInChildren<Image>();

        //get the images/sprites from the buttons
        ButtonImg = QuitButton.GetComponent<Image>();
        ButtonText = QuitButton.GetComponentInChildren<TMP_Text>();
        LinkImg = LinkButton.GetComponent<Image>();

        //save the Quit to Title button color
        //This has to happen before making the button non-interactable, as non-interactable buttons have a different color
        ButtonColor = ButtonImg.color;

        //then make the button not interactable
        QuitButton.interactable = false;

        //set all the components' alphas to 0, except img1 (first cs)
        Img1.canvasRenderer.SetAlpha(0);
        Img2.canvasRenderer.SetAlpha(0);
        Img3.canvasRenderer.SetAlpha(0);
        Img4.canvasRenderer.SetAlpha(0);
        LinkImg.canvasRenderer.SetAlpha(0);
        ButtonText.canvasRenderer.SetAlpha(0);
        //the quit button doesn't have an "image", so it has to adjust the color on the "image"
        ButtonImg.color = new Color(ButtonText.color.r, ButtonText.color.b, ButtonText.color.b, 0);

        //fade in the first image
        StartCoroutine(FadeIn(Img1));
    }

    /// <summary>
    /// Handles the transitioning when the player clicks Left Click
    /// </summary>
    void Update()
    {
        //checks if the player has clicked
        if (Input.GetButtonDown("EMagnet"))
        {
            //checks if there is a transition between images currently happening
            //necessary to ensure transitions behave correctly without overlapping images
            if (!IsTransitioning)
            {
                //crossfades if there is no transition occurring
                switch (CurScreen)
                {
                    case 1:
                        StartCoroutine(CrossFade(Img1, Img2));
                        break;
                    case 2:
                        StartCoroutine(CrossFade(Img2, Img3));
                        break;
                    case 3:
                        StartCoroutine(CrossFadeLastCS(Img3, Img4));
                        break;
                    default:
                        break;
                }
                //increment the counter 
                CurScreen++;
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
        GameController.Instance.AudioManager.buttonClick.Play();
        string scene = GameController.Instance.GameStateManager.MatchScene(GameStateManager.Scene.Title);
        GameController.Instance.SceneTransitionManager.devControl = true;
        GameController.Instance.SceneTransitionManager.OnInitiateTransition(scene);
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
        //counter keeps track of the current time since the start of the fading, and duration is how long the fading should take
        float counter = 0f;
        float duration = 2.0f;

        //disable the "click to continue" text; only the Quit Button will move the player to a new screen
        ClickToContText.SetActive(false);

        //fades out the first image (cs3)
        img1.CrossFadeAlpha(0, 1.0f, false);
        yield return new WaitForSeconds(1.0f);

        //fades in last cs image and its components
        img2.CrossFadeAlpha(1, 2.0f, false);
        LinkImg.CrossFadeAlpha(1, 2.0f, false);
        ButtonText.CrossFadeAlpha(1, 2.0f, false);
        
        //fading in the Quit button by adjusting the alpha value little by little
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter/duration);
            ButtonColor.a = alpha;
            ButtonImg.color = ButtonColor;
        }
        yield return new WaitForSeconds(2.0f);

        //set the buttons to interactable once the fading has finished
        QuitButton.interactable = true;
        LinkButton.interactable = true;
    }
}