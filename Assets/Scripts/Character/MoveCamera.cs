using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    [SerializeField] bool useLate = false;

    void Update()
    {
        if(!useLate)transform.position = cameraPosition.position;
    }

    void LateUpdate()
    {
        if (useLate) transform.position = cameraPosition.position;
    }
}
