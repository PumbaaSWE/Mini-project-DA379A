using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpringCamera : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField]Rigidbody toFollow;
    [SerializeField] float spring = 100;
    [SerializeField] float dampner = 100;
    [SerializeField] float lookAhead = 100;
    [SerializeField] Vector3 offset;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        if(!toFollow) enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateDirection(float dt)
    {
        float speed = toFollow.velocity.magnitude;
        if(speed < 0.001f)
        {
            direction = toFollow.transform.forward;
        }
        else
        {
            direction = toFollow.velocity.normalized;
        }
        direction = direction * lookAhead; //something speed also

        //create a rotational force maybe?
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void UpdateSpring(float dt)
    {
        Vector3 targetPos = toFollow.transform.position + transform.TransformDirection(offset);
        Vector3 delta = targetPos-transform.position;
        float dist = delta.magnitude;
        rb.AddForce(delta * spring - rb.velocity * dampner);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        UpdateSpring(dt);
        UpdateDirection(dt);
    }
}
