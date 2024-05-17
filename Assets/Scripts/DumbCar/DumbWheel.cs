using UnityEngine;

public class DumbWheel : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float wheelRadius = .33f;
    [SerializeField] Transform wheelModel;

    [Header("Springy things")]
    [SerializeField] float springForce = 19000;
    [SerializeField] float springMin = .2f;
    [SerializeField] float springMax = .3f;
    [SerializeField] float springRestLength = .5f;
    [SerializeField] float damperForce = 1900;

    //[Header("Forward Friction things")]
    //[SerializeField] float dynamicFriction = .5f;
    //[SerializeField] float staticFriction = 1f;

    [Header("Sideways Friction things")]
    [SerializeField] float sidewayDynamicSpeed = 5;
    [SerializeField] float sidewayDynamicFriction = .5f;
    [SerializeField] float sidewayStaticSpeed = 1f;
    [SerializeField] float sidewayStaticFriction = 1f;


    [Header("Info things")]
    public Vector3 wheelVelocity;
    public float rotationalSpeed = 0;
    public float angularVelocity = 0;
    public float slipRatio = 0;
    public float rev;
    public bool debugPrint = false;

    float lastLength;
    public Vector3 position;
    Vector3 wheelPosition;
    float netSpringForce;
    //float usefullMass;
    bool grounded;
    Rigidbody rb;
    float accumulatedForce;

    public bool IsGrounded => grounded;
    public float AngularVelocity => angularVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }


    public void ComputeState(float dt)
    {
        grounded = false;
        position = transform.position - (springRestLength + springMax + wheelRadius) * transform.up;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, springRestLength + springMax + wheelRadius, layerMask))
        {
            grounded = true;
            position = hit.point;


            //Compute data for calculations
            float length = hit.distance - wheelRadius;
            length = Mathf.Clamp(length, springRestLength - springMin, springRestLength + springMax);
            float delta = springRestLength - length;
            float springVel = (lastLength - length) / dt;
            lastLength = length;

            //compute correctional spring force
            float actualSpringForce = delta * springForce;
            float actualDamperForce = springVel * damperForce;
            netSpringForce = actualSpringForce + actualDamperForce;

            //compute more data for "friction"
            wheelVelocity = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            rotationalSpeed = wheelVelocity.z * 360 / (2 * wheelRadius * Mathf.PI) * dt; //this is how fast the wheel wants to rotate in degree/sec (free spin, NO torque)

            //correct friction with gravithtyty
            //float gravity = Vector3.Dot(hit.normal, -Physics.gravity);

            angularVelocity = wheelVelocity.z / wheelRadius; //angular Velocity in radians? if v=wr => w=v/r then yes?


            //More frictional things
            //float slipRatio = angularVelocity * wheelRadius / wheelVelocity.z - 1; //this is 0 if no slip?
            //rb.AddForceAtPosition(wheelVelocity.x * transform.right, hit.point, ForceMode.VelocityChange);
        }
        else
        {
            rotationalSpeed = Mathf.MoveTowards(rotationalSpeed, 0, dt);
            //angularVelocity = Mathf.MoveTowards(angularVelocity, 0, dt);
            lastLength = Mathf.Lerp(lastLength, springRestLength, dt);
        }

        

        Debug.DrawLine(transform.position, position, grounded ? Color.red: Color.green);
    }

    void FixedUpdate()
    {
       //Not calling anything here as I want specific update sequence
        // ComputeState(Time.fixedDeltaTime);
       // if(grounded)ApplyForces();
    }

    public void ApplyForces(float dt)
    {
        if (grounded)
        {
            //use Mathf.Max(netSpringForce, 0) or it will stick to the ground
            rb.AddForceAtPosition(transform.up * netSpringForce, transform.position);


            //anti gravity
            float gravityRight = -Vector3.Dot(transform.right, Physics.gravity / 4); //CLAMP THIS! Also fix div by 4, since 4 wheels, hardcoding values since4ever!!!! Maybe we can compute based on spring force insted to decouple?
            float gravityFwd = -Vector3.Dot(transform.forward, Physics.gravity / 4); //CLAMP THIS!
            rb.AddForceAtPosition(gravityRight * transform.right + gravityFwd * transform.forward, position, ForceMode.Acceleration);

            //roll resistance, increases as we slow to stop the car when going really slow
            float rollResistance = 1 / (wheelVelocity.z * wheelVelocity.z + 1); //could be based on angularVelocity also


            float normalForceRatio = 1 - netSpringForce / springForce;
            Print("normalForceRatio = " + normalForceRatio + " netSpringForce = " + netSpringForce);
            float totalSlip = Mathf.Sqrt(wheelVelocity.x * wheelVelocity.x + wheelVelocity.z * wheelVelocity.z); //why?
            Print("totalSlip = " + totalSlip);
            /**
             *  compute how much (sideway) slip we have based on velocity and friction values (dynamic vs static)
             *  
             *  Ideally we should incorperate the wheel load (substitute normal force) for less slip with more force 
             *
             */
            float slip = Remap2(Mathf.Abs(wheelVelocity.x), sidewayStaticSpeed, sidewayDynamicSpeed, sidewayStaticFriction, sidewayDynamicFriction);
            //Debug.Log("sideSlip=" + sideSlip);
            //if (accumulatedForce != 0)
            //{
            //    sideSlip = 1 - rev * 0.5f;
            //    fwdSlip = 1 - rev * 0.2f;
            //}
            //float fwdSlip = 1;


            /**
             * Rev is how much we slide extra, applied to the slip for things like drifting
             * Was originnally for the engine to apply more power thus breaking traction that way
             * But we just cheat because game
             * 
             * */
            if (rev > 0)
            {
                slip *= 1-rev; //half the friction? or whatever? I just make shit up as I go
            }

            

            //if(rev > 0 && accumulatedForce > 0)
            //{
            //        slip *= .5f;
            //    if(10 > Mathf.Abs(wheelVelocity.z))
            //    {
            //    }
            //    else
            //    {
            //        fwdSlip *= .5f;
            //    }
            //    accumulatedForce *= 1.5f;
            //}

            //if(accumulatedForce != 0)
            //{
            //    angularVelocity += accumulatedForce/netSpringForce;
            //    Debug.Log(" = " + accumulatedForce / netSpringForce);
            //}

            slipRatio = slip;


            //Compute the sideways and fwd accelerations, i.e. not dependant on the mass of vehicle-> we slide based on speed, see above slip-value
            Vector3 sideAccel = wheelVelocity.x * slip * -transform.right;
            Vector3 fwdAccel = wheelVelocity.z  * rollResistance * -transform.forward;
            rb.AddForceAtPosition(sideAccel + fwdAccel, position, ForceMode.Acceleration);

            //driving forces
            if(accumulatedForce != 0)
            {
                if (accumulatedForce > netSpringForce) //Friction <= F*µ, but 
                {
                    angularVelocity += accumulatedForce/wheelRadius;
                }
                //angularVelocity += Mathf.Max((accumulatedForce / netSpringForce*slip) - 1 , 0) / dt;
                Print("accumulatedForce = " + accumulatedForce + ", netSpringForce * slip = " + netSpringForce * slip);
                rb.AddForceAtPosition(transform.forward * accumulatedForce, position);
            }

            Debug.DrawLine(transform.position, transform.position + transform.forward * accumulatedForce, Color.yellow);
        }

        //cosmetic wheel manipulation
        wheelModel.transform.Rotate(angularVelocity*Mathf.Rad2Deg*dt, 0, 0);
        wheelModel.transform.position = transform.position - (lastLength * transform.up);

        //reset the forces!
        accumulatedForce = 0;
        //TheFORCE = Vector3.zero;
    }


    public void AddForce(float value)
    {
        accumulatedForce += value;
    }

    public void AddBrakeForce(float value)
    {
        accumulatedForce -= value*wheelVelocity.z;
    }

    public void AddAcceleration(float value)
    {

    }

    internal void Rev(float v)
    {
        rev = v;
    }

    public static float Remap(float value, float low1, float high1, float low2, float high2)
    {
        value = Mathf.Clamp(value, low1, high1);
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    public void Print(string s)
    {
        if(debugPrint)Debug.Log(s);
    }

    public static float Remap2(float value, float a, float b, float x, float y)
    {
        return Mathf.Lerp(x, y, Mathf.InverseLerp(a, b, value));
    }

    internal void Init()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
}
