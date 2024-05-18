using UnityEngine;

/// <summary>
/// 
/// <para>  Recipient of package deliveries. </para>
/// 
/// </summary>
public class Recipient : MonoBehaviour
{
    private Deliveries deliveries;

    public Transform personToLookAtVeryCreepily;

    private void Update()
    {
        if (personToLookAtVeryCreepily)
        {
            transform.LookAt(new Vector3(personToLookAtVeryCreepily.position.x, transform.position.y, personToLookAtVeryCreepily.position.z));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.TryGetComponent(out Package package))
            return;

        if (deliveries.CurrentRecipient() != this)
        {
            ScoreKeeper.GetInstance().AddScore(-50);
            return;
        }

        Destroy(collision.gameObject);
        deliveries.CompleteDelivery(this);

        ScoreKeeper.GetInstance().AddScore(package.Health);
    }

    public void InitRecipient()
    {
        deliveries = Deliveries.GetInstance();
        deliveries.AddRecipient(this);
    }
}
