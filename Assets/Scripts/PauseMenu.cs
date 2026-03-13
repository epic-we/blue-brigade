using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PauseMenuBackgroundAnimation backgroundAnim;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject gameUI; // in-game UI
    [SerializeField] private MainMenuState mainMenuState; // main menu manager
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject optionsBackToMenu;
    [SerializeField]
    private GameObject optionsBackToPause;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private GameObject Instructions;
    [SerializeField]
    private GameObject InstructionsReturn;
    [SerializeField]
    private GameObject confirmationPopup;
    [SerializeField]
    private AudioClip hitClip;
    [SerializeField]
    private ButtonHoverDelayed backHover;

    [Header("PlayerPrefs")]
    [SerializeField]
    private string PlayerPrefsVolumeKey = "MasterVolumeValue";
    [SerializeField]
    private string PlayerPrefsSFXKey = "MasterSFXValue";

    // Music
    [SerializeField] 
    private Image musicHandleImage;
    [SerializeField] 
    private Sprite musicMuteSprite;
    [SerializeField] 
    private Sprite musicLowSprite;
    [SerializeField] 
    private Sprite musicMidSprite;
    [SerializeField] 
    private Sprite musicHighSprite;

    // SFX
    [SerializeField] 
    private Image sfxHandleImage;
    [SerializeField] 
    private Sprite sfxMuteSprite;
    [SerializeField] 
    private Sprite sfxLowSprite;
    [SerializeField] 
    private Sprite sfxMidSprite;
    [SerializeField] 
    private Sprite sfxHighSprite;

    private PlayerMovement playerMov;
    private DetectNpcs detectNPCs;

    private bool isPaused = false;

    private bool canEsc = true;

    public bool CanPause { get; set; } = true;

    private void Start()
    {
        playerMov = FindAnyObjectByType<PlayerMovement>();
        detectNPCs = FindAnyObjectByType<DetectNpcs>();

        float savedVolume = PlayerPrefs.GetFloat(PlayerPrefsVolumeKey, 1f);
        float savedSFX = PlayerPrefs.GetFloat(PlayerPrefsSFXKey, 1f);

        musicSlider.value = savedVolume;
        sfxSlider.value = savedSFX;

        ChangeMusicVolume();
        ChangeSFXVolume();
    }

    void Update()
    {
        if (levelManager != null && levelManager.isRunning)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && CanPause)
            {
                if (isPaused && canEsc)
                {
                    OpenOptions(false);
                    ResumeGame();
                }
                else if (!isPaused)
                {
                    PauseGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        playerMov.CanPlaySound = false;
        isPaused = true;
        Time.timeScale = 0f;
        backgroundAnim.OpenBackground();

        // hide in-game UI while paused
        if (gameUI != null)
        {
            gameUI.SetActive(false);
        }
    }

    public void OpenOptions(bool open)
    {
        optionsMenu.SetActive(open);
        backHover.ResetPosition();

        if (open == true)
        {
            optionsBackToMenu.SetActive(false);
            optionsBackToPause.SetActive(true);
        }
        else
        {
            if (optionsBackToMenu.activeSelf == true)
            {
                EnablePlayerMov(true);
            }
        }
    }

    public void OpenOptionsFromMainMenu()
    {
        StartCoroutine(OpenOptionsFromMainMenuCoroutine());
    }

    private IEnumerator OpenOptionsFromMainMenuCoroutine()
    {
        yield return new WaitForSeconds(0.15f);
        EnablePlayerMov(false);
        optionsMenu.SetActive(true);
    }

    public void EnablePlayerMov(bool enable)
    {
        detectNPCs.CanInteract(enable);
        playerMov.StartMoving(enable);
        playerMov.CanPlaySound = enable;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        backgroundAnim.CloseBackground();

        playerMov.CanPlaySound = true;

        // restore in-game UI
        if (gameUI != null)
        {
            gameUI.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        ResumeGame(); // ensure unpaused
        SceneManager.LoadScene(0);

        /*if (levelManager != null)
        {
            levelManager.ResetToMainMenu(); // stop level, deactivate isRunning, reset all
        }

        if (mainMenuState != null)
        {
            mainMenuState.RestoreState(); // reactivate main menu civillians
        }

        if (gameUI != null)
            gameUI.SetActive(false); // deactivate player UI*/
    }

    public void OpenConfirmationPopup(bool open)
    {
        confirmationPopup.SetActive(open);
    }

    private void UpdateHandleSprite(float value, Image handleImage, Sprite mute, Sprite low, Sprite mid, Sprite high)
    {
        if (value <= 0.10f)
            handleImage.sprite = mute;
        else if (value <= 0.43f)
            handleImage.sprite = low;
        else if (value <= 0.76f)
            handleImage.sprite = mid;
        else
            handleImage.sprite = high;
    }
    public void ChangeMusicVolume()
    {
        float value = musicSlider.value;

        // Protect against log(0)
        if (value <= 0.0001f) value = 0.0001f;

        float dB = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);

        PlayerPrefs.SetFloat(PlayerPrefsVolumeKey, value);

        UpdateHandleSprite(value, musicHandleImage, musicMuteSprite, musicLowSprite, musicMidSprite, musicHighSprite);
    }

    public void ChangeSFXVolume()
    {
        float value = sfxSlider.value;

        // Protect against log(0)
        if (value <= 0.0001f) value = 0.0001f;

        float dB = Mathf.Log10(value) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);

        PlayerPrefs.SetFloat(PlayerPrefsSFXKey, value);

        UpdateHandleSprite(value, sfxHandleImage, sfxMuteSprite, sfxLowSprite, sfxMidSprite, sfxHighSprite);
    }

    public void OpenInstructions()
    {
        Instructions.SetActive(true);
        InstructionsReturn.SetActive(true);
        canEsc = false;
    }

    public void PlayHitSFX()
    {
        playerMov.gameObject.GetComponent<AudioSource>().PlayOneShot(hitClip);
    }

    public void CanEscape()
    {
        canEsc = true;
    }
}
