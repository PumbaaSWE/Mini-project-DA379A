using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform pointFrom;
    public Transform pointAt;

    private Deliveries deliveries;

    private void Start()
    {
        deliveries = Deliveries.Instance();
    }

    private void Update()
    {
        pointAt = deliveries.CurrentDelivery()?.transform;

        if (!pointAt)
            return;

        Vector3 fromPos = pointFrom ? pointFrom.position : transform.position;
        Vector3 relPos = pointAt.position - fromPos;
        relPos.y = 0;

        transform.rotation = Quaternion.LookRotation(relPos.normalized, Vector3.up);

        if (pointFrom)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - pointFrom.rotation.eulerAngles);
    }
}
