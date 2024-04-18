using UnityEngine;

public static class JackMath
{
    /// <summary>
    /// Remaps value between a and b to be between x and y proportionally
    /// </summary>
    /// <param name="value"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static float Remap(float value, float a, float b, float x, float y)
    {
        return Mathf.Lerp(x, y, Mathf.InverseLerp(a, b, value));
    }
}
