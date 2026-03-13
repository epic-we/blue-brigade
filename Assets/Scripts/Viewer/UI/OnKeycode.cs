using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OnKeycode : MonoBehaviour
{
    [SerializeField] private KeyCode _keycode;
    public UnityEvent _unityEvent;
    
    private void Update()
    {
        if (Input.GetKeyDown(_keycode))
        {
            SceneManager.LoadScene(0);
        }
    }
}
