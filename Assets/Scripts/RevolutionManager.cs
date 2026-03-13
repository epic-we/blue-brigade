using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevolutionManager : MonoBehaviour
{
    [SerializeField]
    private float timeToEnd = 30f;

    private FadeBlackScreenAfterRevolution fade;

    private bool skipped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fade = FindAnyObjectByType<FadeBlackScreenAfterRevolution>();

        StartCoroutine(EndTimeCoroutine());
    }

    private IEnumerator EndTimeCoroutine()
    {
        yield return new WaitForSeconds(timeToEnd);
        fade.FadeOut();
    }

    public void EndGame()
    {
        SceneManager.LoadScene(4);
    }

    public void SkipRevolution()
    {
        if (skipped) return;

        fade.FadeOut();
        skipped = true;
    }
}
