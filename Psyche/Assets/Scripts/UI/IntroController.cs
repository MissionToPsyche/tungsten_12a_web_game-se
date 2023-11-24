using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public GameObject intro1, intro2, intro3, intro4;

    int curScreen = 1;

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

                default:
                    SceneManager.LoadScene("SampleScene");
                    break;
            }
        }
    }
}
