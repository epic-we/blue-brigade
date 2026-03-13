using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverDelayed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover settings")]
    public float moveAmount = 10f;
    public float returnDelay = 0.05f;

    [SerializeField]
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private bool isHovered = false;
    private bool returnScheduled = false;
    private float returnTime;

    void Awake()
    {
        //rectTransform = GetComponentInChildren<RectTransform>();
        originalPosition = rectTransform.localPosition;
    }

    void Update()
    {
     /*  if (isHovered)
        {
            // Force instantly UP while hovering
            rectTransform.localPosition = originalPosition + Vector3.up * moveAmount;

            // Cancel scheduled return
            returnScheduled = false;
            return;
        }

        // If a return is scheduled, check timer
        if (returnScheduled && Time.unscaledTime >= returnTime)
        {
            rectTransform.localPosition = originalPosition;
            returnScheduled = false;
        }*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localPosition = originalPosition + Vector3.up * moveAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localPosition = originalPosition;
    }

    public void ResetPosition()
    {
        rectTransform.localPosition = originalPosition;
    }
}
