using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour, IThrowable
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

    private ConditionResult walkableCondition;

    [Range(0f, 100f)]
    [SerializeField]
    private float timeBetweenTicks;

    private float currentTimeUntilTick = 0.0f;

    [SerializeField]
    private Graph graph;

    private bool hasBeenStopped = false;

    private int groundLayer;

    [SerializeField]
    private string groundLayerName = "WhatIsGround";

    private int projectileLayer;

    [SerializeField]
    private string projectileLayerName = "Projectile";

    private int carLayer;

    [SerializeField]
    private string carLayerName = "Car";

    private Rigidbody rb;

    private bool isPickedUp = false;

    private bool knockedDown = false;

    private float knockedDownTimer = 0;

    [Range(0f, 10f)]
    [SerializeField]
    private float knockOutTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();

        currentNode = startNode;

        if(goals.Length < 2)
        {
            Debug.LogError("NPC has less than two goals!");
        }

        groundLayer = LayerMask.NameToLayer(groundLayerName);
        projectileLayer = LayerMask.NameToLayer(projectileLayerName);
        carLayer = LayerMask.NameToLayer(carLayerName);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(isPickedUp)
        {
            if (collision.gameObject.layer == groundLayer)
            {
                isPickedUp = false;
                ResetAgent();
            }
        } 
        
        if(collision.gameObject.layer == projectileLayer)
        {
            KnockDownAgent();
        }

        if(collision.gameObject.layer == carLayer)
        {
            KnockDownAgent();
        }
    }

    public void CannotContinue(ConditionResult conditionResult)
    {
        if (isPickedUp)
        {
            conditionResult.Result = true;
            return;
        }

        if (knockedDown)
        {
            conditionResult.Result = true;
            return;
        }

        if(hasBeenStopped && walkableCondition.Result == false)
        {
            conditionResult.Result = true;
            return;
        }

        conditionResult.Result = false;
    }

    public void Wait(ActionResult actionResult)
    {
        if (knockedDown)
        {
            knockedDownTimer -= Time.deltaTime;

            if(knockedDownTimer <= 0)
            {
                knockedDown = false;
                ResetAgent();
            }
        }

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
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        if (nextNode == null)
        {
            nextNode = path[nextPathNodeIndex--];
            nextPosition = nextNode.GetPointOnNode(agent.radius);

            int adjacencyIndex = currentNode.GetAdjacent().IndexOf(nextNode);
            walkableCondition = currentNode.GetWalkableConditions()[adjacencyIndex];

            SetDestination(nextPosition);
        }

        if(walkableCondition.Result == false && !hasBeenStopped)
        {
            SetDestination(currentNode.GetPointOnNode(agent.radius));
            hasBeenStopped = true;
        }
        else if (walkableCondition.Result == true && hasBeenStopped)
        {
            SetDestination(nextPosition);
            hasBeenStopped = false;
        }

        actionResult.TickStatus = Task.Status.Success;
    }

    private void SetDestination(Vector3 target)
    {
        if (!agent.SetDestination(target))
        {
            Debug.LogError("Could not set position");
        }
    }

    private void ResetAgent()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        agent.enabled = true;
        SetDestination(nextPosition);
    }

    private void KnockDownAgent()
    {
        agent.enabled = false;
        rb.useGravity = true;
        knockedDown = true;
        knockedDownTimer = knockOutTime;
    }

    public void PerformGrabLogic()
    {
        isPickedUp = true;
    }

    public void PerformThrowLogic()
    {
        agent.enabled = false;
        rb.useGravity = true;
    }
}
