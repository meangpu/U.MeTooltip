using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float timeBeforePopUp = 0.1f;
    [SerializeField] bool useTimeDelay = true;

    [SerializeField] string header;
    [Multiline()]
    [SerializeField] string content;

    [Header("Ray check")]
    [SerializeField] bool _checkRayDistance;
    [SerializeField] float _maxRayDistance = 2f;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    bool IsRayDoHitObject()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, _maxRayDistance);
    }

    void OnMouseEnter()
    {
        if (!_checkRayDistance)
        {
            ShowTooltip();
            return;
        }

        if (!IsRayDoHitObject()) return;

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
