using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Singleton instance that keeps track of time.
/// Because this is a MonoBehaviour script, it will be attached to its own GameObject upon instantiation.
/// Use Instance() for EZ access.
/// </summary>
public class Timer : MonoBehaviour
{
    // This script needs to be attached to an object because it's MonoBehaviour
    // It's MonoBehaviour because it needs to have access to Update()
    // Maybe there are better ways to do this? =)

    private static Timer instance;
    private Timer() { }

    public static Timer Instance()
    {
        if (instance == null)
        {
            // Create new GameObject and attach the script as a component
            instance = new GameObject().AddComponent<Timer>();
            instance.gameObject.name = "Timer (Static)";
            instance.gameObject.isStatic = true;
        }

        return instance;
    }

    public enum Mode
    {
        idle,
        countUp,
        countDown
    }

    private float time;
    public Mode mode;

    private void Update()
    {
        switch (mode)
        {
            case Mode.countUp:
                this.time += Time.deltaTime;
                break;
            case Mode.countDown:
                this.time -= Time.deltaTime;
                break;
        }

        //print(time);
    }

    public float Get()
    {
        return this.time;
    }

    public void Set(float time)
    {
        this.time = time;
    }

    public void Add(float time)
    {
        this.time += time;
    }

    public void Remove(float time)
    {
        this.time -= time;
    }
}
