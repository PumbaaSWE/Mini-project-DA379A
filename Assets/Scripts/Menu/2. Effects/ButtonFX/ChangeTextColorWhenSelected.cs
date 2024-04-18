using TMPro;
using UnityEngine;

public class ChangeTextColorWhenSelected : SelectEffect
{
    Color originalColor;

    [SerializeField]
    Color colorWhenSelected;

    [SerializeField]
    TextMeshProUGUI textMesh;

    public override void Initiate()
    {
        base.Initiate();

        if (textMesh)
            originalColor = textMesh.color;
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
            textMesh.color = Color.Lerp(originalColor, colorWhenSelected, t);
        else
            textMesh.color = Color.Lerp(colorWhenSelected, originalColor, t);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
