using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MenuSelectable
{
    [Header("Slider Value")]
    [SerializeField]
    [Range(0f,1f)]
    float value;

    [Header("Foreground")]
    [SerializeField]
    Color foregroundColor;
    [SerializeField]
    Sprite foregroundSprite;
    [SerializeField]
    RectTransform foreground;
    [SerializeField]
    Image foregroundImage;

    [Header("This")]
    [SerializeField]
    RectTransform rect;

    public override void Initiate()
    {
        if (foregroundImage)
        {
            foregroundImage.color = foregroundColor;
            foregroundImage.sprite = foregroundSprite;
        }

        if (foreground)
        {
            UpdateForegroundRect();
        }

        base.Initiate();
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Confirm(Vector3 mousePos)
    {
        SetValueFromMousePos(mousePos);
        UpdateForegroundRect();

        var actions = GetComponents<SliderAction>();

        foreach (var action in actions)
            action.Action(value);

        base.Confirm(mousePos);
    }

    private void SetValueFromMousePos(Vector3 mousePos)
    {
        //mouse screen pos to value on slider
        if (rect)
        {
            //only works if (CanvasScaler.ScreenMatchMode = Expand)
            float scalingFactor;
            float xScaling = Screen.width / 1920f;
            float yScaling = Screen.height / 1080f;
            scalingFactor = Mathf.Min(xScaling, yScaling);

            float mousePosFromCenter = mousePos.x - Screen.width / 2f;
            float rectPosFromCenter = rect.anchoredPosition.x;
            float rectSize = (rect.sizeDelta.x / 2) * transform.localScale.x;
            value = Mathf.InverseLerp(rectPosFromCenter - rectSize, rectPosFromCenter + rectSize, mousePosFromCenter / scalingFactor);
            //print(mousePosFromCenter + " , " + rectPosFromCenter);
        }

    }

    private void UpdateForegroundRect()
    {
        if (rect && foreground)
        {
            foreground.sizeDelta = new Vector2(rect.sizeDelta.x * value, foreground.sizeDelta.y);
            foreground.anchoredPosition = -Vector2.right * (rect.sizeDelta.x - foreground.sizeDelta.x) / 2f;
        }
    }

    //!!!---SET THIS TO FALSE WHEN PLAYING, SET TO TRUE IF EDITING SLIDER---!!!
#if false
    private void OnValidate()
    {
        if (!Application.isPlaying)
            UnityEditor.EditorApplication.update += Initiate;
    }
#endif
}
