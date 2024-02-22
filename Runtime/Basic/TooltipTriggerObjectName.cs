using UnityEngine;

namespace Meangpu.Tooltip.Basic
{
    public class TooltipTriggerObjectName : BaseTooltipTrigger
    {
        // content will get show before tooltip Action OnShowTooltip got call
        public override void ShowContent() => ActionTooltip.OnDisplayBasicText?.Invoke(gameObject.name, "");
    }
}