using UnityEngine;

public class EditorGrabObject : MonoBehaviour
{
    [SerializeField]
    private GameObject grabObject;

    [SerializeField]
    private Node node;

    private bool inEditingMode = true;

    private void Awake()
    {
        inEditingMode = false;
    }

    private void OnDrawGizmos()
    {
        if(inEditingMode)
        {
            if (node != null)
            {
                if (grabObject != null)
                {
                    if (node.IsBlocked)
                    {
                        grabObject.SetActive(false);
                    }
                    else
                    {
                        grabObject.SetActive(true);
                    }
                }
            }
        }        
    }
}
