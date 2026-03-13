using System.Collections;
using UnityEngine;

public class ApplicationCloser : MonoBehaviour
{

    
    public void CloseGame()
    {
        StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        #if UNITY_EDITOR
                // If in Unity Editor, stop play mode
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                // If not in the Editor (i.e., in a built application), quit the application
                Application.Quit();
        #endif
    }
}