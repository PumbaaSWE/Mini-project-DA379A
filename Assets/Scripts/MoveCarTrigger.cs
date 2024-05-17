using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCarTrigger : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider bc;

    [SerializeField]
    float expandModifier = 1f;
    [SerializeField]
    float expandWidthModifier = 1f;
    [SerializeField]
    float expandSpeedModifier = 1f;

    float startWidth;
    float startLength;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        startWidth = bc.size.x;
        startLength = bc.size.z;
    }

    void FixedUpdate()
    {
        float speed = rb.velocity.magnitude;
        Vector3 dir = rb.velocity.normalized;

        float w = (2 - Mathf.Abs(Vector3.Dot(dir, transform.parent.forward))) * expandWidthModifier;

        bc.size = new Vector3(startWidth * w + speed * expandSpeedModifier, bc.size.y, Mathf.Max(startLength, speed * expandModifier));
        transform.position = transform.parent.position + rb.velocity * expandModifier / 2f;
        if(dir.sqrMagnitude > 0)
            transform.forward = dir;
    }
}
