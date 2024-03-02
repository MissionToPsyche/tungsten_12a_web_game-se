using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Basic Intro Cutscene that shows a series of images
/// </summary>
/// Author: jmolive8
public class IntroController : MonoBehaviour
{
    public static GameObject intro1, intro2, intro3, intro4, intro5, intro6, intro7;

    private Image img1 = intro1.GetComponent<Image>();
    private Image img2 = intro2.GetComponent<Image>();
    private Image img3 = intro3.GetComponent<Image>();
    private Image img4 = intro4.GetComponent<Image>();
    private Image img5 = intro5.GetComponent<Image>();
    private Image img6 = intro6.GetComponent<Image>();
    private Image img7 = intro7.GetComponent<Image>();


    int curScreen = 1;

    private void Start()
    {
        img1.canvasRenderer.SetAlpha(0.0f);
        img2.canvasRenderer.SetAlpha(0.0f);
        img3.canvasRenderer.SetAlpha(0.0f);
        img4.canvasRenderer.SetAlpha(0.0f);
        img5.canvasRenderer.SetAlpha(0.0f);
        img6.canvasRenderer.SetAlpha(0.0f);
        img7.canvasRenderer.SetAlpha(0.0f);
        FadeIn(img1);
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
                    StartCoroutine(Switch(img1,img2));
                    intro2.SetActive(true);
                    break;

                case 2:
                    curScreen++;
                    intro2.SetActive(false);
                    intro3.SetActive(true);
                    break;

                case 3:
                    curScreen++;
                    intro3.SetActive(false);
                    intro4.SetActive(true);
                    break;
                
                case 4:
                    curScreen++;
                    intro4.SetActive(false);
                    intro5.SetActive(true);
                    break;

                case 5:
                    curScreen++;
                    intro5.SetActive(false);
                    intro6.SetActive(true);
                    break;

                case 6:
                    curScreen++;
                    intro6.SetActive(false);
                    intro7.SetActive(true);
                    break;

                default:
                    // Set the value this way so if any changes are made they are accounted for
                    string scene = GameController.Instance.gameStateManager.MatchScene(GameStateManager.Scene.Landing);
                    GameController.Instance.sceneTransitionManager.devControl = true;
                    GameController.Instance.sceneTransitionManager.OnInitiateTransition(scene);
                    break;
            }
        }
    }

    IEnumerator Switch(Image pic1, Image pic2)
    {
        pic2.canvasRenderer.SetAlpha(0.0f);

        FadeOut(pic1);
        yield return new WaitForSeconds(2.5f);

        FadeIn(pic2);
        yield return new WaitForSeconds(2.5f);
    }

    void FadeIn(Image pic)
    {
        pic.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut(Image pic)
    {
        pic.CrossFadeAlpha (0.0f, 2.5f, false);  
    }
}
