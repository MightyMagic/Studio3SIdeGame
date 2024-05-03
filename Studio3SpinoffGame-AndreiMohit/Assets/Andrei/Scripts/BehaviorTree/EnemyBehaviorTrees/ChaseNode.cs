using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChaseNode : LeafNode
{
    [SerializeField] string goName;
    [SerializeField] float attackDistance;

    GameObject go;
    HeapPathfinding pathfinding;
    MovementExecution movement;

    GameObject player;
    float distanceToPlayer;
    protected override void OnStart()
    {
        Debug.Log("Started guarding");
        go = GameObject.Find(goName);
        pathfinding = go.GetComponent<HeapPathfinding>();
        movement = go.GetComponent<MovementExecution>();

        player = GameObject.Find("Player");
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;

        if (player == null || go == null)
        {
            Debug.LogError("Missing gameobjects");
        }
    }

    protected override void OnStop()
    {
        movement.Stop();
        pathfinding.grid.path.Clear();

    }

    protected override State OnUpdate()
    {
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;
        if (distanceToPlayer > attackDistance & distanceToPlayer < 30f)
        {
            if (movement.pointsToVisit.Count == 0)
            {
                pathfinding.FindPath(go.transform.position, player.transform.position);
                movement.AddNewGoals(pathfinding.grid.PathToVectors());
            }

            movement.MoveAlongPath();
            movement.RotateTowardsTarget();

            return State.Running;
        }
        else if(distanceToPlayer < attackDistance)
        {
            Debug.Log("Ready to attack!");
            return State.Success;
        }
        else { return State.Running; }
        
    }
}
