using UnityEngine;

public class PathBlockingHandler : MonoBehaviour
{
    [SerializeField]
    private PathBlocker[] pathBlockers;

    [Range(0f, 100f)]
    [SerializeField]
    private float stateChangeTime = 5.0f;

    private float[] timers;

    // Start is called before the first frame update
    void Start()
    {
        timers = new float[pathBlockers.Length];
        for(int i = 0; i < timers.Length; i++)
        {
            timers[i] = stateChangeTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < timers.Length; i++)
        {
            if (timers[i] <= 0)
            {
                pathBlockers[i].SwitchState();

                timers[i] = stateChangeTime;
            }
            else
            {
                timers[i] -= Time.deltaTime;
            }
        }
    }
}
