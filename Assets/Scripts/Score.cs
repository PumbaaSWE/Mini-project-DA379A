using UnityEngine;

/// <summary>
/// Singleton instance that keeps track of score.
/// Use Instance() for EZ access.
/// </summary>
public class Score
{
    private static Score instance;
    private Score() { }

    public static Score Instance()
    {
        if (instance == null) instance = new Score();

        return instance;
    }

    private int score;

    public int Get()
    {
        return this.score;
    }

    public void Set(int score)
    {
        this.score = score;
    }

    public void Add(int score)
    {
        this.score += score;
    }

    public void Remove(int score)
    {
        this.score -= score;
    }
}
