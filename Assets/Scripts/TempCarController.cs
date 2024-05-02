using UnityEngine;

public class TempCarController : MonoBehaviour
{

    [SerializeField] DumbCar car;
    [SerializeField] Transform carSpawn;
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
            if (carSpawn)
            {
                car.Teleport(carSpawn.position, carSpawn.rotation);
            }
            else
            {
                car.Teleport(Vector3.zero, Quaternion.identity);
            }
        }
    }
}
