using System;

public abstract class GenericSingleton<T> where T : GenericSingleton<T>
{
    private static readonly T instance = (T)Activator.CreateInstance(typeof(T), true);

    protected GenericSingleton() { }

    public static T GetInstance() { return instance; }
}
