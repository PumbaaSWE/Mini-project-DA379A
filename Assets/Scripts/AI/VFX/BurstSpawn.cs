using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BurstSpawn : MonoBehaviour
{
    [SerializeField]
    private VisualEffect visualEffect;

    [SerializeField]
    private float timeUntilDeletion = 1f;

    // Start is called before the first frame update
    void Start()
    {
        visualEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilDeletion -= Time.deltaTime;

        if(timeUntilDeletion <= 0)
        {
            Destroy(gameObject);
        }
    }
}
