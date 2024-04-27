using UnityEngine;
using UnityEngine.InputSystem;

public class PathBlocker : MonoBehaviour
{
    [SerializeField]
    private Node from;

    [SerializeField]
    private Node to;

    private ConditionResult walkableCondition;

    [SerializeField]
    private GameObject stopIndicator;

    private Material materialInstance;

    [SerializeField]
    private Color moveColor;

    [SerializeField]
    private Color stopColor;

    private Player_Actions actions;

    // Start is called before the first frame update
    void Start()
    {
        materialInstance = stopIndicator.GetComponent<MeshRenderer>().material;

        int adjacencyIndex = from.GetAdjacent().IndexOf(to);

        walkableCondition = from.GetWalkableConditions()[adjacencyIndex];

        SetToMovable(new InputAction.CallbackContext());

        actions = new();

        actions.Player1.Test_Action.performed += SetToMovable;
        actions.Player1.Anti_Test_Action.performed += SetToStopped;

        actions.Player1.Enable();
    }

    private void OnDisable()
    {
        actions.Player1.Test_Action.performed -= SetToMovable;
        actions.Player1.Anti_Test_Action.performed -= SetToStopped;
    }

    public void SetToMovable(InputAction.CallbackContext context)
    {
        materialInstance.color = moveColor;
        walkableCondition.Result = true;
    }

    public void SetToStopped(InputAction.CallbackContext context)
    {
        materialInstance.color = stopColor;
        walkableCondition.Result = false;
    }
}
