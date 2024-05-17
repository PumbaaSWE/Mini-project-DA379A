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

    private bool isAfraid = false;

    private Vector3 escapeDirection;

    [SerializeField]
    private float fleeStrength = 10f;

    private float fleeTime = 0;

    [SerializeField]
    private float fleeingDuration = 2f;

    private PedestrianHandler handler = null;

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

        if(other.gameObject.layer == carLayer)
        {
            isAfraid = true;
            escapeDirection = (transform.position - other.transform.position).normalized;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == carLayer)
        {
            fleeTime = fleeingDuration;
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

    public void HasBeenScared(ConditionResult conditionResult)
    {
        conditionResult.Result = isAfraid;
    }

    public void Flee(ActionResult actionResult)
    {
        SetDestination(transform.position + escapeDirection * fleeStrength);

        if(fleeTime > 0)
        {
            fleeTime -= Time.deltaTime;

            if(fleeTime <= 0)
            {
                isAfraid = false;
                SetDestination(nextPosition);
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
            NavMesh.SamplePosition(nextPosition, out NavMeshHit hit, 1000, NavMesh.AllAreas);
            nextPosition = hit.position;

            int adjacencyIndex = currentNode.GetAdjacent().IndexOf(nextNode);
            walkableCondition = currentNode.GetWalkableConditions()[adjacencyIndex];

            SetDestination(nextPosition);
        }

        if(walkableCondition.Result == false && !hasBeenStopped)
        {
            Vector3 position = currentNode.GetPointOnNode(agent.radius);
            NavMesh.SamplePosition(position, out NavMeshHit hit, 1000, NavMesh.AllAreas);
            SetDestination(hit.position);
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
            Debug.LogError("Could not set position: " + target.x + "; " + target.y + "; " + target.z);
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

    public void PedestrianStart()
    {
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();

        groundLayer = LayerMask.NameToLayer(groundLayerName);
        projectileLayer = LayerMask.NameToLayer(projectileLayerName);
        carLayer = LayerMask.NameToLayer(carLayerName);
    }

    public void InitPedestrian(Node currentNode, Node[] goals, Graph graph, PedestrianHandler handler)
    {
        this.startNode = currentNode;
        this.currentNode = currentNode;
        this.goals = goals;
        this.graph = graph;
        this.handler = handler;

        Vector3 point = currentNode.GetPointOnNode(agent.radius);
        transform.position = new Vector3(point.x, 1, point.z);
    }
}
