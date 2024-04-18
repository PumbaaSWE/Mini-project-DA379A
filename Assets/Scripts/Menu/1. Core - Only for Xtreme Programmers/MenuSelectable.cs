using UnityEngine;
using UnityEngine.UI;

public class MenuSelectable : MenuElement
{
    protected bool selected;

    [Header("Background")]
    [SerializeField]
    protected Color backgroundColor;
    [SerializeField]
    protected Sprite backgroundSprite;
    [SerializeField]
    protected Image backgroundImage;

    [Header("Outline")]
    [SerializeField]
    protected Color outlineColor;
    [SerializeField]
    protected float outlineWidth;
    [SerializeField]
    protected Outline.OutlineType outlineType;
    [SerializeField]
    protected Outline outline;

    [Header("Effects")]
    [SerializeField]
    protected SelectEffect[] effects;

    [Header("Navigation")]
    [SerializeField]
    MenuButton left;
    [SerializeField]
    MenuButton right;
    [SerializeField]
    MenuButton up;
    [SerializeField]
    MenuButton down;

    public override void Initiate()
    {
        if (backgroundImage)
        {
            backgroundImage.color = backgroundColor;
            backgroundImage.sprite = backgroundSprite;
        }

        if (outline)
        {
            outline.color = outlineColor;
            outline.width = outlineWidth;
            outline.outlineType = outlineType;
        }

        if (this)
            effects = GetComponents<SelectEffect>();

        foreach (var effect in effects)
            if (effect)
                effect.Initiate();

        base.Initiate();
    }

    public virtual void Select()
    {
        foreach(var effect in effects)
            effect.Select();
    }

    public virtual void Deselect()
    {
        foreach (var effect in effects)
            effect.Deselect();
    }

    public virtual MenuSelectable Navigate(Vector2 direction)
    {
        if (direction == Vector2.left)
            return left;
        if (direction == Vector2.right)
            return right;
        if (direction == Vector2.up)
            return up;
        if (direction == Vector2.down)
            return down;

        return null;
    }

    public virtual void Confirm(Vector3 mousePos)
    {

    }
}
