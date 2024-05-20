using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(DumbCar))]
class DumbCarEditor //: Editor
{
    public float value = 0;
    
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();
    //    DumbCar dumbCar = (DumbCar)target;
    //    if (GUILayout.Button("Place Wheels"))
    //    {
    //        Debug.Log("It's alive: " + target.name);
    //        //DumbCar dumbCar = (DumbCar)target;
    //        dumbCar.Place();
    //    }
    //    value = dumbCar.wheelHeightOffset;
    //}
}