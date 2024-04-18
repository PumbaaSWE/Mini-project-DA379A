using UnityEngine;

public class DumbCarTrail : MonoBehaviour
{

    public ParticleSystem particles;

    public TrailRenderer trailRenderer;
    public DumbWheel wheel;
    public float slipThreshold = 0.5f;
    public float offset = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        wheel = GetComponentInParent<DumbWheel>();
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = wheel.position + Vector3.up * offset;
        if(wheel.slipRatio < slipThreshold)
        {
            trailRenderer.emitting = true;
            if (particles)
            {
                var em = particles.emission;
                em.enabled = true;
            }
        }
        else
        {
            trailRenderer.emitting = false;
            if (particles)
            {
                var em = particles.emission;
                em.enabled = false;
            }
        }

        if (!wheel.IsGrounded)
        {
            trailRenderer.emitting = false;
            if (particles)
            {
                var em = particles.emission;
                em.enabled = false;
            }
        }
    }
}
