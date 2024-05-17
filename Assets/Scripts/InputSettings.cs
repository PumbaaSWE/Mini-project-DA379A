using UnityEngine;


[CreateAssetMenu(fileName = "InputSettings", menuName = "ScriptableObjects/InputSettings", order = 1)]
public class InputSettings : ScriptableObject
{
    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode interact = KeyCode.E;
}
