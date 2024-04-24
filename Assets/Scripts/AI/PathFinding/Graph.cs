using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private readonly AStar aStar = new();

    [SerializeField]
    private NodeColumn[] nodeColumns;

    // Start is called before the first frame update
    void Start()
    {
        InitializeNodes();
    }

    // Update is called once per frame
    void Update()
    {
        
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

                    //Debug.Log("Set coordinates of: " + x + ", " + y);
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
