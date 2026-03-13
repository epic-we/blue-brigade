using System.Collections;
using TMPro;
using UnityEngine;

public class FadeBlackScreenAfterRevolution : MonoBehaviour
{

    [SerializeField] private GameObject fadeMask;
    [SerializeField] private Vector3 finalScale;
    [SerializeField] private float fadeSpeed = 1f;

    private RevolutionManager revolutionManager;

    private void Start()
    {
        revolutionManager = FindAnyObjectByType<RevolutionManager>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float lerpValue = 0f;

        Vector3 currentScale = Vector3.zero;

        while (lerpValue < 1f)
        {
            lerpValue += Time.deltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, finalScale, lerpValue);
            yield return null;
        }

        if (lerpValue >= 1f)
        {
            revolutionManager.EndGame();
        }
    }
}
