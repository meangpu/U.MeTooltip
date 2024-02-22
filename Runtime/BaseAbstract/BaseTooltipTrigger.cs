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

        [SerializeField] bool _preventUIOverMouse = true;

        Camera cam;

        void Awake() => cam = Camera.main;

        public void OnPointerEnter(PointerEventData eventData) => ShowTooltip();
        public void OnPointerExit(PointerEventData eventData) => HideTooltip();


        bool IsRayDoHitObject()
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            return Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance);
        }

        void OnMouseEnter()
        {
            if (_preventUIOverMouse)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
            }

            if (!_checkRayDistance)
            {
                ShowTooltip();
                return;
            }

            if (!IsRayDoHitObject()) return;

            ShowTooltip();
        }

        void OnMouseExit() => HideTooltip();

        private void HideTooltip()
        {
            if (_preventUIOverMouse)
            {
                if (EventSystem.current != null)
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;
                }
            }

            CancelInvoke();
            ActionTooltip.OnHideTooltip?.Invoke();
        }

        private void ShowTooltip()
        {
            if (_useTimeDelay)
            {
                Invoke(nameof(SetupContentThenInvokeAction), _timeBeforePopUp);
            }
            else
            {
                SetupContentThenInvokeAction();
            }
        }

        public void SetupContentThenInvokeAction()
        {
            ShowContent();
            ActionTooltip.OnShowTooltip?.Invoke();
        }

        // child class will implement function to setup data tooltip
        public abstract void ShowContent();
    }
}