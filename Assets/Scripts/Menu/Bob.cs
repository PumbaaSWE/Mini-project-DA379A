using UnityEngine;

public class Bob : MonoBehaviour
{
    [SerializeField]
    Vector3 metersPerBounce;

    [SerializeField]
    float bounceSpeed;

    [SerializeField]
    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        transform.position = originalPosition + Mathf.Sin(Time.time * bounceSpeed) * metersPerBounce;
    }
}
