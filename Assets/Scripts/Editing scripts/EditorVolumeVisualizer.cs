using UnityEngine;

public class EditorVolumeVisualizer : MonoBehaviour
{
    [SerializeField]
    private float width = 1f;

    [SerializeField]
    private float height = 1f;

    [SerializeField]
    private float length = 1f;

    [SerializeField]
    private Node node;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(node != null)
        {
            if(!node.IsBlocked)
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(width, height, length));
            }
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, length));
        }
    }
}
