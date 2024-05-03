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
        deliveries.StartRandomDelivery();
    }

    private void Update()
    {
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
            Debug.LogError(name + ": This delivery is not for me!");
            return;
        }

        Destroy(collision.gameObject);
        deliveries.CompleteDelivery(this);
        enabled = false;

        print(name + ": Thank you!");
        print("Number of completed deliveries: " + deliveries.Completed() + " / " + deliveries.Needed());
        print("Number of delivery requests: " + deliveries.Waiting());
    }
}
