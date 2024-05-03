using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PatrolNode : LeafNode
{
    [SerializeField] string goName;
    [SerializeField] string patrolObjectName;
    [SerializeField] float chaseDistance;
    [SerializeField] float alertDistance;

    GameObject patrolObject;
    public List<Transform> targets;

    GameObject go;
    HeapPathfinding pathfinding;
    MovementExecution movement;

    int targetIndex;

    GameObject player;
    float distanceToPlayer;
    protected override void OnStart()
    {
        Debug.Log("Started guarding");
        go = GameObject.Find(goName);
        pathfinding = go.GetComponent<HeapPathfinding>();
        movement = go.GetComponent<MovementExecution>();

        if (movement == null)
        {
            Debug.Log("No movement script attached!");
            movement = go.GetComponent<MovementExecution>();
        }

        player = GameObject.Find("Player");
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;

        patrolObject = GameObject.Find(patrolObjectName);
        foreach (Transform t in patrolObject.transform)
        {
            targets.Add(t);
        }

        targetIndex = Random.Range(0, targets.Count);

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
        if (distanceToPlayer > chaseDistance & distanceToPlayer < alertDistance)
        {
            if (movement.pointsToVisit.Count == 0)
            {
                targetIndex = (targetIndex + 1) % targets.Count;
                pathfinding.FindPath(go.transform.position, targets[targetIndex].position);
                movement.AddNewGoals(pathfinding.grid.PathToVectors());
            }

            movement.MoveAlongPath();
            movement.RotateTowardsTarget();

            return State.Running;
        }
        else if (distanceToPlayer < chaseDistance)
        {
            Debug.Log("Ready to attack!");
            return State.Success;
        }
        else { return State.Running; }

    }
}
