using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VInspector;

namespace Meangpu.Tooltip
{
    public abstract class BaseTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] bool _useTimeDelay = true;

        [ShowIf("_useTimeDelay")]
        [SerializeField] float _timeBeforePopUp = 0.1f;
        [EndIf]

        [Header("Ray check")]

        [SerializeField] bool _checkRayDistance;
        [ShowIf("_checkRayDistance")]
        [SerializeField] float _maxRayDistance = 999f;
        [EndIf]

        [SerializeField] bool _UIPreventThisTooltipFromTrigger = false;

        bool _isMouseOverThisObject;
        public bool IsMouseOverThisObject => _isMouseOverThisObject;

        Camera cam;

        void Awake() => cam = Camera.main;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            ShowTooltip();
            _isMouseOverThisObject = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            HideTooltip();
            _isMouseOverThisObject = false;
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

        void OnMouseExit() => HideTooltip();

        bool IsRayDoHitObject()
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            return Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance);
        }

        public virtual void HideTooltip()
        {
            if (_UIPreventThisTooltipFromTrigger && EventSystem.current.IsPointerOverGameObject()) return;

            CancelInvoke();
            InvokeHideTooltip();
        }

        public virtual void ShowTooltip()
        {
            if (_UIPreventThisTooltipFromTrigger && EventSystem.current.IsPointerOverGameObject()) return;

            if (_useTimeDelay)
            {
                Invoke(nameof(SetupContentThenInvokeAction), _timeBeforePopUp);
            }
            else
            {
                SetupContentThenInvokeAction();
            }
        }

        public virtual void InvokeShowTooltip() => ActionMeTooltip.OnShowTooltip?.Invoke();
        public virtual void InvokeHideTooltip() => ActionMeTooltip.OnHideTooltip?.Invoke();

        public void SetupContentThenInvokeAction()
        {
            ShowContent();
            InvokeShowTooltip();
        }

        // child class will implement function to setup data tooltip
        public abstract void ShowContent();
    }
}