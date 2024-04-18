using UnityEngine;

public class ResizeWhenSelected : SelectEffect
{
    float previousScale;

    [SerializeField]
    float scaleWhenSelected;

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
            scale = Mathf.Lerp(1f, scaleWhenSelected, t);
        else
            scale = Mathf.Lerp(scaleWhenSelected, 1f, t);

        transform.localScale /= previousScale;
        transform.localScale *= scale;
        previousScale = scale;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
