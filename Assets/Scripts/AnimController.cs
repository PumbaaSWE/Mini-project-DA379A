using System.Collections;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Animator animator;
    public float pickDelay = 0.5f;
    public float throwDelay = 0.06f;
    //public Rigidbody packetPrefab;
    Rigidbody packet;
    public float force = 100;
    public Transform holdPos;
    bool thrown;
    public bool IsHolding => packet != null && !thrown;

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


        
        animator.SetBool("Holding", (packet != null) && !thrown);
        

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
        StartCoroutine(Throw(throwDelay));
    }


    public void LateUpdate()
    {
        if (!thrown && packet)
        {

            packet.transform.SetLocalPositionAndRotation(holdPos.position, holdPos.rotation);
            
        }
    }

    IEnumerator PickUp(float t, Rigidbody body)
    {
        yield return new WaitForSeconds(t);
        if (packet == null)
        {
            //packet = Instantiate(packetPrefab);
            packet = body;
            packet.isKinematic = true;
            thrown = false;
        }

    }

    IEnumerator Throw(float t)
    {
        yield return new WaitForSeconds(t);
        Throw();
        
    }

    private void Throw()
    {
        if(!packet) return;
        thrown = true;
        packet.isKinematic = false;
        packet.AddForce(transform.forward * force);
        Debug.DrawRay(packet.position, transform.forward, Color.red, 5);
        packet = null;
    }
}
