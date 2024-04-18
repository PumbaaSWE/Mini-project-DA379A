using UnityEngine;
using UnityEngine.UI;

public class ChangeColorWhenSelected : SelectEffect
{
    Color originalColor;

    [SerializeField]
    Color colorWhenSelected;

    [SerializeField]
    Image image;

    public override void Initiate()
    {
        base.Initiate();

        if (image)
            originalColor = image.color;
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
            image.color = Color.Lerp(originalColor, colorWhenSelected, t);
        else
            image.color = Color.Lerp(colorWhenSelected, originalColor, t);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
