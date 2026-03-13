using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class TMPDropdownArrowByExpandedState : MonoBehaviour
{
    [SerializeField] private Image arrowImage;
    [SerializeField] private Sprite openSprite;

    private TMP_Dropdown dropdown;
    private Sprite normalSprite;
    private bool lastExpanded;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        if (arrowImage == null)
        {
            Debug.LogError("[TMPDropdownArrowByExpandedState] Arrow Image not assigned.", this);
            enabled = false;
            return;
        }

        normalSprite = arrowImage.sprite;
        lastExpanded = dropdown.IsExpanded;

        // 🔥 Instant reset when user selects an option (removes the close delay)
        dropdown.onValueChanged.AddListener(_ => SetClosedImmediate());
    }

    private void Update()
    {
        bool expanded = dropdown.IsExpanded;
        if (expanded == lastExpanded) return;

        lastExpanded = expanded;

        if (expanded)
        {
            if (openSprite != null) arrowImage.sprite = openSprite;
        }
        else
        {
            arrowImage.sprite = normalSprite;
        }
    }

    private void SetClosedImmediate()
    {
        lastExpanded = false;
        arrowImage.sprite = normalSprite;
    }
}