using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Meangpu.Tooltip
{
    public abstract class BaseTooltip : MonoBehaviour
    {
        [Header("Tooltip Object")]
        [SerializeField] protected RectTransform _toolTipRectTrans;
        [SerializeField] protected LayoutElement _tooltipLayoutElementObj;
        [Tooltip("This should not be this object it self")][SerializeField] GameObject _tooltipParentObject;

        [Header("Setting")]
        [SerializeField] protected Vector2 _offSet = new(30, 20);
        [SerializeField] bool IsUsedNewInputSystem;
        protected Vector2 _mousePos;
        protected Vector2 offScreenBound;

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

        public virtual void ShowTooltip() => _tooltipParentObject.SetActive(true);
        public virtual void HideTooltip() => _tooltipParentObject.SetActive(false);

        private void Update() => UpdatePosition();

        void SetupOffScreenBound()
        {
            offScreenBound = new Vector2(_toolTipRectTrans.rect.width, _toolTipRectTrans.rect.height); // use to set bound to box size
        }

        public bool IsBetweenFloat(float testValue, float bound1, float bound2) => testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2);

        public void UpdatePosition()
        {
            SetupOffScreenBound();
            _mousePos = IsUsedNewInputSystem ? Mouse.current.position.ReadValue() : Input.mousePosition;
            DoSetPosSmart(_mousePos);
        }

        bool TooltipBoxIsInScreenBound()
        {
            return IsBetweenFloat(_mousePos.x, offScreenBound.x, Screen.width - offScreenBound.x) && IsBetweenFloat(_mousePos.y, offScreenBound.y, Screen.height - offScreenBound.y);
        }

        void DoSetPosSmart(Vector2 position)
        {
            if (TooltipBoxIsInScreenBound())
            {
                _toolTipRectTrans.pivot = new Vector2(0, 1); // set pivot to 0 1 - it will be lower right
                _tooltipParentObject.transform.position = position + _offSet;
                return;
            }
            else
            {
                float pivotX = position.x / Screen.width;
                float pivotY = position.y / Screen.height;

                _toolTipRectTrans.pivot = new Vector2(pivotX, pivotY);
                _tooltipParentObject.transform.position = position; // at edge of screen so no offset
            }
        }

        // this use to make UI smart and make long text wrap around box
        public abstract bool LayoutElementShouldBeEnable();
        public void UpdateBoxLayoutElement() => _tooltipLayoutElementObj.enabled = LayoutElementShouldBeEnable();
    }
}