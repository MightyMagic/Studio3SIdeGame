using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : LeafNode
{
    [SerializeField] string goName;
    [SerializeField] float attackDistance;
    [SerializeField] float alertDistance;
    [SerializeField] float attackCoolDown;

    GameObject go;
    HeapPathfinding pathfinding;
    MovementExecution movement;
    Animator animator;

    float timer = 0f;
    bool alreadyAttacked = true;

    GameObject player;
    float distanceToPlayer;
    protected override void OnStart()
    {
        Debug.LogError("Started attacking!");
        go = GameObject.Find(goName);
        pathfinding = go.GetComponent<HeapPathfinding>();
        movement = go.GetComponent<MovementExecution>();
        animator = go.transform.GetChild(0).GetComponent<Animator>();
        if(animator == null)
            animator = go.GetComponent<Animator>();

        // Attack!
        if(animator == null )
        {
            Debug.LogError("No animator!");
        }
        animator.SetBool("Attack", true);
        //animator.SetBool("Attack", false);

        player = GameObject.Find("Player");
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;

        if (player == null || go == null)
        {
            Debug.LogError("Missing gameobjects");
        }
    }

    protected override void OnStop()
    {
        timer = 0f;
    }

    protected override State OnUpdate()
    {
        go = GameObject.Find(goName);
        player = GameObject.Find("Player");

        movement.RotateTowardsPlayer();
        distanceToPlayer = (go.transform.position - player.transform.position).magnitude;

        if (timer > attackCoolDown & distanceToPlayer < attackDistance)
        {
            timer = 0f;
            // Attack!
            //animator.SetBool("Attack", true);
            //animator.SetBool("Attack", false);

            return State.Running;
        }
        else if(timer <= attackDistance & distanceToPlayer < attackDistance)
        {
            timer += Time.deltaTime;
            return State.Running;
        }
        else
        {
            animator.SetBool("Attack", false);

            if(distanceToPlayer > (alertDistance * 1.1f))
            {
                return State.Success;
            }
            else
            {
                return State.Running;
            }

        }
    }
}
