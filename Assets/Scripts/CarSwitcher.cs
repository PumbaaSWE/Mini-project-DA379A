using UnityEngine;

public class CarSwitcher : MonoBehaviour
{

    [SerializeField]DumbCar[] carPrefabs;
    int car = 0;
    DumbCar currentCar;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] DumbCarUI ui;

    [SerializeField]
    private Transform carSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SwapCar();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwapCar();
        }
    }

    void SwapCar()
    {
        if(currentCar) Destroy(currentCar.gameObject);
        
        if(carSpawn != null)
        {
            currentCar = Instantiate(carPrefabs[car], carSpawn.position, Quaternion.identity);
        }
        else
        {
            currentCar = Instantiate(carPrefabs[car], Vector3.zero, Quaternion.identity);
        }
        
        cameraFollow.ToFollow = currentCar.transform;
        if(ui)ui.car = currentCar;
        car = (car + 1) % carPrefabs.Length;
    }
}
