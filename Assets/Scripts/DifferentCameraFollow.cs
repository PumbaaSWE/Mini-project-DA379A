using UnityEngine;

/// <summary>
/// 
/// For this script to work, there needs to be a disabled CameraFollow script already on the Camera-GO, so that we can get toFollow.
/// We also need layers to know what the camera should and shouldn't collide with.
/// 
/// offset.x current doesn't do anything.
/// offset.y is set before pivoting
/// offset.z is set after pivoting
/// 
/// </summary>
public class DifferentCameraFollow : MonoBehaviour
{
    [SerializeField] Transform toFollow;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask cameraCollidesWith;
    [SerializeField] float sensitivity;
    [SerializeField] float xRotationMin, xRotationMax;

    Vector2 additionalRotation;

    void Update()
    {
        if (!toFollow)
        {
            //get subject
            if (TryGetComponent(out CameraFollow cameraFollow))
            {
                if (cameraFollow.ToFollow)
                    toFollow = cameraFollow.ToFollow;
            }

            return;
        }

        //lock da mouse
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;

        //add user input to eulerRotation
        additionalRotation.x = Mathf.Clamp(additionalRotation.x - Input.GetAxis("Mouse Y") * sensitivity, xRotationMin, xRotationMax);
        additionalRotation.y = (additionalRotation.y + Input.GetAxis("Mouse X") * sensitivity) % 360f;

        //zoom in and out with scroll yo
        offset.z = Mathf.Min(offset.z + Input.mouseScrollDelta.y, 0f);

        //more dynamic camera
        //transform.rotation = toFollow.rotation;

        //more static camera
        transform.rotation = Quaternion.Euler(new Vector2(0f, toFollow.eulerAngles.y));

        //add user made rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3)additionalRotation);

        //move camera above subject
        transform.position = toFollow.position + Vector3.up * offset.y;

        //check for obstructions
        Ray ray = new Ray(transform.position, transform.forward * offset.z);
        float radius = 1f;
        float maxDistance = Mathf.Abs(offset.z);
        bool hitSomething = Physics.SphereCast(ray, radius, out RaycastHit hitInfo, maxDistance, cameraCollidesWith);
        if (hitSomething)
        {
            //move camera from obstructions
            transform.position += transform.forward * hitInfo.distance * Mathf.Sign(offset.z);

            //check for obstructions along the obstruction (if that makes sense!)
            //I don't like how it looks but some might prefer it - let's comment it out for now =]
            
            //Vector3 dir = Vector3.ProjectOnPlane(ray.direction, hitInfo.normal);
            //Debug.DrawRay(transform.position, dir, Color.red);
            //ray = new Ray(transform.position, dir);
            //maxDistance = Mathf.Abs(offset.z) - hitInfo.distance;
            //bool hitSomethingAgain = Physics.SphereCast(ray, radius, out hitInfo, maxDistance, cameraCollidesWith);
            //if (hitSomethingAgain)
            //{
            //    transform.position += dir * hitInfo.distance;
            //}
            //else
            //{
            //    transform.position += dir * maxDistance;
            //}
        }
        else
        {
            //no obstructions
            transform.position += transform.forward * offset.z;
        }
    }
}
