using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using TMPro;
using System.Collections;
using static LevelManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LocalizationSystem.Core;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public struct Level
    {
        public int numberOfPeople;
        public int numberOfFaults;
        public float spawnRate;
        public float timer;

        public int badBooksNumber;
        public int badTopicsNumber;
        public int badHatsNumber;
        public bool badSinging;
        public bool badRadio;
        public bool badFootball;

        public int maximumAnomaliesActive;

        public bool specialLevel;
        public int initialChance;
        public int chanceIncrease;

        [Range(0, 1)]
        public float initialGreyscale;
        public Sprite levelCensorBar;

        [TextArea(3, 10)]
        public string levelText;
        [TextArea(3, 10)]
        public string levelTextPT;

        // ✅ NOVO: Y por idioma (EN / PT)
        public float imagesY_EN;
        public float imagesY_PT;

        public bool imagesActive;
    }

    [SerializeField] private FaultManager faultManager;
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject logosCanvas;
    [SerializeField] private Image censorBar;

    [SerializeField] private Level[] levels;
    private int currentLevel;
    private int numberOfPeopleSpawned;
    private List<int> indexOfFaults;
    private List<int> availableSlots;
    private static System.Random random = new System.Random();

    private bool spawning = false;

    private float justSpawned = 0f;

    [SerializeField] private CreateTopics createTopicsScript;

    [SerializeField] private TextMeshProUGUI timerText;
    public bool isRunning = false;
    private float timeRemaining = 0f;

    [SerializeField] private CivilianBrain civilianBrainScript;

    private List<CivilianFaultType> faultTypes;

    [SerializeField] private FadeBlackScreen fadeScreen;

    public int anomaliesCount = 0;

    [SerializeField] private GameObject pointerPivot;
    [SerializeField] private Animator watchAnimator;

    [SerializeField] private TextMeshProUGUI reasonTextUI;

    //[SerializeField] private GameObject[] tabUI;

    private bool restarted = false;

    public bool revolution = false;

    public void SetLogosCanvasActive(bool active)
    {
        if (logosCanvas != null)
            logosCanvas.SetActive(active);
    }

    [SerializeField] private ForceLoadScene forceLoadScene;

    [SerializeField]
    private GameObject musicianNPC;
    [SerializeField]
    private GameObject guardsNPCs;
    [SerializeField]
    private GameObject floristNPC;
    [SerializeField]
    private GameObject floristcloveNPC;
    [SerializeField]
    private GameObject floristhouse;
    [SerializeField]
    private GameObject floristclovehouse;
    [SerializeField]
    private GameObject statue;
    [SerializeField]
    private GameObject clovestatue;
    [SerializeField]
    private GameObject groundHitParticles;
    [SerializeField]
    private Material spritesGreyscaleMaterial;
    [SerializeField]
    private Material groundGreyscaleMaterial;

    private float specialLevelGreyscale = 1f;

    private bool specialDayRevolutionApproaching = false;

    [SerializeField]
    private PlayerMovement playerMov;

    private int newTopics = 0;
    private int newBooks = 0;
    private int newHats = 0;
    private int newSinging = 0;
    private int newRadio = 0;

    public bool SpecialLevel
    {
        get
        {
            return levels[currentLevel].specialLevel;
        }
    }

    [SerializeField]
    private GameObject policeman;
    [SerializeField]
    private TextMeshProUGUI policemanText;
    [SerializeField]
    private RectTransform[] levelImages;
    [SerializeField]
    private Image[] policemanCensorItems;

    [SerializeField]
    private AudioClip[] letterSounds;

    [SerializeField]
    private PauseMenuController pauseMenu;

    private float clovesChance = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableSlots = new List<int>();
        indexOfFaults = new List<int>();
        faultTypes = new List<CivilianFaultType>();

        spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", 0);
        groundGreyscaleMaterial.SetFloat("_Greyscale", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (revolution)
            return;

        if (spawning && Time.time - justSpawned > levels[currentLevel].spawnRate)
        {

            if (levels[currentLevel].specialLevel)
            {
                createTopicsScript.IncreaseCloves();
                specialDayRevolutionApproaching = true;
                if ((anomaliesCount / levels[currentLevel].numberOfFaults > 0.5 || (levels[currentLevel].timer - timeRemaining) / levels[currentLevel].timer > 0.5) && !specialDayRevolutionApproaching)
                {
                    Debug.Log("revolution Started");
                }

                int chanceOfspawn = UnityEngine.Random.Range(0, 100);

                if (chanceOfspawn < clovesChance)
                {
                    CivilianFaultType randomFault = (CivilianFaultType)UnityEngine.Random.Range(3, 5);
                    civilianBrainScript.CreateNewCivilian(randomFault);
                    faultManager.AddFault(specialDayRevolutionApproaching);
                }
                else
                {
                    //Debug.Log("Good Civillian Spawned");
                    civilianBrainScript.CreateNewCivilian(CivilianFaultType.None);
                }

                clovesChance += levels[currentLevel].chanceIncrease;
            }
            else
            {
                if (numberOfPeopleSpawned >= levels[currentLevel].numberOfPeople)
                {
                    spawning = false;
                }
                else
                {
                    if (indexOfFaults.Contains(numberOfPeopleSpawned))
                    {
                        indexOfFaults.Remove(numberOfPeopleSpawned);
                        // Spawn Bad Civillian
                        //Debug.Log("Bad Civillian Spawned");
                        CivilianFaultType randomFault = PriorityFaultType();
                        civilianBrainScript.CreateNewCivilian(randomFault);
                        faultManager.AddFault(false);
                    }
                    else
                    {
                        // Spawn Good Civillian
                        //Debug.Log("Good Civillian Spawned");
                        civilianBrainScript.CreateNewCivilian(CivilianFaultType.None);
                    }
                }
            }

            numberOfPeopleSpawned++;
            justSpawned = Time.time;

        }

        if (anomaliesCount > levels[currentLevel].maximumAnomaliesActive)
        {
            if (levels[currentLevel].specialLevel)
            {
                StartCoroutine(LoadRevolutionCR());
                createTopicsScript.revolutionObject.SetActive(true);
                revolution = true;
            }
            else
            {
                reasonTextUI.text = "Too many suspects active...";
                RestartDay(false);
            }

        }
        Debug.Log("index:" + indexOfFaults.Count);
        Debug.Log("anomalies:" + anomaliesCount);

        if (indexOfFaults.Count <= 0 && anomaliesCount <= 0 && isRunning)
        {
            isRunning = false;
            EndDay();
            Debug.Log("heree");
        }

        if (isRunning == false && anomaliesCount > 0 && !restarted && levels[currentLevel].specialLevel)
        {
            StartCoroutine(LoadRevolutionCR());
            createTopicsScript.revolutionObject.SetActive(true);
            revolution = true;
        }
        else if (isRunning == false && anomaliesCount > 0 && !restarted)
        {
            restarted = true;
            //reasonTextUI.text = "Time ran out...";
            createTopicsScript.timeRanOutObject.SetActive(true);
            RestartDay(false);
        }

        if (isRunning)
        {
            UpdateTimer();
        }

        if (levels[currentLevel].specialLevel)
        {
            specialLevelGreyscale -= (1 / levels[currentLevel].timer) * Time.deltaTime * 2;

            if (specialLevelGreyscale < 0)
            {
                specialLevelGreyscale = 0;
            }

            spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", specialLevelGreyscale);
            groundGreyscaleMaterial.SetFloat("_Greyscale", specialLevelGreyscale);
        }

    }

    private CivilianFaultType PriorityFaultType()
    {
        if (specialDayRevolutionApproaching)
        {
            //Ignore singing and radio to force more cloves after a certain time on the special level
            return faultTypes[UnityEngine.Random.Range(0, faultTypes.Count - 2)];
        }
        else if (newTopics > 0)
        {
            newTopics--;
            return CivilianFaultType.Talking;
        }
        else if (newBooks > 0)
        {
            newBooks--;
            return CivilianFaultType.Item;
        }
        else if (newHats > 0)
        {
            newHats--;
            return CivilianFaultType.Fashion;
        }
        else if (newSinging > 0)
        {
            newSinging--;
            return CivilianFaultType.Singing;
        }
        else if (newRadio > 0)
        {
            newRadio--;
            return CivilianFaultType.Radio;
        }
        else
        {
            return faultTypes[UnityEngine.Random.Range(0, faultTypes.Count)];
        }
    }

    private IEnumerator LoadRevolutionCR()
    {
        yield return new WaitForSeconds(4.5f);
        forceLoadScene.LoadRevolution();
    }

    private void EndDay()
    {
        playerMov.StartMoving(false);
        playerMov.CanPlaySound = false;
        currentLevel++;
        createTopicsScript.goodJobObject.SetActive(true);
        StartCoroutine(EndLevelCoroutine());
    }

    public void RestartDay(bool died)
    {
        playerMov.StartMoving(false);

        if (died)
        {
            /*if(levels[currentLevel].specialLevel)
            {
                reasonTextUI.text = "";
                createTopicsScript.revolutionObject.SetActive(true);
                revolution = true;
            }
            else*/
            //reasonTextUI.text = "Stop censoring the innocent!";
            createTopicsScript.censoringInnocentsObject.SetActive(true);
        }
        StartCoroutine(EndLevelCoroutine());
    }

    void UpdateTimer()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease time
        }
        else if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
        }

        pointerPivot.transform.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0, 360, timeRemaining / levels[currentLevel].timer));

        if (timeRemaining / levels[currentLevel].timer <= 0.25)
        {
            watchAnimator.SetBool("TimeRunningOut", true);
        }
        else
        {
            watchAnimator.SetBool("TimeRunningOut", false);
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

    public void StartLevel(int level)
    {
        /*foreach (GameObject go in tabUI)
        {
            go.SetActive(false);
        }*/
        StartCoroutine(StartLevelCR(level));
    }

    private IEnumerator StartLevelCooldownCR(int level)
    {
        // Stop any spawning immediately
        spawning = false;
        isRunning = false;
        numberOfPeopleSpawned = 0;
        anomaliesCount = 0;

        // Clear civilians and reset nodes
        civilianBrainScript?.ClearCivillians();

        // Small delay to ensure everything is fully reset
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.05f);

        // Call your original StartLevel() function
        StartLevel(level);
    }

    public IEnumerator StartLevelCR(int level)
    {
        pauseMenu.CanPause = false;
        fadeScreen.Fade(false);
        fadeScreen.SetDay((20 + level).ToString(), true);
        playerMov.CanPlaySound = false;

        yield return new WaitForSeconds(1f);

        FaultManager.Instance.ResetFaults(levels[level].numberOfFaults);

        if (level == 0)
        {
            createTopicsScript.InitializeCensorshipItems();
        }

        civilianBrainScript.ClearCivillians();
        createTopicsScript.badgeCover1.SetActive(false);
        createTopicsScript.badgeCover2.SetActive(false);
        createTopicsScript.badgeCover3.SetActive(false);

        if (currentLevel > 0)
        {
            newTopics = levels[currentLevel].badTopicsNumber - levels[currentLevel - 1].badTopicsNumber;
            newBooks = levels[currentLevel].badBooksNumber - levels[currentLevel - 1].badBooksNumber;
            newHats = levels[currentLevel].badHatsNumber - levels[currentLevel - 1].badHatsNumber;

            if (levels[currentLevel].badSinging && !levels[currentLevel - 1].badSinging)
            {
                newSinging = 1;
            }

            if (levels[currentLevel].badRadio && !levels[currentLevel - 1].badRadio)
            {
                newRadio = 1;
            }
        }

        yield return new WaitForSeconds(2f);

        uiObject.SetActive(true);
        logosCanvas.SetActive(false);
        if (level == 0)
        {
            spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", 0f);
            groundGreyscaleMaterial.SetFloat("_Greyscale", 0f);
            specialLevelGreyscale = 1f;
        }
        else
        {
            spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", levels[currentLevel].initialGreyscale);
            groundGreyscaleMaterial.SetFloat("_Greyscale", levels[currentLevel].initialGreyscale);
            specialLevelGreyscale = levels[currentLevel].initialGreyscale;
        }

        groundHitParticles.SetActive(false);
        groundHitParticles.SetActive(true);
        if (levels[currentLevel].specialLevel)
        {
            specialLevelGreyscale = levels[currentLevel].initialGreyscale;
            musicianNPC.SetActive(true);
            floristcloveNPC.SetActive(true);
            floristclovehouse.SetActive(true);
            clovestatue.SetActive(true);
            floristNPC.SetActive(false);
            guardsNPCs.SetActive(false);
            floristhouse.SetActive(false);
            statue.SetActive(false);

            clovesChance = levels[currentLevel].initialChance;
        }
        else
        {
            musicianNPC.SetActive(false);
            guardsNPCs.SetActive(true);
            floristcloveNPC.SetActive(false);
            floristNPC.SetActive(true);
            floristclovehouse.SetActive(false);
            clovestatue.SetActive(false);
            floristhouse.SetActive(true);
            statue.SetActive(true);
        }

        censorBar.sprite = levels[level].levelCensorBar;

        policeman.SetActive(true);
        policeman.GetComponent<PolicemanIntroduction>().CanProgress = true;

        if (LocalizationManager.Language == Language.en)
        {
            policemanText.text = levels[level].levelText;
        }
        else if (LocalizationManager.Language == Language.pt_pt)
        {
            policemanText.text = levels[level].levelTextPT;
        }

        StartCoroutine(LetterSoundCoroutine());

        // ✅ NOVO: escolher Y com base na língua
        float targetY =
            (LocalizationManager.Language == Language.pt_pt)
                ? levels[level].imagesY_PT
                : levels[level].imagesY_EN;

        for (int i = 0; i < levelImages.Length; i++)
        {
            // Set Y position
            Vector3 pos = levelImages[i].localPosition;
            pos.y = targetY;
            levelImages[i].localPosition = pos;

            // Set opacity
            Image img = levelImages[i].GetComponent<Image>();
            Color c = img.color;
            c.a = levels[level].imagesActive ? 1f : 0f;
            img.color = c;
        }

        fadeScreen.Fade(true);

        if (levels[level].specialLevel)
            createTopicsScript.ActivateSpecialDay();

        numberOfPeopleSpawned = 0;

        faultTypes.Clear();
        indexOfFaults.Clear();
        specialDayRevolutionApproaching = false;

        InitalizeAvailableSpots(levels[level].numberOfPeople);

        for (int i = 0; i < levels[level].numberOfFaults; i++)
        {
            indexOfFaults.Add(availableSlots[i]);
        }

        if (currentLevel == 0)
        {
            createTopicsScript.SetNumberOfFaultTypes(levels[level].badTopicsNumber,
                                                    newTopics,
                                                    levels[level].badHatsNumber,
                                                    newHats,
                                                    levels[level].badBooksNumber,
                                                    newBooks,
                                                    levels[level].badSinging,
                                                    newSinging,
                                                    levels[level].badRadio,
                                                    newRadio,
                                                    levels[level].badFootball,
                                                    true
            );
        }
        else
        {
            createTopicsScript.SetNumberOfFaultTypes(levels[level].badTopicsNumber,
                                                    newTopics,
                                                    levels[level].badHatsNumber,
                                                    newHats,
                                                    levels[level].badBooksNumber,
                                                    newBooks,
                                                    levels[level].badSinging,
                                                    newSinging,
                                                    levels[level].badRadio,
                                                    newRadio,
                                                    levels[level].badFootball,
                                                    false
            );
        }

        createTopicsScript.CreateNewTopics();
        createTopicsScript.CreateNewHats();
        createTopicsScript.CreateNewBooks();

        if (levels[level].badTopicsNumber > 0)
            faultTypes.Add(CivilianFaultType.Talking);

        if (levels[level].badHatsNumber > 0)
            faultTypes.Add(CivilianFaultType.Fashion);

        if (levels[level].badBooksNumber > 0)
            faultTypes.Add(CivilianFaultType.Item);

        if (levels[level].badSinging)
        {
            createTopicsScript.CreateSinging(true);
            faultTypes.Add(CivilianFaultType.Singing);
        }
        else
        {
            createTopicsScript.CreateSinging(false);
        }

        if (levels[level].badRadio)
        {
            createTopicsScript.CreateRadio(true);
            faultTypes.Add(CivilianFaultType.Radio);
        }
        else
        {
            createTopicsScript.CreateRadio(false);
        }

        createTopicsScript.UpdateCensorship(currentLevel);

        spawning = true;

        isRunning = true;
        timeRemaining = levels[level].timer;

        restarted = false;
        anomaliesCount = 0;
        Time.timeScale = 0;
        //playerMov.StartMoving(true);
        //playerMov.CanPlaySound = true;
        playerMov.ResetPosition();
        faultManager.ResetCarnations();
    }

    private IEnumerator LetterSoundCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        PlayLetterSound();
    }

    public void PlayLetterSound()
    {
        AudioSystem.PlaySound(letterSounds);
    }

    private void InitalizeAvailableSpots(int amount)
    {
        availableSlots.Clear();

        for (int i = 0; i < amount; i++)
        {
            availableSlots.Add(i);
        }

        Shuffle(availableSlots);
    }

    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int swapIndex = random.Next(0, i + 1);
            (list[i], list[swapIndex]) = (list[swapIndex], list[i]);
        }
    }

    private IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        playerMov.CanPlaySound = false;
        yield return new WaitForSeconds(2.65f);
        StartLevel(currentLevel);
    }

    public void ResetGrayscale()
    {
        if (spritesGreyscaleMaterial != null)
            spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", 0f);

        if (groundGreyscaleMaterial != null)
            groundGreyscaleMaterial.SetFloat("_Greyscale", 0f);

        specialLevelGreyscale = 1f; // optional: reset internal variable
    }

    // --- RETURN TO MAIN MENU (without purge flag) ---
    public void ResetToMainMenu()
    {
        // Stop level
        isRunning = false;
        spawning = false;
        restarted = false;
        anomaliesCount = 0;
        numberOfPeopleSpawned = 0;
        timeRemaining = 0f;

        // Deactivate UI
        if (uiObject != null) uiObject.SetActive(false);
        if (logosCanvas != null) logosCanvas.SetActive(true);

        // Deactivate NPCs and objects
        if (musicianNPC != null) musicianNPC.SetActive(false);
        if (guardsNPCs != null) guardsNPCs.SetActive(false);
        if (floristNPC != null) floristNPC.SetActive(false);
        if (floristcloveNPC != null) floristcloveNPC.SetActive(false);
        if (floristhouse != null) floristhouse.SetActive(false);
        if (floristclovehouse != null) floristclovehouse.SetActive(false);
        if (statue != null) statue.SetActive(false);
        if (clovestatue != null) clovestatue.SetActive(false);

        // Remove all spawned civilians
        civilianBrainScript?.ClearCivillians();

        // Reset player movement for main menu
        if (playerMov != null)
            playerMov.StartMoving(true);
    }

    public void LoadAboutScene()
    {
        StartCoroutine(LoadSceneDelay(2));
    }

    public void LoadCreditsScene()
    {
        StartCoroutine(LoadSceneDelay(3));
    }

    private IEnumerator LoadSceneDelay(int scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }
}