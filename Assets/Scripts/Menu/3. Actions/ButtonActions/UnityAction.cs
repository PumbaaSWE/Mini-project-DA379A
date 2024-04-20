using UnityEngine;
using UnityEngine.Events;

public class UnityAction : ButtonAction
{
    [SerializeField]
    UnityEvent action;

    public override void Action()
    {
        action.Invoke();
    }
}
