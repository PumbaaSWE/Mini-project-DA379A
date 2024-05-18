using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// 
/// <para>  Recipient of package deliveries. </para>
/// 
/// </summary>
public class Recipient : MonoBehaviour
{
    private Deliveries deliveries;

    public Transform personToLookAtVeryCreepily;

    [SerializeField]
    private VisualEffect happyEffect;

    [SerializeField]
    private VisualEffect angryEffect;

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
            Instantiate(angryEffect, transform.position, Quaternion.identity);
            return;
        }

        Destroy(collision.gameObject);
        deliveries.CompleteDelivery(this);

        ScoreKeeper.GetInstance().AddScore(package.Health);
        Instantiate(happyEffect, transform.position, Quaternion.identity);
    }

    public void InitRecipient()
    {
        deliveries = Deliveries.GetInstance();
        deliveries.AddRecipient(this);
    }
}
