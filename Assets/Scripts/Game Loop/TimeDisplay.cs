using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private string timeDescription = "s left!";

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
            timeText.text = TimeKeeper.GetInstance().GetTime().ToString("0.00") + timeDescription;
        }        
    }
}
