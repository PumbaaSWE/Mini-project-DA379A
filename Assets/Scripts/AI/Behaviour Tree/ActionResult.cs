/// <summary>
/// Stores the result of an Action calling its Tick method.
/// </summary>
public class ActionResult
{
    public Task.Status TickStatus { get; set; }
}
