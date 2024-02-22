using UnityEngine;

namespace Meangpu.Tooltip.Basic
{
    public class TooltipTrigger : BaseTooltipTrigger
    {
        [SerializeField] string header;
        [TextArea][SerializeField] string content;

        public override void ShowContent() => ActionTooltip.OnDisplayBasicText?.Invoke(content, header);
    }
}