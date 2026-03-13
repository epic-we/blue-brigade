using LocalizationSystem.Core;
using UnityEngine;

public class LocalizationPlayerPrefs : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            LocalizationManager.Language = Language.en;
        }
        else if (PlayerPrefs.GetInt("Language") == 1)
        {
            LocalizationManager.Language = Language.pt_pt;
        }
    }
}
