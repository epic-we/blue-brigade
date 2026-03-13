using System.Collections;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;
    [SerializeField]
    private DetectNpcs detectNpcs;
    [SerializeField]
    private GameObject instructionsCivilian;
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private GameObject instructionsContinue;
    [SerializeField]
    private FadeBlackScreen fade;
    [SerializeField]
    private PauseMenuController pauseMenu;

    private bool activeFromMenu = false;
    private bool activeFromIntroduction = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.StartMoving(true);

            if (activeFromMenu)
            {
                activeFromMenu = false;
                instructionsCivilian.SetActive(true);
                detectNpcs.CanInteract(true);
            }

            if (activeFromIntroduction)
            {
                activeFromIntroduction = false;
                fade.FadeOut();
                levelManager.StartLevel(0);
                StartCoroutine(DeactivateCoroutine());
            }
            else
            {
                gameObject.SetActive(false);
                instructionsContinue.SetActive(false);
                pauseMenu.CanEscape();
            }
        }
    }

    public void ActiveFromMenu(bool active)
    {
        activeFromMenu = active;
    }

    public void ActiveFromIntro(bool active)
    {
        activeFromIntroduction = active;
    }

    private IEnumerator DeactivateCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
        instructionsContinue.SetActive(false);
    }
}
