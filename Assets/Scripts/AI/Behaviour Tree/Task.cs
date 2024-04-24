using UnityEngine;

/// <summary>
/// Abstract class for a Behaviour Tree node.
/// </summary>
public abstract class Task : MonoBehaviour
{
    protected Status taskStatus;

    public Status TaskStatus { get => taskStatus; }

    public abstract Status Tick();

    public enum Status
    {
        Success,
        Failure,
        Running
    }
}
