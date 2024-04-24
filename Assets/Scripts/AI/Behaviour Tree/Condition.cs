using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Returns Success or Failure depending on if some condition is true or false.
/// </summary>
public class Condition : Task
{
    [SerializeField]
    private UnityEvent<ConditionResult> conditionEvaluation;

    private readonly ConditionResult result = new();

    public override Status Tick()
    {
        if (conditionEvaluation.GetPersistentEventCount() > 0)
        {
            conditionEvaluation.Invoke(result);
        }
        else
        {
            throw new System.NotImplementedException();
        }

        if (result.Result == true)
        {
            taskStatus = Status.Success;
        }
        else
        {
            taskStatus = Status.Failure;
        }

        return taskStatus;
    }
}
