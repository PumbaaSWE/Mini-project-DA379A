using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Task that performs some action.
/// </summary>
public class Action : Task
{
    [SerializeField]
    private UnityEvent<ActionResult> onTick;

    private readonly ActionResult result = new();

    public override Status Tick()
    {
        if (onTick.GetPersistentEventCount() > 0)
        {
            onTick.Invoke(result);
        }
        else
        {
            throw new System.NotImplementedException();
        }

        taskStatus = result.TickStatus;

        return taskStatus;
    }
}
