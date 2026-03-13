using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Settings")]
    [Tooltip("Assign a Texture2D for the cursor")]
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    private void Awake()
    {
        // Persist across scenes
        DontDestroyOnLoad(gameObject);

        // Set the cursor immediately
        SetCursor();
    }

    private void Start()
    {
        // Optional: reset cursor on scene start
        SetCursor();
    }

    private void SetCursor()
    {
        if (cursorTexture != null)
        {
            // Automatically set hotspot to center of texture
            Vector2 hotspot = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);
            Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        }
        else
        {
            Debug.LogWarning("Cursor texture not assigned!");
        }
    }
}
