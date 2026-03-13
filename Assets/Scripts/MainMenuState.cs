using UnityEngine;

public class MainMenuState : MonoBehaviour
{
    [Header("Main Menu Civillians")]
    [SerializeField] private GameObject startCivilian;
    [SerializeField] private GameObject aboutCivilian;
    [SerializeField] private GameObject quitCivilian;

    [Header("Level Civillians that should appear in main menu")]
    [SerializeField] private GameObject musicianNPC;
    [SerializeField] private GameObject guardsNPCs;
    [SerializeField] private GameObject floristNPC;
    [SerializeField] private GameObject floristhouse;
    [SerializeField] private GameObject statue;
    [SerializeField] private LevelManager levelManager;

    public void RestoreState()
    {
        // Reactivate civillians
        if (startCivilian != null) startCivilian.SetActive(true);
        if (aboutCivilian != null) aboutCivilian.SetActive(true);
        if (quitCivilian != null) quitCivilian.SetActive(true);

        // Reactivate level NPCs
        if (musicianNPC != null) musicianNPC.SetActive(true);
        if (guardsNPCs != null) guardsNPCs.SetActive(true);
        if (floristNPC != null) floristNPC.SetActive(true);
        if (floristhouse != null) floristhouse.SetActive(true);
        if (statue != null) statue.SetActive(true);

        // Reset grayscale
        levelManager?.ResetGrayscale();
    }

}
