using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Node : MonoBehaviour, IEquatable<Node>
{
    public Vec2Int Coordinates { get; private set; }

    private readonly List<Node> adjacentNodes = new();

    private readonly List<ConditionResult> walkableConditions = new();

    public int G { get; set; } = 0;

    public int H { get; set; } = 0;

    public int F { get { return G + H; } }

    public Node Previous { get; set; }

    [SerializeField]
    private Node[] nodesToMakeAdjacent;

    [SerializeField]
    private float widthX = 1;

    [SerializeField]
    private float widthZ = 1;

    public float Width { get => widthX; }

    [SerializeField]
    private bool isBlocked = false;

    public bool IsBlocked { get => isBlocked; }

    [SerializeField]
    private LayerMask groundMask;

    public Node()
    {
        Coordinates = new Vec2Int(0, 0);
    }

    public Node(int x, int y)
    {
        Coordinates = new Vec2Int(x, y);
    }

    void Awake()
    {
        AddAdjacentNodes();
    }

    public void SetCoordinates(Vec2Int coordinates)
    {
        Coordinates = coordinates;
    }

    public void MakeAdjacentTo(Node other)
    {
        adjacentNodes.Add(other);

        ConditionResult conditionResult = new()
        {
            Result = true
        };
        walkableConditions.Add(conditionResult);
    }

    public void RemoveAdjacencyTo(Node other)
    {
        adjacentNodes.Remove(other);
    }

    public bool IsAdjacent(Node other)
    {
        return adjacentNodes.Contains(other);
    }

    public List<Node> GetAdjacent()
    {
        return adjacentNodes;
    }

    public List<ConditionResult> GetWalkableConditions()
    {
        return walkableConditions;
    }

    private void AddAdjacentNodes()
    {
        for(int i = 0; i < nodesToMakeAdjacent.Length; i++)
        {
            if (nodesToMakeAdjacent[i] != null)
            {
                MakeAdjacentTo(nodesToMakeAdjacent[i]);
            }
        }
    }

    public Vector3 GetPointOnNode(float radius)
    {
        float x = UnityEngine.Random.Range(-widthX + radius, widthX - radius);
        float z = UnityEngine.Random.Range(-widthZ + radius, widthZ - radius);

        Vector3 offset = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * new Vector3(x, 0, z);

        Vector3 point = transform.position + offset;

        return point;
    }

    public static bool operator ==(Node first, Node second)
    {
        if(first is null)
        {
            if(second is null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if(second is null)
        {
            return false;
        }

        return first.Coordinates.Equals(second.Coordinates);
    }

    public static bool operator !=(Node first, Node second)
    {
        if (first is null)
        {
            if (second is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        if (second is null)
        {
            return true;
        }

        return !first.Coordinates.Equals(second.Coordinates);
    }

    public bool Equals(Node other)
    {
        return this == other;
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is Node node && Equals(node);
    }

    public override int GetHashCode()
    {
        return Coordinates.GetHashCode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 widthVector =  Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * new Vector3(widthX, widthX, widthZ);

        Gizmos.DrawWireCube(transform.position, widthVector);
    }
}

