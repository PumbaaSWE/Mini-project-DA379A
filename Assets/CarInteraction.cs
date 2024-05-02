using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    [SerializeField] float interactionRange;
    [SerializeField] GameObject player;
    [SerializeField] CameraFollow carCamera;
    [SerializeField] MoveCamera firstPersonCamera;

    bool inCar = false;

    TempCarController car;
    Transform carDoorPos;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!inCar)
            {
                RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), interactionRange);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.CompareTag("DriverDoor"))
                    {
                        car = hits[i].transform.gameObject.GetComponentInParent<TempCarController>();
                        carDoorPos = hits[i].transform;
                        player.SetActive(false);
                        carCamera.toFollow = car.transform;

                        car.controling = true;
                        firstPersonCamera.enabled = false;
                        carCamera.enabled = true;

                        inCar = true;
                    }
                }
            }
            else
            {
                RaycastHit[] hits = Physics.RaycastAll(carDoorPos.transform.position, -car.transform.right, 2);
                if (hits.Length > 0)
                {
                    Debug.Log("Somthing is in the way of the driver door!!");
                    return;
                }

                player.transform.position = carDoorPos.transform.position - car.transform.right * 3;
                player.SetActive(true);

                car.controling = false;
                firstPersonCamera.enabled = true;
                carCamera.enabled = false;

                inCar = false;
            }
        }
    }
}
