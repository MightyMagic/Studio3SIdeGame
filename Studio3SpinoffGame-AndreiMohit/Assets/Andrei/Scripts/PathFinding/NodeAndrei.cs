using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeAndrei : IHeapItem<NodeAndrei>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    int heapIndex;

    public NodeAndrei parent;

    public NodeAndrei(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;

        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost {  get { return gCost + hCost; } }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(NodeAndrei nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
