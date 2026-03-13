using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FaultManager : MonoBehaviour
{
    private int faultCounter = 0;

    [Header("Possible Text When Censoring")]
    [SerializeField] private string[] textInnocent;
    [SerializeField] private string[] textGroup;
    [SerializeField] private string[] textTalking;
    [SerializeField] private string[] textFashion;
    [SerializeField] private string[] textItem;

    [Header("UI")]
    private int currentLine = 0;
    [SerializeField] private Slider faultSlider;
    [SerializeField] private Animator carnationAnimation;

    [SerializeField] private Animator newCarnations;
    private bool phase1 = false;
    private bool phase2 = false;
    private bool phase3 = false;

    [SerializeField] private LevelManager levelManager;

    public static FaultManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void FixedUpdate()
    {
        if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 3 && phase1 == false)
        {
            phase1 = true;
            newCarnations.gameObject.SetActive(true);
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 3 && phase1 == true)
        {
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 6 && phase1 && !phase2)
        {
            phase2 = true;
            newCarnations.SetTrigger("Phase2");
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 6 && phase1 && phase2)
        {
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 9 && phase2 && !phase3)
        {
            phase3 = true;
            newCarnations.SetTrigger("Phase3");
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 9 && phase2 && phase3)
        {
            return;
        }
    }

    public void ResetFaults(int maxFaults)
    {

        /*foreach(TextMeshProUGUI text in noteBookLines)
        {
            text.text = "";
        }*/
        currentLine = 0;
        faultCounter = 0;
        //levelManager.anomaliesCount = faultCounter;
        faultSlider.value = faultCounter;
        faultSlider.maxValue = maxFaults;
    }

    public void AddFault(bool specialDayRevolutionApproaching)
    {
        faultCounter++;
        levelManager.anomaliesCount = faultCounter;

        if (specialDayRevolutionApproaching)
        {
            float sliderValue = faultSlider.maxValue - faultCounter;
            if (sliderValue < faultSlider.minValue)
            {
                sliderValue = faultSlider.minValue;
            }

            if (faultSlider.value > sliderValue)
            {
                faultSlider.value = sliderValue;
                //carnationAnimation.gameObject.SetActive(true);
                //carnationAnimation.SetTrigger("CarnationSpawn");
            }
        }
    }

    public void RemoveFault()
    {
        faultSlider.GetComponent<Animator>().SetTrigger("SuccessfulCensor");
        faultCounter--;
        levelManager.anomaliesCount = faultCounter;

        if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 3 && phase1 == false)
        {
            //phase1 = true;
            //newCarnations.gameObject.SetActive(true);
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 3 && phase1 == true)
        {
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 6 && phase1 && !phase2)
        {
            //phase2 = true;
            //newCarnations.SetTrigger("Phase2");
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 6 && phase1 && phase2)
        {
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 9 && phase2 && !phase3)
        {
            //phase3 = true;
            //newCarnations.SetTrigger("Phase3");
            return;
        }
        else if (levelManager.SpecialLevel == true && faultSlider.value == faultSlider.maxValue - 9 && phase2 && phase3)
        {
            return;
        }

        faultSlider.value++;
    }

    public void ResetCarnations()
    {
        newCarnations.gameObject.SetActive(false);
        phase1 = false;
        phase2 = false;
        phase3 = false;
}

    public void ClearFault(CivilianFaultType faultType)
    {
        if (faultType == CivilianFaultType.None)
        {
            // Add badge censoring code
            return;
        }
            

        /*switch(faultType)
        {
            case CivilianFaultType.Group:
                noteBookLines[currentLine].text = textGroup[Random.Range(0, textGroup.Length)];
                break;
            case CivilianFaultType.Talking:
                noteBookLines[currentLine].text = textTalking[Random.Range(0, textTalking.Length)];
                break;

            case CivilianFaultType.Fashion:
                noteBookLines[currentLine].text = textFashion[Random.Range(0, textFashion.Length)];
                break;

            case CivilianFaultType.Item:
                noteBookLines[currentLine].text = textItem[Random.Range(0, textItem.Length)];
                break;

        }*/
        currentLine++;

        RemoveFault();

    }


}
