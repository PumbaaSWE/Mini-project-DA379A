using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    [SerializeField]
    private Task behaviourTree;

    private NavMeshAgent agent;

    [SerializeField]
    private Node startNode;

    [SerializeField]
    private Node[] goals;

    private int goalIndex = 0;

    private Node currentNode = null;

    private Node nextNode = null;
    private Vector3 nextPosition;

    private List<Node> path = new();

    private int nextPathNodeIndex;

    [Range(0f, 100f)]
    [SerializeField]
    private float timeBetweenTicks;

    private float currentTimeUntilTick = 0.0f;

    [SerializeField]
    private Graph graph;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentNode = startNode;

        if(goals.Length < 2)
        {
            Debug.LogError("NPC has less than two goals!");
        }
    }

    private void Update()
    {
        Tick();
    }

    public void Tick()
    {
        if (currentTimeUntilTick <= 0)
        {
            behaviourTree.Tick();
            currentTimeUntilTick = timeBetweenTicks;
        }
        else
        {
            currentTimeUntilTick -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Path"))
        {
            Node otherNode = other.GetComponent<Node>();

            if(otherNode == nextNode)
            {
                currentNode = nextNode;
                nextNode = null;
            }
        }
    }

    public void CannotPass(ConditionResult conditionResult)
    {
        //Add logic later...
        conditionResult.Result = false;
    }

    public void Wait(ActionResult actionResult)
    {
        actionResult.TickStatus = Task.Status.Success;
    }

    public void HasNoPath(ConditionResult conditionResult)
    {
        if(path == null || path.Count == 0)
        {
            conditionResult.Result = true;
        }
        else
        {
            conditionResult.Result = false;
        }
    }

    public void GetPath(ActionResult actionResult)
    {
        path = graph.PathFind(currentNode.Coordinates, goals[goalIndex].Coordinates);

        if(path == null || path.Count == 0)
        {
            Debug.LogWarning("Could not find a path!");
        }
        else
        {
            nextPathNodeIndex = path.Count - 1;
        }

        actionResult.TickStatus = Task.Status.Success;
    }

    public void NotReachedGoal(ConditionResult conditionResult)
    {
        if(currentNode == goals[goalIndex])
        {
            conditionResult.Result = false;
            path = null;
            goalIndex++;

            if (goalIndex >= goals.Length)
            {
                goalIndex = 0;
            }
            return;
        }

        conditionResult.Result = true;
    }

    public void MoveTowardsGoal(ActionResult actionResult)
    {
        if (nextNode == null)
        {
            nextNode = path[nextPathNodeIndex--];
            nextPosition = nextNode.GetPointOnNode(agent.radius);

            if (!agent.SetDestination(nextPosition))
            {
                Debug.LogError("Could not set position");
            }
        }

        actionResult.TickStatus = Task.Status.Success;
    }
}
