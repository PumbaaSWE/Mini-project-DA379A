using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Setup")]
    public GameObject player;
    //public Transform holdPos;
    //public float throwForce = 500f;
    public float pickUpRange = 3f;
    public float pickUpRadius = .5f;
    public TextMeshProUGUI text;

    [Header("Trunk")]
    public Package packagePrefab;
    private Package package;
    public LayerMask layerMask;
    bool shouldHelpTextBeEnabled;
    [Header("Animation")]
    public AnimController animController;

    RaycastHit[] hits = new RaycastHit[20];

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
        int n = Physics.SphereCastNonAlloc(transform.position - transform.forward * pickUpRadius, pickUpRadius, transform.forward, hits, pickUpRange, 0xFFFF, QueryTriggerInteraction.Ignore);

        Transform best = null;
        float bestDistance = float.MaxValue;
        for (int i = 0; i < n; i++)
        {
            if (hits[i].transform.gameObject.CompareTag("canPickUp"))
            {
                if(hits[i].distance < bestDistance)
                {
                    bestDistance = hits[i].distance;
                    best = hits[i].transform;
                }
                canPickUp = true;
            }
        }
        if (canPickUp)
        {
            heldObjRb = best.GetComponent<Rigidbody>();
            HelpText("Press [F] to pick up");
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
                    package = Instantiate(packagePrefab, transform.position, Quaternion.identity);
                    animController.PickUp(package.GetComponent<Rigidbody>());
                    //PickUpObject( package.gameObject);
                    return true;
                }
                HelpText("Press [F] to pick up a new package");
            }
            else if(!animController.IsHolding)
            {
                HelpText("Deliver current package!");
            }
        }
        return false;
    }

    public void LateUpdate()
    {
        //if (!thrown && package)
        //{

        //    package.transform.SetLocalPositionAndRotation(animController.HoldPos.position, animController.HoldPos.rotation);

        //}
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


    private void OnDisable()
    {
        HelpText(false);
    }
}
