using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem current;
    public Tooltip _tooltip;

    private void Awake() => current = this;
    public static void Show(string content, string header = "") => current._tooltip.SetText(content, header);
    public static void Hide() => current._tooltip.gameObject.SetActive(false);
}
