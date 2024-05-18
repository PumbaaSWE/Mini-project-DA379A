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

    public static Vector3 WithZ(this Vector3 v, float z = 0)
    {
        return new Vector3(v.x, v.y, z);
    }
    public static Vector3 WithY(this Vector3 v, float y = 0)
    {
        return new Vector3(v.x, y, v.z);
    }
    public static Vector3 WithX(this Vector3 v, float x = 0)
    {
        return new Vector3(x, v.y, v.z);
    }
}
