using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointRotation : MonoBehaviour
{
    [SerializeField]
    Transform cameraRot;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, cameraRot.rotation.z));
    }
}
