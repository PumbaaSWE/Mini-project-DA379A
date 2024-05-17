public class ScoreKeeper : GenericSingleton<ScoreKeeper>
{
    private int score = 0;

    private ScoreKeeper() : base() { }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
