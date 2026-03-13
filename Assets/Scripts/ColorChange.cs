using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownItemLabelColors : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Toggle toggle;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color selectedColor = Color.white; // set different if you want

    private bool isHovered;

    private void Awake()
    {
        if (label == null) label = GetComponentInChildren<TMP_Text>(true);
        if (toggle == null) toggle = GetComponent<Toggle>();

        if (toggle != null)
            toggle.onValueChanged.AddListener(_ => ApplyColor());

        ApplyColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        ApplyColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (label == null) return;

        if (isHovered) label.color = hoverColor;
        else if (toggle != null && toggle.isOn) label.color = selectedColor;
        else label.color = normalColor;
    }
}