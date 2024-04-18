using TMPro;
using UnityEngine;

public class DumbCarUI : MonoBehaviour
{

    public TMP_Text speedText;
    public TMP_Text rpmText;
    public DumbCar car;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!car)
        {
            return;
        }
        float kph = car.localVelocity.z * 3.6f;
        speedText.text = kph.ToString("n2");

        int rpm = (int)car.EngineRPM;
        rpmText.text = rpm.ToString();
    }
}
