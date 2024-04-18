using UnityEditor;
using UnityEngine;

public class RigidbodyFinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemapJoints()
    {
        Rigidbody rb = GetComponentInParent<Rigidbody>();
        Joint[] joints = GetComponentsInChildren<Joint>();
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].connectedBody = rb;
        }
    }
}
[CustomEditor(typeof(RigidbodyFinder))]
class RigidbodyFinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Test"))
            ((RigidbodyFinder)target).RemapJoints();
    }
}
