using System.Collections;
using UnityEngine;


/*
 * This class should not hadle packages, only animations
 * Use events or callbacks to know when pick up and throw anims are in the correct place
 * */
public class AnimController : MonoBehaviour
{
    public Animator animator;
    public float pickDelay = 0.5f;
    public float throwDelay = 0.06f;
    //public Rigidbody packetPrefab;
    public Rigidbody packet;
    public float force = 100;
    public float upForce = 100;
    public Transform holdPos;
    bool thrown;
    public bool IsHolding => packet != null && !thrown;

    public delegate void OnThrow();
    public event OnThrow OnThrowCallback;

    public delegate void OnPickUp();
    public event OnPickUp OnPickUpCallback;

    public Transform HoldPos => holdPos;

    // Start is called before the first frame update
    void Start()
    {
        if(!animator)animator = GetComponentInChildren<Animator>();
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if(transforms[i].gameObject.name == "HoldPos")
            {
                holdPos = transforms[i];
            }
        }

    }


    // Update is called once per frame
    void Update()
    {


        
        animator.SetBool("Holding", IsHolding);
        //if(packet.gameObject.activeSelf)

        //if(packet == null)
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    animator.SetTrigger("Hold");
        //}
    }

    public void Reach()
    {
        animator.SetTrigger("Pick");
    }
    public void PickUp(Rigidbody body)
    {
        animator.SetTrigger("Pick");
        if(body)StartCoroutine(PickUp(pickDelay, body));
    }

    public void DoThrow()
    {
        animator.SetTrigger("Throw");
        if (!packet) return;
        if (packet.gameObject.activeSelf)
        {
            StartCoroutine(Throw(throwDelay));
        }
    }


    public void LateUpdate()
    {
        if (!thrown && packet)
        {

            packet.transform.SetLocalPositionAndRotation(holdPos.position, holdPos.rotation);
            
        }
    }

    private void OnDisable()
    {
        if(packet)
            packet.gameObject.SetActive(false);
        if (animator)
            animator.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        if (packet)
            packet.gameObject.SetActive(true);
        if (animator)
            animator.gameObject.SetActive(true);
    }

    IEnumerator PickUp(float t, Rigidbody body)
    {
        yield return new WaitForSeconds(t);
        OnPickUpCallback?.Invoke();
        if (packet == null)
        {
            //packet = Instantiate(packetPrefab);
            packet = body;
            if (!enabled) packet.gameObject.SetActive(false);
            packet.isKinematic = true;
            thrown = false;
        }

    }

    IEnumerator Throw(float t)
    {
        yield return new WaitForSeconds(t);
        OnThrowCallback?.Invoke();
        Throw();
    }

    private void Throw()
    {
        //if(!packet) return;
        thrown = true;
        packet.isKinematic = false;
        packet.AddForce(transform.forward * force + Vector3.up * upForce);
        Debug.DrawRay(packet.position, transform.forward, Color.red, 5);
        packet = null;
    }
}
