using UnityEngine;

public abstract class SelectEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 1f)]
    protected float speed;

    [SerializeField]
    protected AnimationCurve curve;

    protected float timer;
    protected bool selected;

    /// <summary>
    /// Use this value when interpolating between values
    /// </summary>
    protected float t;

    public virtual void Initiate()
    {
        selected = false;
        timer = float.MaxValue;
        enabled = false;
    }

    public virtual void Select()
    {
        selected = true;
        timer = speed - Mathf.Min(timer, speed); //
        enabled = true;

    }

    //
    //We have some goofy ass transitions here... Not good!
    //

    public virtual void Deselect()
    {
        selected = false;
        timer = speed - Mathf.Min(timer, speed); //
        enabled = true;
    }

    protected virtual void Update()
    {
        timer = Mathf.Min(timer + Time.deltaTime, speed);

        t = curve.Evaluate(timer / speed);
    }

    protected virtual void LateUpdate()
    {
        if (timer == speed)
            enabled = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (curve is null)
        {
            curve = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }
#endif
}
