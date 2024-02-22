using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Meangpu.Tooltip.Basic
{
    public class Tooltip : BaseTooltip
    {
        public TMP_Text headerField;
        public TMP_Text contentField;
        [SerializeField] LayoutElement layoutElementObj;

        public override void HideTooltip()
        {
            throw new System.NotImplementedException();
        }

        public override void ShowTooltip()
        {
            throw new System.NotImplementedException();
        }

        public override bool LayoutElementShouldBeEnable()
        {
            return Mathf.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElementObj.preferredWidth;
        }

        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header)) headerField.gameObject.SetActive(false);
            else
            {
                headerField.SetText(header);
                headerField.gameObject.SetActive(true);
            }
            contentField.SetText(content);
            UpdateBoxLayoutElement();
            UpdatePosition();
            gameObject.SetActive(true);
        }

    }
}