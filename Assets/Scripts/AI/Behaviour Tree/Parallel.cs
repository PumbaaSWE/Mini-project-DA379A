using UnityEngine;

public class Parallel : Task
{
    [SerializeField]
    private Condition[] children;

    private int N { get => children.Length; }

    [SerializeField]
    private int M;

    public override Status Tick()
    {
        int successes = 0;
        int failures = 0;

        for(int i = 0; i < children.Length; i++)
        {
            Status status = children[i].Tick();

            if (status == Status.Success)
            {
                successes++;
            }
            else if (status == Status.Failure)
            {
                failures++;
            }

            if (successes >= M)
            {
                return Status.Success;
            }

            if (failures >= (N - M + 1))
            {
                return Status.Failure;
            }
        }

        return Status.Failure;
    }

}
