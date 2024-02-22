using UnityEngine;
using TMPro;

namespace Meangpu.Tooltip.Basic
{
    public class Tooltip : BaseTooltip
    {
        public TMP_Text _headerField;
        public TMP_Text _contentField;

        protected override void OnEnable()
        {
            base.OnEnable();
            ActionTooltip.OnDisplayBasicText += DisplayContent;
        }
        protected override void OnDisable()
        {
            base.OnEnable();
            ActionTooltip.OnDisplayBasicText -= DisplayContent;
        }

        public override bool LayoutElementShouldBeEnable()
        {
            return Mathf.Max(_headerField.preferredWidth, _contentField.preferredWidth) >= _tooltipLayoutElementObj.preferredWidth;
        }

        public void DisplayContent(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header)) _headerField.gameObject.SetActive(false);
            else
            {
                _headerField.SetText(header);
                _headerField.gameObject.SetActive(true);
            }
            _contentField.SetText(content);
            UpdateBoxLayoutElement();
            UpdatePosition();
            gameObject.SetActive(true);
        }
    }
}