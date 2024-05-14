using UnityEngine;

public class DeactivateEditorObject : MonoBehaviour
{
    [SerializeField]
    private GameObject editorObject;

    // Start is called before the first frame update
    void Start()
    {
        if(editorObject != null)
        {
            editorObject.SetActive(false);
        }
    }
}
