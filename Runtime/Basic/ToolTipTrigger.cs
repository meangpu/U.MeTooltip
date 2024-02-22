using UnityEngine;

namespace Meangpu.Tooltip.Basic
{
    public class TooltipTrigger : BaseTooltipTrigger
    {
        [SerializeField] string header;
        [TextArea][SerializeField] string content;

        // content will get show before tooltip Action OnShowTooltip got call
        public override void ShowContent() => ActionTooltip.OnDisplayBasicText?.Invoke(content, header);
    }
}