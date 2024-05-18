using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField] private string preTimeDescription = "Time left: ";
    [SerializeField] private string postTimeDescription = " time left!";
    [SerializeField] private bool showMillies = true;

    [SerializeField]
    private float startTime = 180f;

    [SerializeField]
    private int endSceneIndex = 2;

    // Start is called before the first frame update
    void Start()
    {
        TimeKeeper.GetInstance().ResetTime(startTime);
    }

    // Update is called once per frame
    void Update()
    {
        TimeKeeper.GetInstance().AddTime(-Time.deltaTime);

        if(TimeKeeper.GetInstance().GetTime() <= 0)
        {
            SceneManager.LoadScene(endSceneIndex);
        }
        else
        {
            float time = TimeKeeper.GetInstance().GetTime();
            //string result = string.Format(CultureInfo.InvariantCulture, "{00}:{01:00}", (int)time / 60, time % 60);

            TimeSpan ts = TimeSpan.FromSeconds(time);

            string result = ts.ToString(showMillies ? "mm\\:ss\\.fff" : "mm\\:ss");

            timeText.text = preTimeDescription + result + postTimeDescription;
        }        
    }
}
