using System;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private readonly List<Node> searchable = new();
    private readonly List<Node> searched = new();

    public AStar()
    {
        
    }

    /// <summary>
    /// Returns the shortest path to the final node. The final node is the first element of the list and the next node is the last.
    /// </summary>
    /// <param name="firstNode">The node to start searching from</param>
    /// <param name="finalNode">The node to find</param>
    /// <returns></returns>
    public List<Node> FindPath(Node firstNode, Node finalNode)
    {
        var path = new List<Node>();
        searchable.Clear();
        searched.Clear();

        searchable.Add(firstNode);

        while (searchable.Count > 0)
        {
            var currentNode = searchable[0];

            //Trace path AStar took:
            //Debug.Log("SNC: " + currentNode.Coordinates.X + ", " + currentNode.Coordinates.Y);

            //Finds any better node, where the best one is the one with the lowest F cost or H cost if F is equal.
            foreach (var node in searchable)
            {
                if (node.F < currentNode.F || (node.F == currentNode.F && node.H < currentNode.H))
                {
                    currentNode = node;
                }
            }

            searched.Add(currentNode);
            searchable.Remove(currentNode);

            //If the final node has been found, then you create the path to it.
            if (currentNode == finalNode)
            {
                Node currentPathNode = finalNode;

                while (currentPathNode != firstNode)
                {
                    path.Add(currentPathNode);
                    currentPathNode = currentPathNode.Previous;
                }

                break;
            }

            var adjacent = currentNode.GetAdjacent();

            //Go through each node adjacent to the current one, add it to the searchable list if it has not been searched
            //and update its G and H so they have the value they would have if they are on the shortest path.
            for (int i = 0; i < adjacent.Count; i++)
            {
                Node currentAdjacent = adjacent[i];

                if (!searched.Contains(currentAdjacent))
                {
                    bool isSearchable = searchable.Contains(currentAdjacent);

                    int costToAdjacent = currentNode.G + 1;

                    if (!isSearchable || costToAdjacent < currentAdjacent.G)
                    {
                        currentAdjacent.G = costToAdjacent;
                        currentAdjacent.Previous = currentNode;

                        if (!isSearchable)
                        {
                            currentAdjacent.H = Math.Abs(currentAdjacent.Coordinates.Y - finalNode.Coordinates.Y)
                                                 + Math.Abs(currentAdjacent.Coordinates.X - finalNode.Coordinates.X);
                            searchable.Add(currentAdjacent);
                        }
                    }
                }
            }
        }

        return path;
    }

}
