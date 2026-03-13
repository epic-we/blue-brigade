using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsBlueBrigade : MonoBehaviour
{
    [SerializeField]
    private FadeBlackScreen fadeBlackScreen;

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ReturnDelay(0));
        }*/
    }

    private IEnumerator ReturnDelay(int scene)
    {
        fadeBlackScreen.Fade(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void ChangeScene(int scene)
    {
        StartCoroutine(ReturnDelay(scene));
    }
}
