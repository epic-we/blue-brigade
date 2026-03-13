using UnityEngine;

public class DeactivateOnKeyPress : MonoBehaviour
{
    void Update()
    {
        // Check for Escape or Space key press
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            // Deactivate the current GameObject
            gameObject.SetActive(false);
        }
    }
}
