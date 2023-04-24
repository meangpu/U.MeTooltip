using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    // original code learn from: https://www.youtube.com/watch?v=HXFoUGw7eKk
    public TMP_Text headerField;
    public TMP_Text contentField;

    public LayoutElement layoutElementObj;
    public RectTransform rectTrans;

    public Vector2 offSet = new Vector2(30, 20);
    Vector2 _mousePos;

    public Vector2 offScreenBound;
    public bool IsUsedNewInputSystem;

    private void Awake() => rectTrans = GetComponent<RectTransform>();

    private void Update() => UpdatePosition();

    void SetupOffScreenBound()
    {
        offScreenBound = new Vector2(rectTrans.rect.width, rectTrans.rect.height); // use to set bound to box size
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
        SetBoxLayoutElement();

        UpdatePosition();
        gameObject.SetActive(true);
    }

    public void UpdatePosition()
    {
        SetupOffScreenBound();
        _mousePos = IsUsedNewInputSystem ? Mouse.current.position.ReadValue() : Input.mousePosition;
        DoSetPosSmart(_mousePos);
    }

    bool TooltipBoxIsInScreenBound()
    {
        return IsBetweenFloat(_mousePos.x, offScreenBound.x, Screen.width - offScreenBound.x) && IsBetweenFloat(_mousePos.y, offScreenBound.y, Screen.height - offScreenBound.y);
    }

    void DoSetPosSmart(Vector2 position)
    {
        if (TooltipBoxIsInScreenBound())
        {
            rectTrans.pivot = new Vector2(0, 1); // set pivot to 0 1 - it will be lower right
            transform.position = position + offSet;
            return;
        }
        else
        {
            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            rectTrans.pivot = new Vector2(pivotX, pivotY);
            transform.position = position; // at edge of screen so no offset
        }
    }

    void SetBoxLayoutElement()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        layoutElementObj.enabled = Mathf.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElementObj.preferredWidth;
    }

    public bool IsBetweenFloat(float testValue, float bound1, float bound2)
    {
        return (testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2));
    }

}
