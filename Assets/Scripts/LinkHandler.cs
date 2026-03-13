using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkHandler : MonoBehaviour//, IPointerClickHandler
{
    private TMP_Text textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, eventData.position, null);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();

            Application.OpenURL(linkID);
        }
    }*/

    public void OnPointerEnter()
    {
        gameObject.transform.localScale = Vector3.one * 1.5f;
    }

    public void OnPointerExit()
    {
        gameObject.transform.localScale = Vector3.one;
    }

    public void OpenLink()
    {
        Application.OpenURL("https://epic-we.eu");
    }
}
