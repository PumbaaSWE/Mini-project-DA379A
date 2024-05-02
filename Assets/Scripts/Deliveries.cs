using UnityEngine;

/// <summary>
/// Singleton instance that keeps track of deliveries.
/// Use Instance() for EZ access.
/// </summary>
public class Deliveries
{
    private static Deliveries instance;
    private Deliveries() { }

    public static Deliveries Instance()
    {
        if (instance == null) instance = new Deliveries();

        return instance;
    }
}
