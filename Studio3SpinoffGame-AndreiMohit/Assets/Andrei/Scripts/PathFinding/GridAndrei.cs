using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAndrei : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    [SerializeField] bool showAllGizmos;

    NodeAndrei[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public List<NodeAndrei> path = new List<NodeAndrei>();

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);

        CreateGrid();

    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    public void CreateGrid()
    {
        
        grid = new NodeAndrei[gridSizeX, gridSizeY];

        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        
        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);
                grid[x,y] = new NodeAndrei(walkable, worldPoint, x, y);
            }
        }

        Debug.LogError("I am making the grid! " + " It is this big " + grid.Length);
    }

    public List<NodeAndrei> GetNeighbors(NodeAndrei node)
    {
        List<NodeAndrei> neighbors = new List<NodeAndrei>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    public NodeAndrei NodeFromWorldPoint(Vector3 worldPosition)
    {
        float fractionX = worldPosition.x / gridWorldSize.x + 0.5f;
        float fractionY = worldPosition.z / gridWorldSize.y + 0.5f;

        fractionX = Mathf.Clamp01(fractionX);
        fractionY = Mathf.Clamp01(fractionY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * fractionX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * fractionY);

        return grid[x,y];
    }

    public List<Vector3> PathToVectors()
    {
        List<Vector3> vectors = new List<Vector3>();
        if(path.Count > 0)
        {
            for(int i = 0; i < path.Count; i++)
            {
                vectors.Add(path[i].worldPosition);
            }

            return vectors;
        }
        else { return null; }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1f, gridWorldSize.y));

        if(!showAllGizmos)
        {
            if(path != null)
            {
                Gizmos.color = Color.black;
                foreach(NodeAndrei node in path)
                {
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeDiameter * 0.6f);
                }
            }

        }
        else
        {
            if (grid != null)
            {

                NodeAndrei playerNode = NodeFromWorldPoint(player.position);

                foreach (NodeAndrei node in grid)
                {
                    Gizmos.color = (node.walkable ? Color.white : Color.red);

                    if (playerNode == node)
                        Gizmos.color = Color.blue;

                    if (path != null)
                    {
                        if (path.Contains(node))
                        {
                            Gizmos.color = Color.black;
                        }
                    }

                    Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeDiameter * 0.6f);
                }
            }
        }  
    }


}
