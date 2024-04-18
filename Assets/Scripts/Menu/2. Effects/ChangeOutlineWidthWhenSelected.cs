using UnityEngine;

public class ChangeOutlineWidthWhenSelected : SelectEffect
{
    float previousScale;

    [SerializeField]
    float scaleWhenSelected;

    [SerializeField]
    Outline outline;

    public override void Initiate()
    {
        base.Initiate();

        previousScale = 1f;
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

        float scale;

        if (selected)
            scale = Mathf.SmoothStep(1f, scaleWhenSelected, t);
        else
            scale = Mathf.SmoothStep(scaleWhenSelected, 1f, t);

        if (outline)
        {
            outline.width /= previousScale;
            outline.width *= scale;
            previousScale = scale;
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
