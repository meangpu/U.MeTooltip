using UnityEngine;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Meangpu.Tooltip
{
    public abstract class BaseTooltip : MonoBehaviour
    {
        [Header("fix 4k canvas")]
        [SerializeField] protected Transform _canvasParent;
        [Header("Tooltip Object")]
        [SerializeField] protected RectTransform _tooltipRectTrans;
        [SerializeField] protected LayoutElement _tooltipLayoutElementObj;
        [Tooltip("This should not be this object it self")][SerializeField] GameObject _parentObject;

        [Header("Setting")]
        [SerializeField] protected Vector2 _offSet = new(30, 20);
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

        void UpdateScreenBound()
        {
            _offScreenBound.Set(_tooltipRectTrans.rect.width * _canvasParent.localScale.x, _tooltipRectTrans.rect.height * _canvasParent.localScale.y);// use to set bound to box size
        }

        public void UpdatePosition()
        {
            UpdateScreenBound();

#if ENABLE_LEGACY_INPUT_MANAGER
            _mousePos = Input.mousePosition;
#elif ENABLE_INPUT_SYSTEM
            _mousePos = Mouse.current.position.ReadValue();
#endif
            DoSetPosSmart(_mousePos);
        }

        bool IsInBound() => XIsInBound() && YIsInBOund();
        bool XIsInBound() => MathUtil.IsBetweenFloat(_mousePos.x, _offScreenBound.x + _offSet.x, Screen.width - _offScreenBound.x - _offSet.x);
        bool YIsInBOund() => MathUtil.IsBetweenFloat(_mousePos.y, _offScreenBound.y + _offSet.y, Screen.height - _offScreenBound.y - _offSet.y);

        bool IsLeftBoundBad() => _mousePos.x < _offScreenBound.x + _offSet.x;
        bool IsRightBoundBad() => _mousePos.x > Screen.width - _offScreenBound.x - _offSet.x;

        bool IsTopBoundBad() => _mousePos.y > Screen.height - _offScreenBound.y + _offSet.y;
        bool IsBottomBoundBad() => _mousePos.y < _offScreenBound.y - _offSet.y;


        void DoSetPosSmart(Vector2 mousePos)
        {
            Vector2 realOffset = GetRealOffset();

            if (IsInBound())
            {
                _tooltipRectTrans.pivot = new Vector2(0, 1); // set pivot to 0 1 - it will be lower right
                _parentObject.transform.position = mousePos + _offSet;
                return;
            }
            else
            {
                float pivotX = GetXPivot(mousePos);
                float pivotY = GetYPivot(mousePos);

                _tooltipRectTrans.pivot = new Vector2(pivotX, pivotY);
                _parentObject.transform.position = mousePos + realOffset;  // at edge of screen so no offset
            }
        }

        Vector2 GetRealOffset() => new(GetXOffset(), GetYOffset());

        float GetXOffset()
        {
            if (IsRightBoundBad()) return -_offSet.x;
            else return _offSet.x;
        }

        float GetYOffset()
        {
            if (IsTopBoundBad()) return -_offSet.y;
            else return _offSet.y;
        }

        float GetXPivot(Vector2 mousePos)
        {
            // set adaptive UI only when right bad
            if (!IsRightBoundBad()) return 0;
            else return mousePos.x / Screen.width;
        }

        float GetYPivot(Vector2 mousePos)
        {
            // set adaptive UI only when bottom bad
            if (!IsBottomBoundBad()) return 1;
            else return mousePos.y / Screen.height;
        }

        // this use to make UI smart and make long text wrap around box
        public abstract bool LayoutElementShouldBeEnable();
        public void UpdateBoxLayoutElement() => _tooltipLayoutElementObj.enabled = LayoutElementShouldBeEnable();
    }
}