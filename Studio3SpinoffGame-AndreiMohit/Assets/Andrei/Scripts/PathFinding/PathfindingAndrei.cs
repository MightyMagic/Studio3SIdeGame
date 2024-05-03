using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAndrei : MonoBehaviour
{
    GridAndrei grid;

    public Transform seeker, target;

    private void Awake()
    {
        
    }
    private void Start()
    {
        grid = GetComponent<GridAndrei>();
        //FindPath(seeker.position, target.position);
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Space))
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        NodeAndrei startNode = grid.NodeFromWorldPoint(startPos);
        NodeAndrei targetNode = grid.NodeFromWorldPoint(targetPos);

        List<NodeAndrei> openSet = new List<NodeAndrei>();
        HashSet<NodeAndrei> closedSet = new HashSet<NodeAndrei>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            NodeAndrei currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.LogError("Path is being retraced, the distance is " + grid.path.Count + " nodes!");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (NodeAndrei neigbor in grid.GetNeighbors(currentNode))
            {
                if (!neigbor.walkable || closedSet.Contains(neigbor))
                    continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neigbor);
                if(newMovementCostToNeighbor < currentNode.gCost || !openSet.Contains(neigbor))
                {
                    neigbor.gCost = newMovementCostToNeighbor;
                    neigbor.hCost = GetDistance(neigbor, targetNode);
                    neigbor.parent = currentNode;

                    if(!openSet.Contains(neigbor))
                        openSet.Add(neigbor);
                }

                
            }
        }

        Debug.LogError("Path is found, the distance is " + grid.path.Count + " nodes!");

    }

    void RetracePath(NodeAndrei startNode, NodeAndrei endNode)
    {
        List<NodeAndrei> path = new List<NodeAndrei>();
        NodeAndrei currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        grid.path = path;
    }

    int GetDistance(NodeAndrei nodeA, NodeAndrei nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);  

        return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}
