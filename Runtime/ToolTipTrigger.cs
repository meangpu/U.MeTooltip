using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float timeBeforePopUp = 0.1f;
    [SerializeField] bool useTimeDelay = true;

    [SerializeField] string header;
    [Multiline()]
    [SerializeField] string content;


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    void OnMouseEnter()
    {
        ShowTooltip();
    }

    void OnMouseExit()
    {
        HideTooltip();
    }

    private void HideTooltip()
    {
        CancelInvoke();
        if (ToolTipSystem.current != null) ToolTipSystem.Hide();
    }

    private void ShowTooltip()
    {
        if (useTimeDelay) Invoke("ShowContent", timeBeforePopUp);
        else ShowContent();
    }

    void ShowContent()
    {
        ToolTipSystem.Show(content, header);
    }
}
