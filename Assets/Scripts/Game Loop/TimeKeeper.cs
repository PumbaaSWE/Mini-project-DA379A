public class TimeKeeper : GenericSingleton<TimeKeeper>
{
    private float time;

    private TimeKeeper() : base() { }

    public float GetTime() 
    { 
        return time; 
    }

    public void AddTime(float timeToAdd)
    {
        time += timeToAdd;
    }

    public void ResetTime(float initialTime)
    {
        time = initialTime;
    }
}
