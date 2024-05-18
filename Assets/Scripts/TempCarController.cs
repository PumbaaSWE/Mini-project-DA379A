using UnityEngine;

public class TempCarController : MonoBehaviour
{

    [SerializeField] DumbCar car;
    [SerializeField] Transform carSpawn;
    Vector3 origin;
    public bool controling;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!car) { 
            if(!TryGetComponent(out car))
            {
                Debug.LogWarning("TempCarController - No Car found!");
            }
        }
        //if (!carSpawn) {
        //    carSpawn = transform;
        //}
        origin = transform.position;
    }

    //move this out when we have character controller
    void Update()
    {
        if (!controling) return;

        car.SetSteerInput(Input.GetAxis("Horizontal"));
        car.SetThrottleInput(Input.GetAxis("Vertical"));
        car.SetRevInput(Input.GetKey(KeyCode.LeftShift) ? 1 : 0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            car.SelfRight();


            //if (carSpawn)
            //{
            //    car.Teleport(carSpawn.position, carSpawn.rotation);
            //}
            //else
            //{
            //    car.Teleport(origin, Quaternion.identity);
            //}
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (carSpawn)
            {
                car.Teleport(carSpawn.position, carSpawn.rotation);
            }
            else
            {
                car.Teleport(transform.position+Vector3.up, Quaternion.LookRotation(transform.forward, Vector3.up));
            }
        }
    }

    public void StopControlling()
    {
        controling = false;
        //car.SetSteerInput(0);
        car.SetThrottleInput(0);
        car.SetRevInput(0);
    }
}
