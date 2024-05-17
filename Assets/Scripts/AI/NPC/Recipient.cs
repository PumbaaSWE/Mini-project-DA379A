using UnityEngine;

/// <summary>
/// 
/// <para>  Will probably inherit pedestrian later? </para>
/// 
/// </summary>
public class Recipient : MonoBehaviour
{
    Deliveries deliveries;

    public Transform personToLookAtVeryCreepily;

    private void Start()
    {
        deliveries = Deliveries.Instance();
        deliveries.RequestDelivery(this);
        //deliveries.StartRandomDelivery(); //sjukt... den ger ju error med mer än en delivery person...
    }

    private void Update()
    {
        if (deliveries.CurrentDelivery() == null) deliveries.StartRandomDelivery();
        if (personToLookAtVeryCreepily)
        {
            transform.LookAt(new Vector3(personToLookAtVeryCreepily.position.x, transform.position.y, personToLookAtVeryCreepily.position.z));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.TryGetComponent(out Package _))
            return;

        if (deliveries.CurrentDelivery() != this)
        {
            //Debug.LogError(name + ": This delivery is not for me!"); //Kasta inget error 
            return;
        }

        print(name + ": Thank you!");
        print("Number of completed deliveries: " + deliveries.Completed() + " / " + deliveries.Needed());
        print("Number of delivery requests: " + deliveries.Waiting());

        Destroy(collision.gameObject);
        deliveries.CompleteDelivery(this);
        enabled = false;

        ScoreKeeper.GetInstance().AddScore(100);
    }
}
