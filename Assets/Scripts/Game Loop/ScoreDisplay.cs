using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private string scorePreface = "Score: ";

    [SerializeField]
    private string scoreName = "pts";

    // Update is called once per frame
    void Update()
    {
        scoreText.text = scorePreface + ScoreKeeper.GetInstance().GetScore() + scoreName;
    }
}
