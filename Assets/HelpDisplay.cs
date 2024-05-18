using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpDisplay : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] float shrinkTime = 1f;
    [SerializeField] float shrinkFactor = 1f;
    [SerializeField] RectTransform helpKey;
    bool toggle;
    float time;
    float aspect = 1;

    // Start is called before the first frame update
    void Start()
    {
        aspect = rect.rect.width / rect.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        if(!rect) return;
        if (Input.GetKeyDown(KeyCode.H))
        {
            time = 0;
            toggle = !toggle;
            rect.gameObject.SetActive(true);
            helpKey.gameObject.SetActive(false);
        }
        time += Time.deltaTime / shrinkTime;
        float t = time;
        if(toggle) t = 1 - time;
        float x = Mathf.Lerp(1, shrinkFactor, t);
        float y = Mathf.Lerp(1, aspect * shrinkFactor, t);
        rect.localScale = new Vector3(x, y, 1);
        if(t >= 1)
        {
            rect.gameObject.SetActive(false);
            helpKey.gameObject.SetActive(true);
        }
    }
}
