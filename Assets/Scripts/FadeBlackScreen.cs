using LocalizationSystem.Core;
using System.Collections;
using TMPro;
using UnityEngine;

public class FadeBlackScreen : MonoBehaviour
{

    [SerializeField] private GameObject fadeMask;
    private Vector3 initialScale;
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField]
    private Instructions instructions;
    [SerializeField]
    private GameObject introduction;
    [SerializeField]
    private GameObject policeman;
    [SerializeField]
    private PauseMenuController pauseMenu;
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField] 
    private LevelManager levelManager;
    [SerializeField]
    private GameObject[] mainMenuNPCS;
    


    private bool inIntro = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = fadeMask.transform.localScale;

        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inIntro)
        {
            inIntro = false;
            introduction.SetActive(false);

            if (levelManager != null)
                levelManager.SetLogosCanvasActive(false);

            instructions.gameObject.SetActive(true);
            instructions.ActiveFromIntro(true);
            Fade(true);
        }
    }

    public void PolicemanFade()
    {
        StartCoroutine(PolicemanFadeCoroutine());
    }

    public void Fade(bool fadeIn)
    {
        if(fadeIn) StartCoroutine(FadeIn());
        else StartCoroutine(FadeOut());
    }

    public void SetDay(string day, bool toggle)
    {
        dayText.gameObject.SetActive(toggle);
        if (LocalizationManager.Language == Language.en)
        {
            dayText.text = day + " April 1974";
        }
        else if (LocalizationManager.Language == Language.pt_pt)
        {
            dayText.text = day + " Abril 1974";
        }

    }

    public IEnumerator FadeIn()
    {
        float lerpValue = 0f;

        Vector3 currentScale = fadeMask.transform.localScale;

        while(lerpValue < 1f)
        {
            lerpValue += Time.unscaledDeltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, Vector3.zero, lerpValue);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.1f);

        float lerpValue = 0f;

        Vector3 currentScale = fadeMask.transform.localScale;

        while (lerpValue < 1f)
        {
            lerpValue += Time.unscaledDeltaTime / fadeSpeed;

            fadeMask.transform.localScale = Vector3.Lerp(currentScale, initialScale, lerpValue);
            yield return null;
        }
    }

    private IEnumerator PolicemanFadeCoroutine()
    {
        Fade(false);
        yield return new WaitForSecondsRealtime(1.2f);
        playerMovement.DelayStartMovement();
        policeman.SetActive(false);
        Fade(true);
        yield return new WaitForSecondsRealtime(1.2f);
        pauseMenu.CanPause = true;
    }

    public void InIntroduction(bool intro)
    {
        introduction.SetActive(intro);
        StartCoroutine(IntroCoroutine(intro));
    }

    private IEnumerator IntroCoroutine(bool intro)
    {
        yield return new WaitForSeconds(0.5f);
        inIntro = intro;
    }

    public void DisableMainMenuChars()
    {
        StartCoroutine(DisableMainMenuCharsCoroutine());
    }

    private IEnumerator DisableMainMenuCharsCoroutine()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject npc in mainMenuNPCS)
        {
            npc.SetActive(false);
        }
    }
}
