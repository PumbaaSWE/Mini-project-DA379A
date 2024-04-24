using System;
using System.Diagnostics.CodeAnalysis;

[Serializable]
public readonly struct Vec2Int
{
    private readonly int x;
    private readonly int y;

    public int X { get => x; }
    public int Y { get => y; }

    public Vec2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator ==(Vec2Int first, Vec2Int second)
    {
        return (first.X == second.X) && (first.Y == second.Y);
    }

    public static bool operator !=(Vec2Int first, Vec2Int second)
    {
        return (first.X != second.X) || (first.Y != second.Y);
    }

    public bool Equals(Vec2Int other)
    {
        return this == other;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is Vec2Int vec2 && Equals(vec2);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode();
    }
}
