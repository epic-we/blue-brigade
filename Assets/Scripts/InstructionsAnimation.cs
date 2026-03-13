using UnityEngine;
using DG.Tweening;

public class InstructionsAnimation : MonoBehaviour
{
    [Header("Positions & Animation")]
    public Vector3 offScreenPos = new Vector3(0, -800, 0);
    public Vector3 onScreenPos = new Vector3(0, 0, 0);
    public float moveDuration = 0.5f;
    public float overshoot = 30f;

    [Header("Panel to Activate")]
    [SerializeField] private GameObject panelToActivate;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.localPosition = offScreenPos;
        if (panelToActivate != null)
            panelToActivate.SetActive(false);
    }

    public void OpenInstructions()
    {
        if (panelToActivate != null)
            panelToActivate.SetActive(true);

        rect.localPosition = offScreenPos;

        rect.DOLocalMoveY(onScreenPos.y + overshoot, moveDuration / 2f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                rect.DOLocalMoveY(onScreenPos.y, moveDuration / 2f)
                    .SetUpdate(true);
            });
    }

    public void CloseInstructions()
    {
        rect.DOLocalMoveY(onScreenPos.y + overshoot, moveDuration / 2f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                rect.DOLocalMoveY(offScreenPos.y, moveDuration / 2f)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        if (panelToActivate != null)
                            panelToActivate.SetActive(false);
                    });
            });
    }
}
