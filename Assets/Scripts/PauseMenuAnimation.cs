using UnityEngine;
using DG.Tweening;

public class PauseMenuBackgroundAnimation : MonoBehaviour
{
    [Header("Positions & Animation")]
    public Vector3 offScreenPos = new Vector3(0, -800, 0); // background starts off-screen
    public Vector3 onScreenPos = new Vector3(0, 0, 0);    // final position
    public float moveDuration = 0.5f;
    public float overshoot = 30f; // moves slightly past then back

    [Header("Panel to Activate")]
    [SerializeField] private GameObject panelToActivate; // panel that activates

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.localPosition = offScreenPos; // start off-screen
    }

    public void OpenBackground()
    {
        // Activate the panel immediately
        if (panelToActivate != null)
            panelToActivate.SetActive(true);

        // Reset in case
        rect.localPosition = offScreenPos;

        // Tween up a little past final, then back to final
        rect.DOLocalMoveY(onScreenPos.y + overshoot, moveDuration / 2f)
            .SetUpdate(true) // ignore Time.timeScale
            .OnComplete(() =>
            {
                rect.DOLocalMoveY(onScreenPos.y, moveDuration / 2f)
                    .SetUpdate(true);
            });
    }

    public void CloseBackground()
    {
        // Deactivate the panel immediately
        if (panelToActivate != null)
            panelToActivate.SetActive(false);

        // Tween up a little first, then off-screen
        rect.DOLocalMoveY(onScreenPos.y + overshoot, moveDuration / 2f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                rect.DOLocalMoveY(offScreenPos.y, moveDuration / 2f)
                    .SetUpdate(true);
            });
    }
}
