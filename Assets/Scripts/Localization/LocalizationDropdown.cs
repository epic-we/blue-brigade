using LocalizationSystem.Core;
using LocalizationSystem.Integration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationDropdown : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            GetComponent<TMP_Dropdown>().value = 0;
        }
        else if (PlayerPrefs.GetInt("Language") == 1)
        {
            GetComponent<TMP_Dropdown>().value = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLanguage ()
    {
        if (GetComponent<TMP_Dropdown>().value == 0)
        {
            LocalizationManager.Language = Language.en;
            PlayerPrefs.SetInt("Language", 0);
        }
        else if (GetComponent<TMP_Dropdown>().value == 1)
        {
            LocalizationManager.Language = Language.pt_pt;
            PlayerPrefs.SetInt("Language", 1);
        }

        Debug.Log(LocalizationManager.Language);
    }
}
