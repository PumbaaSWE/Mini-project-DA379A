using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Setup")]
    public GameObject player;
    //public Transform holdPos;
    //public float throwForce = 500f;
    public float pickUpRange = 3f;
    public TextMeshProUGUI text;

    [Header("Trunk")]
    public Package packagePrefab;
    private Package package;
    public LayerMask layerMask;
    bool shouldHelpTextBeEnabled;
    [Header("Animation")]
    public AnimController animController;



    //private float frameTime = 1.4f;
    //[SerializeField] Animator animator;
    //[SerializeField] GameObject handPos;

    void Start()
    {
        if (!animController)
            animController = GetComponent<AnimController>();
    }

    void Update()
    {
        shouldHelpTextBeEnabled = false;


        if (InteractWithStorage())
        {
            return;
        }
        //HelpText();

        if (animController.IsHolding)
        {
            HandleHolding();
        }
        else
        {
            HandlePickUp();
        }
        HelpText(shouldHelpTextBeEnabled);
    }

    private void HandleHolding()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            animController.DoThrow();
        }
    }

    void HandlePickUp()
    {
        Rigidbody heldObjRb = null;
        bool canPickUp = false;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickUpRange, 0xFFFF, QueryTriggerInteraction.Ignore))
        {
            //make sure pickup tag is attached
            if (hit.transform.gameObject.CompareTag("canPickUp"))
            {
                canPickUp = true;
                heldObjRb = hit.transform.GetComponent<Rigidbody>();
                HelpText("Press [F] to pick up");
            }
        }
        if (canPickUp && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0)))
        {
            animController.PickUp(heldObjRb); 
        }
    }

    bool InteractWithStorage()
    {
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, pickUpRange, layerMask, QueryTriggerInteraction.Collide))
        {
            if (!package)
            {
                if (packagePrefab && Input.GetKeyDown(KeyCode.F))
                {
                    package = Instantiate(packagePrefab);
                    animController.PickUp(package.GetComponent<Rigidbody>());
                    //PickUpObject( package.gameObject);
                    return true;
                }
                HelpText("Press [F] to pick up a new package");
            }
            else
            {
                HelpText("Deliver current package!");
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

}
