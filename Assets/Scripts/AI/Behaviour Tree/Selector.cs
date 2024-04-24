using UnityEngine;

/// <summary>
/// Performs Tasks from left to right as long as they return Failure.
/// </summary>
public class Selector : Task
{
    [SerializeField]
    private Task[] children;

    public override Status Tick()
    {
        for(int i = 0; i < children.Length; i++)
        {
            if (children[i].Tick() != Status.Failure)
            {
                return children[i].TaskStatus;
            }
        }

        return Status.Failure;
    }
}
