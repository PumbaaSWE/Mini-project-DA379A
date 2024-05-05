using UnityEngine;

public class EditorVolumeVisualizer : MonoBehaviour
{
    [SerializeField]
    private float width = 1f;

    [SerializeField]
    private float height = 1f;

    [SerializeField]
    private float length = 1f;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, length));
    }
}
