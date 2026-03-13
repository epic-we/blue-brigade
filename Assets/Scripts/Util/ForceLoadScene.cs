using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceLoadScene : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadRevolution();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadFirstScene();
        }
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadScene("TuginhasFinalGame");
    }

    public void LoadRevolution()
    {
        SceneManager.LoadScene("REVOLUTION");
    }
}
