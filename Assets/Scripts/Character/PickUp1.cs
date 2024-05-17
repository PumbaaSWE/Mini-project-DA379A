using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUp1 : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private IThrowable heldObjectThrowable;
    private bool canDrop = true;
    public TextMeshProUGUI text;

    [Header("Test")]
    public Package packagePrefab;
    public Package package;
    public LayerMask layerMask;
    bool shouldHelpTextBeEnabled;


    void Update()
    {
        shouldHelpTextBeEnabled = false;


        if (Interact())
        {
            return;
        }
        HelpText();


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickUpRange, 0xFFFF, QueryTriggerInteraction.Ignore))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.CompareTag("canPickUp"))
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

        HelpText(shouldHelpTextBeEnabled);
    }


    bool Interact()
    {
        //HelpText(false);
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, pickUpRange, layerMask, QueryTriggerInteraction.Collide))
        {
            //if (((1 << hit.collider.gameObject.layer) & layerMask) != 0)
            //{
            //    //It matched
            //}
            if (!package)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    package = Instantiate(packagePrefab, holdPos);
                    PickUpObject( package.gameObject);
                    return true;
                }
                HelpText("Press [F] to pick up package");
            }
        }
        return false;
    }

    void HelpText(string s = "")
    {
        if (text)
        {
            shouldHelpTextBeEnabled = true;
            text.text = s;
        }
    }
    void HelpText(bool b)
    {
        if (text) text.enabled = b;
    }


    void HelpText()
    {
        
        if(!text) return;
        if (heldObj == null && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit0, pickUpRange,0xFFFF, QueryTriggerInteraction.Ignore))
        {
            //make sure pickup tag is attached
            if (hit0.transform.gameObject.CompareTag("canPickUp"))
            {
                HelpText("Press [RMB] to pick up package");
            }
        }
    }


    void PickUpObject(GameObject pickUpObj)
    {
        Rigidbody rb = pickUpObj.GetComponent<Rigidbody>();
        if (rb)
        {
            heldObj = pickUpObj;
            heldObjRb = rb;
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform;

            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

            heldObjectThrowable = pickUpObj.GetComponent<IThrowable>();

            heldObjectThrowable?.PerformGrabLogic();
        }
    }
    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
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
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;

        heldObjectThrowable?.PerformThrowLogic();
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
