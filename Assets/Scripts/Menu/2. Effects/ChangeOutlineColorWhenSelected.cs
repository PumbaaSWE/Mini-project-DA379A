using UnityEngine;

public class ChangeOutlineColorWhenSelected : SelectEffect
{
    Color originalColor;

    [SerializeField]
    Color colorWhenSelected;

    [SerializeField]
    Outline outline;

    public override void Initiate()
    {
        base.Initiate();

        if (outline)
            originalColor = outline.color;
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    protected override void Update()
    {
        base.Update();

        if (selected)
            outline.color = Color.Lerp(originalColor, colorWhenSelected, t);
        else
            outline.color = Color.Lerp(colorWhenSelected, originalColor, t);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
