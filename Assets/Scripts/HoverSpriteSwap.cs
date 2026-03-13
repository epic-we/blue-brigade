using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSpriteSwap : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("Sprites")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;

    [Header("Movement")]
    [SerializeField] private bool moveOnHover = false;
    [SerializeField] private Vector2 hoverOffset = new Vector2(20f, 0f);
    [SerializeField] private float moveSpeed = 10f;

    [Header("Click Scale")]
    [SerializeField] private bool scaleOnClick = false;
    [SerializeField] private float clickScaleMultiplier = 1.1f;
    [SerializeField] private float scaleSpeed = 12f;

    private RectTransform rectTransform;
    private Vector2 originalPos;
    private Vector3 originalScale;
    private bool isHovering;
    private bool isPressed;

    private void Awake()
    {
        rectTransform = targetImage.rectTransform;
        originalPos = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
    }

    private void Reset()
    {
        targetImage = GetComponent<Image>();
    }

    private void Update()
    {
        // Movement
        if (moveOnHover)
        {
            Vector2 targetPos = isHovering ? originalPos + hoverOffset : originalPos;
            rectTransform.anchoredPosition =
                Vector2.Lerp(rectTransform.anchoredPosition, targetPos, Time.unscaledDeltaTime * moveSpeed);
        }

        // Scale
        if (scaleOnClick)
        {
            Vector3 targetScale = isPressed ? originalScale * clickScaleMultiplier : originalScale;
            rectTransform.localScale =
                Vector3.Lerp(rectTransform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.sprite = hoverSprite;
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.sprite = normalSprite;
        isHovering = false;
        isPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
