using UnityEngine;

namespace Meangpu.Tooltip.Basic
{
    public class ToolTipTrigger : BaseTooltipTrigger
    {
        [SerializeField] string header;
        [TextArea][SerializeField] string content;

        public override void ShowContent()
        {

        }
    }
}