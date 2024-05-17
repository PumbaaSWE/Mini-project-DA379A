using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private readonly AStar aStar = new();

    [SerializeField]
    private NodeColumn[] nodeColumns;

    [SerializeField]
    private bool autoCreate = false;

    public int XLength { get; private set; }
    public int YLength { get; private set; }

    void Awake()
    {
        if(autoCreate)
        {
            AutoCreateGraph();
        }

        InitializeNodes();

        XLength = nodeColumns.Length;
        YLength = nodeColumns[0].Row.Length;
    }

    private void InitializeNodes()
    {
        for(int x = 0; x < nodeColumns.Length; x++)
        {
            for(int y = 0; y < nodeColumns[x].Row.Length; y++)
            {
                if (nodeColumns[x].Row[y] != null)
                {
                    nodeColumns[x].Row[y].SetCoordinates(new Vec2Int(x, y));
                }
            }
        }
    }

    private void AutoCreateGraph()
    {
        nodeColumns = new NodeColumn[transform.childCount];
        
        for(int i = 0; i < nodeColumns.Length; i++)
        {
            nodeColumns[i] = new(transform.GetChild(i).GetComponentsInChildren<Node>());
        }

        for (int x = 0; x < nodeColumns.Length; x++)
        {
            for (int y = 0; y < nodeColumns[x].Row.Length; y++)
            {
                if (!nodeColumns[x].Row[y].IsBlocked)
                {
                    if(x > 0 && !nodeColumns[x - 1].Row[y].IsBlocked)
                    {
                        nodeColumns[x].Row[y].MakeAdjacentTo(nodeColumns[x - 1].Row[y]);
                    }

                    if(x < nodeColumns.Length - 1 && !nodeColumns[x + 1].Row[y].IsBlocked)
                    {
                        nodeColumns[x].Row[y].MakeAdjacentTo(nodeColumns[x + 1].Row[y]);
                    }

                    if(y > 0 && !nodeColumns[x].Row[y - 1].IsBlocked)
                    {
                        nodeColumns[x].Row[y].MakeAdjacentTo(nodeColumns[x].Row[y - 1]);
                    }

                    if(y < nodeColumns[x].Row.Length - 1 && !nodeColumns[x].Row[y + 1].IsBlocked)
                    {
                        nodeColumns[x].Row[y].MakeAdjacentTo(nodeColumns[x].Row[y + 1]);
                    }

                    //Debug.Log(nodeColumns[x].Row[y].GetAdjacent().Count);
                }
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        return nodeColumns[x].Row[y];
    }

    /// <summary>
    /// Returns the path to the goal node. The goal node is the first element of the list.
    /// </summary>
    /// <param name="start">Start coordinates.</param>
    /// <param name="end">Goal coordinates</param>
    /// <returns></returns>
    public List<Node> PathFind(Vec2Int start, Vec2Int end)
    {
        if(nodeColumns[start.X].Row[start.Y] == null || nodeColumns[end.X].Row[end.Y] == null)
        {
            Debug.LogError("Tried to path find from a null node!");
        }

        return aStar.FindPath(nodeColumns[start.X].Row[start.Y], nodeColumns[end.X].Row[end.Y]);
    }
}
