using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu()]
public class GuardNode : LeafNode
{
    [SerializeField] string goName;
    [SerializeField] float alertDistance;

    GameObject go;
    HeapPathfinding pathfinding;

    GameObject player;
    float distanceToPlayer;
    protected override void OnStart()
    {
        Debug.Log("Started guarding");
        go = GameObject.Find(goName);
        pathfinding = go.GetComponent<HeapPathfinding>();

        player = GameObject.Find("Player");
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;

        if(player == null || go == null)
        {
            Debug.LogError("Missing gameobjects");
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;
        if (distanceToPlayer < alertDistance)
        {
            Debug.LogError("Player is near me!");
            return State.Success;
        }
        else
        {
            Debug.Log("keep guarding, distance is " + distanceToPlayer);
            return State.Running;
        }
    }
}
