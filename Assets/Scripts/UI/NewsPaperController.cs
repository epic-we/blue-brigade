using UnityEngine;

public class NewsPaperController : MonoBehaviour
{
    [SerializeField] private KeyCode toggleNewspaper = KeyCode.Tab;
    [SerializeField] private GameObject newsPaper;

    [SerializeField] private AudioClip[] openJournal;
    [SerializeField] private AudioClip[] closeJournal;

    private LevelManager levelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(levelManager.isRunning)
        {
            newsPaper.SetActive(true);
        }
        else if (levelManager.isRunning == false)
        {
            newsPaper.SetActive(false);
        }*/
    }
}
