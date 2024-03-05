using System;

namespace Meangpu.Tooltip
{
    public static class ActionMeTooltip
    {
        public static Action OnShowTooltip;
        public static Action OnHideTooltip;
        public static Action<string, string> OnDisplayBasicText;
    }
}