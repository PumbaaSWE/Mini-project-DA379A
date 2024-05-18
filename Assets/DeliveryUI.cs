using UnityEngine;
using UnityEngine.UI;

public class DeliveryUI : MonoBehaviour
{
    public Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!image) image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Recipient recipient = Deliveries.GetInstance().CurrentRecipient();
        if (recipient == null)
        {
            image.enabled = false;
            return;
        }


        Vector3 relPos = recipient.transform.position - Camera.main.transform.position; //cam to recipiant

        //Vector3 pos = Camera.main.WorldToScreenPoint(recipient.transform.position + Vector3.up * 2);

        if(Vector3.Dot(relPos, Camera.main.transform.forward) > 0)
        {
            image.enabled = true;
            Vector3 pos = Camera.main.WorldToScreenPoint(recipient.transform.position + Vector3.up * 2).WithZ();

            image.transform.position = pos;
        }
        else
        {
            image.enabled = false;
        }

    }
}
