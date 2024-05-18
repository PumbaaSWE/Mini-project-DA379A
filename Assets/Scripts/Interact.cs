using TMPro;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject player;
    //[SerializeField] CameraFollow carCamera;
    [SerializeField] DifferentCameraFollow differentCamera;
    [SerializeField] MoveCamera firstPersonCamera;


    [Header("Vehicles")]
    [SerializeField] private LayerMask vehicleLayer;
    [SerializeField] private float vehicleRange = 4;

    bool inCar = false;
    TempCarController car;
    Transform carDoorPos;

    [Header("Items")]
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private float itemRange = 3;
    [SerializeField] private Package item;

    [Header("Special")]
    [SerializeField] private LayerMask specialLayer;
   // [SerializeField] private float specialRange = 3;
    [SerializeField] private Package packagePrefab;
    [SerializeField] private Package package;

    [Header("Tooltip")]
    [SerializeField] private TextMeshProUGUI helpText;
    bool shouldHelpTextBeEnabled;

    public enum State { InCar, Holding, Free};
    private State state = State.Free;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        shouldHelpTextBeEnabled = false;

        switch (state)
        {
            case State.InCar:
                UpdateInCar();
                break;
            case State.Holding:
                UpdateHolding();
                break;
            case State.Free:
                break;
            default:
                break;
        }
        HelpText(shouldHelpTextBeEnabled);
    }

    private void UpdateInCar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DoExit();
        }
    }

    private void UpdateHolding()
    {
        
    }

    private bool CheckItems()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, itemRange, itemLayer, QueryTriggerInteraction.Collide))
        {
            //if (((1 << hit.collider.gameObject.layer) & layerMask) != 0)
            //{
            //    //It matched
            //}
            if (!package)
            {
                
                HelpText("Press [F] to pick up package");
            }
        }
        return false;
    }

    bool CheckVehicle() {
        if (!inCar)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), vehicleRange, vehicleLayer, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hits.Length; i++)
            {
                car = hits[i].transform.gameObject.GetComponentInParent<TempCarController>();
                if (car)
                {
                    carDoorPos = hits[i].transform;
                    return true;
                }
            }
        }
        
        return false;
    }

    private void DoExit()
    {
        RaycastHit[] hits = Physics.RaycastAll(carDoorPos.transform.position, -car.transform.right, 2);
        if (hits.Length > 0)
        {
            Debug.Log("Somthing is in the way of the driver door!!");
            return;
        }

        player.transform.position = carDoorPos.transform.position - car.transform.right * 3;
        player.SetActive(true);

        //car.controling = false;
        car.StopControlling();
        firstPersonCamera.enabled = true;
        //carCamera.enabled = false;
        differentCamera.enabled = false;
        inCar = false;
    }

    private void DoEnter()
    {
        player.SetActive(false);
        //carCamera.toFollow = car.transform;
        differentCamera.ToFollow = car.transform;
        car.controling = true;
        firstPersonCamera.enabled = false;
        //carCamera.enabled = true;
        differentCamera.enabled = true;
        inCar = true;
    }

    void HelpText(bool b)
    {
        if (helpText) helpText.enabled = b;
    }

    void HelpText(string s = "")
    {
        if (helpText)
        {
            shouldHelpTextBeEnabled = true;
            helpText.text = s;
        }
    }
}
