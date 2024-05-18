using UnityEngine;
using UnityEngine.UI;

public class FindLostPackage : MonoBehaviour
{
    public Image image;
    [SerializeField] float time = 5;
    [SerializeField] float frequency = 5;
    [SerializeField] float minDist = 5;
    float timer = 0;

    Package package;
    Color startColor;

    // Start is called before the first frame update
    void Start()
    {
        startColor = image.color;
        image.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.G))
        {
            package = FindObjectOfType<Package>();
            if (!package)
            {
                return;
            }
            //if (!package.gameObject.activeSelf)
            //{
            //    return;
            //}
            //Find
            //print("finding obj" + package);
            timer = time;
            image.enabled = true;
            image.color = startColor;
        }


        timer -= Time.deltaTime;
        if (timer > 0)
        {
            UpdatePosition();
           
        }
        else
        {
            image.enabled = false;
        }
    }

    public void UpdatePosition()
    {

        if(!package) return;

        Vector3 relPos = package.transform.position - Camera.main.transform.position; //cam to recipiant

        if (relPos.sqrMagnitude < minDist * minDist)
        {
            timer = 0;
            image.enabled = false;
        }

        image.color = Color.Lerp(startColor, Color.clear, Mathf.Sin(Time.time * frequency) * .5f + .5f);

        //Vector3 pos = Camera.main.WorldToScreenPoint(recipient.transform.position + Vector3.up * 2);

        if (Vector3.Dot(relPos, Camera.main.transform.forward) > 0)
        {
            image.enabled = true;
            Vector3 pos = Camera.main.WorldToScreenPoint(package.transform.position).WithZ();

            image.transform.position = pos;
        }
        else
        {
            image.enabled = false;
        }
    }
}
