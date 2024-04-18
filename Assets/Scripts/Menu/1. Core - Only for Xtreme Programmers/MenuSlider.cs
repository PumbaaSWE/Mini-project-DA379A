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

    [Header("Head")]
    [SerializeField]
    Color headColor;
    [SerializeField]
    Sprite headSprite;
    [SerializeField]
    Vector2 headSize;
    [SerializeField]
    RectTransform head;
    [SerializeField]
    Image headImage;

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

        if (headImage)
        {
            headImage.color = headColor;
            headImage.sprite = headSprite;
        }

        if (head)
        {
            UpdateHeadRect();
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
        UpdateHeadRect();

        var actions = GetComponents<SliderAction>();

        foreach (var action in actions)
            action.Action(value);

        base.Confirm(mousePos);
    }

    [SerializeField]
    TextMeshProUGUI text;
    //Debug rn
    private void FixedUpdate()
    {
        Vector2 value = Vector2.zero;

        if (rect)
        {
            value = rect.position;
        }

        if(text)
            text.text = value.ToString();
    }

    private void SetValueFromMousePos(Vector3 mousePos)
    {
        //!!!---BROKEN AF---!!!

        //mouse screen pos to value on slider
        if (rect)
        {
            //works almost, idk
            float sizeX = rect.sizeDelta.x * Screen.width / 1920f;
            value = Mathf.InverseLerp(rect.position.x - sizeX / 2, rect.position.x + sizeX / 2, mousePos.x) * transform.localScale.x;
        }

    }

    private void UpdateForegroundRect()
    {
        if (rect && foreground)
        {
            //implement correctly
        }
    }

    private void UpdateHeadRect()
    {
        if (rect && head)
        {
            float posX = Mathf.Lerp(rect.offsetMin.x, rect.offsetMax.x, value);
            float offsetX1 = Mathf.Lerp(0, -headSize.x, value);
            float offsetX2 = Mathf.Lerp(headSize.x, 0, value);
            head.offsetMin = new Vector2(posX + offsetX1, -headSize.y);
            head.offsetMax = new Vector2(posX + offsetX2, headSize.y);
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
