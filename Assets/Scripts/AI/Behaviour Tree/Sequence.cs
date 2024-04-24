using UnityEngine;

/// <summary>
/// Performs Tasks from left to right until a Task does not return Success.
/// </summary>
public class Sequence : Task
{
    [SerializeField]
    private Task[] children;

    public override Status Tick()
    {
        for(int i = 0; i < children.Length; i++)
        {
            Status status = children[i].Tick();

            if(status != Status.Success)
            {
                return status;
            }
        }

        return Status.Success;
    }
}
