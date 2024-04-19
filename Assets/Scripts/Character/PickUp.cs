using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("HoldLayer");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw
            {
                StopClipping();
                ThrowObject();
            }
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform;
            heldObj.layer = LayerNumber;

            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;
    }
    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
    }
    
    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
