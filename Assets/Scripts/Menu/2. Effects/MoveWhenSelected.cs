using UnityEngine;

/// <summary>
/// Has some issues because it doesn't "fully complete" the animation, but shouldn't matter all that much
/// </summary>
public class MoveWhenSelected : SelectEffect
{
    Vector2 previousDiff;

    [SerializeField]
    Vector2 moveWhenSelected;

    public override void Initiate()
    {
        base.Initiate();

        previousDiff = Vector2.zero;
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

        Vector2 diff;

        if (selected)
            diff = Vector2.Lerp(Vector2.zero, moveWhenSelected, t);
        else
            diff = Vector2.Lerp(moveWhenSelected, Vector2.zero, t);

        var rect = GetComponent<RectTransform>();

        rect.offsetMin -= previousDiff;
        rect.offsetMin += diff;
        rect.offsetMax -= previousDiff;
        rect.offsetMax += diff;

        previousDiff = diff;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
