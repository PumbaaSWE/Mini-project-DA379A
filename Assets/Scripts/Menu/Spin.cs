using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    Vector3 degreesPerSecond;

    void Update()
    {
        transform.Rotate(degreesPerSecond * Time.deltaTime);
    }
}
