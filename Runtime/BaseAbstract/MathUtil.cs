using UnityEngine;

namespace Meangpu.Tooltip
{
    public static class MathUtil
    {
        public static bool IsBetweenFloat(float testValue, float bound1, float bound2) => testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2);
    }
}
