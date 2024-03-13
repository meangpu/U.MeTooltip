using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Meangpu.Tooltip
{
    public abstract class BaseTooltipLeftRight : MonoBehaviour
    {
        [Header("fix 4k canvas")]
        [SerializeField] protected Transform _canvasParent;
        [Header("Tooltip Object")]
        [SerializeField] protected RectTransform _tooltipRectTrans;
        [SerializeField] protected LayoutElement _tooltipLayoutElementObj;
        [Tooltip("This should not be this object it self")][SerializeField] GameObject _parentObject;

        [Header("Setting")]
        [SerializeField] protected Vector2 _offSet = new(30, 20);
        protected Vector2 _realOffset = new();
        protected Vector2 _mousePos;
        protected Vector2 _offScreenBound = new();
        protected Vector2 _nouUIPos;


        public void SetParentObjectActive(bool newState)
        {
            if (_parentObject == null)
            {
                Debug.Log($"<color=red>Parent object not set!!</color>", gameObject);
                return;
            }
            _parentObject.SetActive(newState);
        }

        private void Awake() => SetParentObjectActive(false);

        protected virtual void OnEnable()
        {
            ActionMeTooltip.OnShowTooltip += ShowTooltip;
            ActionMeTooltip.OnHideTooltip += HideTooltip;
        }

        protected virtual void OnDisable()
        {
            ActionMeTooltip.OnShowTooltip -= ShowTooltip;
            ActionMeTooltip.OnHideTooltip -= HideTooltip;
        }

        public virtual void ShowTooltip() => SetParentObjectActive(true);
        public virtual void HideTooltip() => SetParentObjectActive(false);

        private void Update() => UpdatePosition();

        void SetupOffScreenBound()
        {
            _offScreenBound.Set(_tooltipRectTrans.rect.width * _canvasParent.localScale.x, _tooltipRectTrans.rect.height * _canvasParent.localScale.y); // use to set bound to box size
        }


        public void UpdatePosition()
        {
            SetupOffScreenBound();

#if ENABLE_LEGACY_INPUT_MANAGER
            _mousePos = Input.mousePosition;
#elif ENABLE_INPUT_SYSTEM
            _mousePos = Mouse.current.position.ReadValue();
#endif
            SetTooltipPivotByMousePos(_mousePos);
        }

        void SetTooltipPivotByMousePos(Vector2 mousePos)
        {
            if (mousePos.x < Screen.width * 0.5f)
            {
                SetTooltipPosToRightOfMouse();
                _realOffset = _offSet;
            }
            else
            {
                SetTooltipToLeftOfMouse();
                _realOffset.Set(-_offSet.x, _offSet.y);
            }

            if (IsBottomBoundBad())
            {
                _tooltipRectTrans.pivot = new(_tooltipRectTrans.pivot.x, 0);
            }

            _parentObject.transform.position = mousePos + _realOffset;
        }

        bool IsBottomBoundBad() => _mousePos.y < _offScreenBound.y - _offSet.y;


        void SetTooltipPosToRightOfMouse() => _tooltipRectTrans.pivot = new(0, 1);
        void SetTooltipToLeftOfMouse() => _tooltipRectTrans.pivot = new(1, 1);

        // this use to make UI smart and make long text wrap around box
        public abstract bool LayoutElementShouldBeEnable();
        public void UpdateBoxLayoutElement() => _tooltipLayoutElementObj.enabled = LayoutElementShouldBeEnable();
    }
}
