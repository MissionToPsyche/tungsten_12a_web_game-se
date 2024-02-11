using UnityEngine;

/// <summary>
/// Basic Intro Cutscene that shows a series of images
/// </summary>
/// Author: jmolive8
public class IntroController : MonoBehaviour
{
    public GameObject intro1, intro2, intro3, intro4, intro5;

    int curScreen = 1;

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
                    intro1.SetActive(false);
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

                default:
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Landing_Scene");
                    break;
            }
        }
    }
}
