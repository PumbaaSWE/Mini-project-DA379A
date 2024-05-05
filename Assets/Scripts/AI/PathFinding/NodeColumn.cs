using System;
using UnityEngine;

[Serializable]
public class NodeColumn
{
    [SerializeField]
    private Node[] row;

    public Node[] Row { get => row; }

    public NodeColumn()
    {

    }

    public NodeColumn(Node[] row)
    {
        this.row = row;
    }
}
