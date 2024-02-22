using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Meangpu.Tooltip
{
    public abstract class BaseTooltip : MonoBehaviour
    {
        [Header("Tooltip Object")]
        [SerializeField] protected RectTransform _tooltipRectTrans;
        [SerializeField] protected LayoutElement _tooltipLayoutElementObj;
        [Tooltip("This should not be this object it self")][SerializeField] GameObject _parentObject;

        [Header("Setting")]
        [SerializeField] protected Vector2 _offSet = new(30, 20);
        protected Vector2 _mousePos;
        protected Vector2 _offScreenBound;
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
            ActionTooltip.OnShowTooltip += ShowTooltip;
            ActionTooltip.OnHideTooltip += HideTooltip;
        }

        protected virtual void OnDisable()
        {
            ActionTooltip.OnShowTooltip -= ShowTooltip;
            ActionTooltip.OnHideTooltip -= HideTooltip;
        }

        public virtual void ShowTooltip() => SetParentObjectActive(true);
        public virtual void HideTooltip() => SetParentObjectActive(false);

        private void Update() => UpdatePosition();

        void SetupOffScreenBound()
        {
            _offScreenBound = new Vector2(_tooltipRectTrans.rect.width, _tooltipRectTrans.rect.height); // use to set bound to box size
        }

        public bool IsBetweenFloat(float testValue, float bound1, float bound2) => testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2);

        public void UpdatePosition()
        {
            SetupOffScreenBound();

#if ENABLE_LEGACY_INPUT_MANAGER
            _mousePos = Input.mousePosition;
#elif ENABLE_INPUT_SYSTEM
            _mousePos = Mouse.current.position.ReadValue();
#endif
            DoSetPosSmart(_mousePos);
        }

        bool IsInBound() => XIsInBound() && YIsInBOund();
        bool XIsInBound() => IsBetweenFloat(_mousePos.x, _offScreenBound.x, Screen.width - _offScreenBound.x);
        bool YIsInBOund() => IsBetweenFloat(_mousePos.y, _offScreenBound.y, Screen.height - _offScreenBound.y);

        bool IsLeftBoundBad() => _mousePos.x < _offScreenBound.x;
        bool IsRightBoundBad() => _mousePos.x > Screen.width - _offScreenBound.x;

        bool IsTopBoundBad() => _mousePos.y > Screen.height - _offScreenBound.y;
        bool IsBottomBoundBad() => _mousePos.y < _offScreenBound.y;


        void DoSetPosSmart(Vector2 mousePos)
        {
            IsBottomBoundBad();
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
                _parentObject.transform.position = mousePos + _offSet; ; // at edge of screen so no offset
            }
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