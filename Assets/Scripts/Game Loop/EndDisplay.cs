using TMPro;
using UnityEngine;

public class EndDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private string scoreMessage = "Your final score was: ";

    [SerializeField]
    private string scoreName = "pts!";

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = scoreMessage + ScoreKeeper.GetInstance().GetScore() + scoreName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
