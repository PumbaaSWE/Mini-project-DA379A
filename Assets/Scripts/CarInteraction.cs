using System;
using TMPro;
using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    [SerializeField] float interactionRange;
    [SerializeField] GameObject player;
    [SerializeField] CameraFollow carCamera;
    [SerializeField] DifferentCameraFollow differentCamera;
    [SerializeField] MoveCamera firstPersonCamera;
    [SerializeField] LayerMask layerMask;
    [SerializeField] private TextMeshProUGUI text;

    bool inCar = false;

    TempCarController car;
    Transform carDoorPos;

    public void Start()
    {
        if(!differentCamera)
            differentCamera = GetComponentInParent<DifferentCameraFollow>(true);
    }
    // Update is called once per frame
    void Update()
    {
        Interact();


        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if(!inCar)
        //    {
        //        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), interactionRange, layerMask);

        //        for (int i = 0; i < hits.Length; i++)
        //        {
        //            car = hits[i].transform.gameObject.GetComponentInParent<TempCarController>();
        //            if (car)
        //            {
        //                //car = hits[i].transform.gameObject.GetComponentInParent<TempCarController>();
        //                carDoorPos = hits[i].transform;
        //                player.SetActive(false);
        //                //carCamera.toFollow = car.transform;
        //                differentCamera.ToFollow = car.transform;

        //                car.controling = true;
        //                firstPersonCamera.enabled = false;
        //                //carCamera.enabled = true;
        //                differentCamera.enabled = true;

        //                inCar = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        RaycastHit[] hits = Physics.RaycastAll(carDoorPos.transform.position, -car.transform.right, 2);
        //        if (hits.Length > 0)
        //        {
        //            Debug.Log("Somthing is in the way of the driver door!!");
        //            return;
        //        }

        //        player.transform.position = carDoorPos.transform.position - car.transform.right * 3;
        //        player.SetActive(true);

        //        //car.controling = false;
        //        car.StopControlling();
        //        firstPersonCamera.enabled = true;
        //        carCamera.enabled = false;
        //        differentCamera.enabled = false;
        //        inCar = false;
        //    }
        //}
    }


    void Interact()
    {
        
        bool canEnter = false;
        if (!inCar)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), interactionRange, layerMask, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hits.Length; i++)
            {
                car = hits[i].transform.gameObject.GetComponentInParent<TempCarController>();
                if (car)
                {
                    carDoorPos = hits[i].transform;
                    canEnter = true;
                }
            }
        }
        ShowHelp(canEnter);
        if (Input.GetKeyDown(KeyCode.E)){
            if (canEnter)
            {
                DoEnter();
            }
            else if(inCar)
            {
                DoExit();
            }
        }
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
        carCamera.enabled = false;
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

    void ShowHelp(bool showText)
    {
        if (!text) return;
        if (showText)
        {
            text.text = "Press [E] to enter";

        }
        text.enabled = showText;
    }
}
