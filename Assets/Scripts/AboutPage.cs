using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutPage : MonoBehaviour
{
    [SerializeField]
    private FadeBlackScreen fadeBlackScreen;

    private void Update()
    {
        /* (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ReturnDelay());
        }*/
    }

    private IEnumerator ReturnDelay(int scene)
    {
        fadeBlackScreen.Fade(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void MainMenu()
    {
        StartCoroutine(ReturnDelay(0));
    }

    public void Credits()
    {
        StartCoroutine(ReturnDelay(3));
    }
}
