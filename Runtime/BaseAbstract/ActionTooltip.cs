using System;

namespace Meangpu.Tooltip
{
    public static class ActionTooltip
    {
        public static Action OnShowTooltip;
        public static Action OnHideTooltip;
        public static Action<string, string> OnDisplayBasicText;
    }
}