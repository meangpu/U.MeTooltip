using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        [SerializeField] bool IsUsedNewInputSystem;
        protected Vector2 _mousePos;
        protected Vector2 offScreenBound;


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
            offScreenBound = new Vector2(_tooltipRectTrans.rect.width, _tooltipRectTrans.rect.height); // use to set bound to box size
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
                _tooltipRectTrans.pivot = new Vector2(0, 1); // set pivot to 0 1 - it will be lower right
                _parentObject.transform.position = position + _offSet;
                return;
            }
            else
            {
                float pivotX = position.x / Screen.width;
                float pivotY = position.y / Screen.height;

                _tooltipRectTrans.pivot = new Vector2(pivotX, pivotY);
                _parentObject.transform.position = position; // at edge of screen so no offset
            }
        }

        // this use to make UI smart and make long text wrap around box
        public abstract bool LayoutElementShouldBeEnable();
        public void UpdateBoxLayoutElement() => _tooltipLayoutElementObj.enabled = LayoutElementShouldBeEnable();
    }
}