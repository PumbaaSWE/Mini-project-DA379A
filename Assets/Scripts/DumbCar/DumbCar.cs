using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DumbCar : MonoBehaviour
{

    [Header("Setup")]
    public DumbWheel wheelPrefab;
    public float wheelBase = 3;
    public float wheelTrack = 1.8f;
    public float wheelHeightOffset = 1;
    public List<DumbWheel> wheels = new List<DumbWheel>();
    [SerializeField] private List<DumbWheel> drivenWheels = new List<DumbWheel>();


    [Header("Steering")]
    [Tooltip("Turn radius in meters")]
    public float turnRadius = 10;
    [Tooltip("v^2 is added to the turn radius with this multiplier")] 
    public float decreaseTurnBySpeed = 0;
    [Tooltip("How fast you can turn the stering wheel")]
    public float turnSpeed = 10;
    [Tooltip("How much ackermann to apply")]
    [SerializeField][Range(-1,1)]private float ackermannRatio = 1;

    public enum DriveType { ForwardWheelDrive, RearWheelDrive, FourWheelDrive };
    [Header("Throtteling")]
    public DriveType driveType = DriveType.RearWheelDrive;
    public float throttleSpeed = 10;
    public float power = 10000;

    [Header("Braking")]
    public float brakeSpeed = 10;
    public float brakeForce = 1000;

    [Header("Boost/Rev???")]
    public float maxRev = .5f;
    public float revSpeed = 10;


    //TODO: move this outa here
    [Header("Drive Train")]
    public float idleRpm = 900;
    public float maxRpm = 7000;
    public float[] gearRatios = new float[5] { 1, 2, 3, 4, 5 };
    public float[] gearRatiosDisp = new float[5];
    public float driveRatio = 4;
    public int currentGear = 1;
    public float currentRpm = 1;
    public float targetRpm = 1;

    [Header("Misc")]
    [SerializeField] Transform centerOfMass;
    [SerializeField] float selfRight = 1;

    //should be private
    [Header("For debug")]
    public float actualSteerInput = 0;
    public float targetSteerInput = 0;
    public float actualThrottleInput = 0;
    public float targetThrottleInput = 0;
    public float actualBrakeInput = 0;
    public float targetBrakeInput = 0;
    public float actualRevInput = 0;
    public float targetRevInput = 0;
    public Vector3 localVelocity;
    public Vector3 LocalVelocity => localVelocity;
    public float EngineRPM => currentRpm;
    public float MaxRPM => maxRpm;
    public float ThrottleInput => actualThrottleInput;

    public float MinRPM => idleRpm;


    //privates
    Rigidbody rb;
    bool hasTeleported;

    /// <summary>
    /// Steer the car from -1 to 1
    /// </summary>
    /// <param name="value"></param>
    public void SetSteerInput(float value)
    {
        targetSteerInput = Mathf.Clamp(value, -1, 1);
    }

    /// <summary>
    /// 0 to 1, amount to rev, not sure what "Rev" is for a name..
    /// </summary>
    /// <param name="value"></param>
    public void SetRevInput(float value)
    {
        targetRevInput = Mathf.Clamp01(value);
    }

    /// <summary>
    /// 1 for max throttle to 0 for nothing, then to -1 for brake->reverse. Any values between are fine.
    /// </summary>
    /// <param name="value"></param>
    public void SetThrottleInput(float value)
    {
        targetThrottleInput = Mathf.Clamp(value, -1, 1);
    }
    
    /// <summary>
    /// Teleport to a location
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="keepVelocities"></param>
    public void Teleport(Vector3 pos, Quaternion rot, bool keepVelocities = false)
    {
        if (keepVelocities)
        {
            rb.velocity = rot * LocalVelocity;
            rb.angularVelocity = rot * transform.InverseTransformDirection(rb.angularVelocity);
            
        }
        else
        {
            //rb.AddForce(-rb.velocity, ForceMode.VelocityChange);
            //rb.AddTorque(-rb.angularVelocity, ForceMode.VelocityChange);
            rb.velocity = rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            //rb.
        }
        hasTeleported = true;
        transform.SetPositionAndRotation(pos, rot);
    }

    //TODO: ADD A RESET FOR INPUTS, otherwise a teleport will keep throttle and steer and that might not always be desired (respawn for example!!)

    private void Steer(float dt)
    {
        actualSteerInput = Mathf.MoveTowards(actualSteerInput, targetSteerInput, turnSpeed * dt);

        float radius = turnRadius;
        if (decreaseTurnBySpeed != 0)
        {
            //radius += Mathf.Abs(localVelocity.z) * decreaseTurnBySpeed;
            radius += localVelocity.z * localVelocity.z * decreaseTurnBySpeed;
        }
            

        float sign = (wheelTrack / 2) * Mathf.Sign(actualSteerInput) * ackermannRatio;

        float left = Mathf.Atan(wheelBase / (radius + sign)) * actualSteerInput * Mathf.Rad2Deg;
        float right = Mathf.Atan(wheelBase / (radius - sign)) * actualSteerInput * Mathf.Rad2Deg;
        wheels[0].transform.localRotation = Quaternion.Euler(0, left, 0);
        wheels[1].transform.localRotation = Quaternion.Euler(0, right, 0);
    }

    //TODO, move rev here and combine as we did with break, they all deal with z-force
    private void Poweeeeeeer(float dt)
    {
        actualThrottleInput = Mathf.MoveTowards(actualThrottleInput, targetThrottleInput, throttleSpeed * dt);

        float power = driveType == DriveType.FourWheelDrive ? this.power/4 : this.power/2;

        power *= actualThrottleInput;
        //power += actualRevInput * power;

        float av = 0;
        bool shouldAddPower = true; //maybe make sure all wheels are reversable state---this is not used yet
        for (int i = 0; i < drivenWheels.Count; i++)
        {
            av = Mathf.Max(Mathf.Abs(drivenWheels[i].AngularVelocity), av);
            //if (Mathf.Sign(drivenWheels[i].AngularVelocity) != Mathf.Sign(actualThrottleInput) && Mathf.Abs(drivenWheels[i].AngularVelocity) > 0.1f)
            //{
            //    shouldAddPower = false;
            //}

            if (drivenWheels[i].AngularVelocity > -555.5f && actualThrottleInput > 0)
            {
                //we want forward to -> add power
                //maybe if boos only -- othweerwise breake first
               
            }else if (drivenWheels[i].AngularVelocity < .1f && actualThrottleInput < 0)
            {
                //we are going backward but want to go forward
            }
            else
            {
                shouldAddPower = false;
            }

        }

        //all drive wheels in agreement
        if (shouldAddPower)
        {
            for (int i = 0; i < drivenWheels.Count; i++)
            {
                drivenWheels[i].AddForce(power);
            }
        }
        else if (actualThrottleInput != 0) //we break with all wheels
        {
            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].AddBrakeForce(brakeForce);
            }
        }

    }

    //MOVE this to its own class
    private void UpdateDriveTrain(float dt)
    {
        targetRpm = idleRpm;
        if (targetThrottleInput > 0)
        {
            float maxAV = 0;
            if (driveType != DriveType.ForwardWheelDrive)
            {
                maxAV = Mathf.Max(wheels[2].AngularVelocity, wheels[3].AngularVelocity, maxAV);
            }
            if (driveType != DriveType.RearWheelDrive)
            {
                maxAV = Mathf.Max(wheels[0].AngularVelocity, wheels[1].AngularVelocity, maxAV);
            }
            float wheelrpm = maxAV / 2*Mathf.PI / 60; //rad/s to rpm
            targetRpm = wheelrpm * gearRatios[currentGear] * driveRatio;
            for (int i = 0; i < gearRatiosDisp.Length; i++)
            {
                gearRatiosDisp[i] = wheelrpm * gearRatios[i] * driveRatio;
            }
        }
        else
        {
            targetRpm = idleRpm;
        }
        if (currentRpm > maxRpm)
        {
            targetRpm = maxRpm - 100;
        }
        currentRpm = Mathf.MoveTowards(currentRpm, targetRpm, revSpeed * dt);
        
        if (targetRevInput < 1)
        {
            if(targetRpm < 1000)
            {
                currentGear--;
            }else if(targetRpm > 6000)
            {
                currentGear++;
            }
            currentGear = Math.Clamp(currentGear, 0, 4);
        }else
        {
            //currentGear = 0;
        }
    }

    //private void Brake(float dt)
    //{
    //    actualBrakeInput = Mathf.MoveTowards(actualBrakeInput, targetBrakeInput, brakeSpeed * dt);

    //    for (int i = 0; i < wheels.Count; i++)
    //    {
    //        wheels[i].AddBrakeForce(brakeForce * actualBrakeInput);
    //    }

    //}

    private void Rev(float dt)
    {
        actualRevInput = Mathf.MoveTowards(actualRevInput, targetRevInput, revSpeed * dt);

        if (driveType != DriveType.ForwardWheelDrive)
        {
            wheels[2].Rev(actualRevInput * maxRev);
            wheels[3].Rev(actualRevInput * maxRev);
        }
        if (driveType != DriveType.RearWheelDrive)
        {
            wheels[0].Rev(actualRevInput * maxRev);
            wheels[1].Rev(actualRevInput * maxRev);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(centerOfMass)
            rb.centerOfMass = centerOfMass.localPosition;

        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].Init();
        }
        SetDriveWheels();
    }






    // Update is called once per frame
    void FixedUpdate()
    {
        
        float dt = Time.fixedDeltaTime;
        UpdateState(dt);
        Steer(dt);
        Poweeeeeeer(dt);
 //       Brake(dt);
        Rev(dt);
        if (hasTeleported)
        {
            hasTeleported = false;
            return;
        }

        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].ComputeState(dt);
        }
        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].ApplyForces(dt);
        }
        UpdateDriveTrain(dt);
    }

    private void UpdateState(float dt)
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity);
    }



    private void OnValidate()
    {
        if (centerOfMass)
            GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
        SetDriveWheels();
    }
    public void Place()
    {
        if (!wheelPrefab)
        {
            Debug.LogWarning("No wheel prefab assigned");
            return;
        }
        if(wheels == null) wheels = new List<DumbWheel>();

        for (int i = 0; i < wheels.Count; i++)
        {
            DestroyImmediate(wheels[i]);
            Debug.Log("DestroyImmediate old wheels");
        }
        wheels.Clear();
        for (int i = 0; i < 4; i++)
        {
            DumbWheel dumbWheel = Instantiate(wheelPrefab, transform, false);
            wheels.Add(dumbWheel);
        }

        float hw = wheelTrack / 2;
        float hl = wheelBase / 2;
        wheels[0].transform.position = transform.position + new Vector3(-hw, -wheelHeightOffset, hl);
        wheels[0].name = "FrontLeft";
        wheels[1].transform.position = transform.position + new Vector3(hw, -wheelHeightOffset, hl);
        wheels[1].name = "FrontRight";
        wheels[2].transform.position = transform.position + new Vector3(-hw, -wheelHeightOffset, -hl);
        wheels[2].name = "RearLeft";
        wheels[3].transform.position = transform.position + new Vector3(hw, -wheelHeightOffset, -hl);
        wheels[3].name = "RearRight";
        SetDriveWheels();
    }
    public void SetDriveWheels()
    {
        drivenWheels.Clear();
        if (driveType != DriveType.ForwardWheelDrive)
        {
            drivenWheels.Add(wheels[2]);
            drivenWheels.Add(wheels[3]);
        }
        if (driveType != DriveType.RearWheelDrive)
        {
            drivenWheels.Add(wheels[0]);
            drivenWheels.Add(wheels[1]);
        }
    }

    public void SelfRight()
    {
        rb.AddTorque(rb.mass * selfRight * transform.up);
    }
}
