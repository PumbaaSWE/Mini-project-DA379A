using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float smoothPositionTime = .2f;
    //[SerializeField] float camSmoothSpeed = .2f;
    //[SerializeField] float followForce = 1000;
    [SerializeField] Transform toFollow;
    [SerializeField] Vector3 offset = new Vector3(0, 5, -20);

    public Transform ToFollow { get { return toFollow; } set { toFollow = value; } }
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        if (!toFollow) enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, toFollow.position, smoothPositionTime);

        //GetComponent<Rigidbody>().velocity = delta * smoothPositionTime;
        transform.position = Vector3.SmoothDamp(transform.position, toFollow.position+toFollow.TransformDirection(offset), ref velocity, smoothPositionTime);

        transform.rotation = Quaternion.LookRotation(toFollow.forward, Vector3.up);
        //transform.rotation = Damp(transform.rotation, Quaternion.LookRotation(toFollow.forward, Vector3.up), camSmoothSpeed, Time.fixedDeltaTime);
    }

    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
