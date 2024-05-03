using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementExecution : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] Rigidbody rb;

    [SerializeField] GameObject player;

    public float RbMagnitude;

    public List<Vector3> pointsToVisit = new List<Vector3>();

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void MoveAlongPath()
    {
        if (pointsToVisit.Count > 0)
        {
            MoveTowardsNextGoal(pointsToVisit[0]);
            RotateTowardsTarget();

            RbMagnitude = rb.velocity.magnitude;

            Vector3 distanceToNextCheckpoint = pointsToVisit[0] - transform.position;
            distanceToNextCheckpoint = new Vector3(distanceToNextCheckpoint.x, 0f, distanceToNextCheckpoint.z);

            if ((distanceToNextCheckpoint.magnitude < 0.5f))
            {
                ClearLatestGoal();
            }
            
        }
        else 
        {
            rb.velocity = Vector3.zero;
            // no points to visit
        }
    }

    public void AddNewGoals(List<Vector3> goals)
    {
        for(int i = 0; i < goals.Count; i++)
        {
            AppendNewGoal(goals[i]);
        }
    }

    void AppendNewGoal(Vector3 goal)
    {
        pointsToVisit.Add(goal);
    }

    void ClearLatestGoal()
    {
        if(pointsToVisit.Count > 0)
            pointsToVisit.RemoveAt(0);
    }

    public void ClearAllPoints()
    {
        pointsToVisit.Clear();
    }

    void MoveTowardsNextGoal(Vector3 goal)
    {
        Vector3 direction = (goal - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector3(direction.x, 0f, direction.z);
    }

    public void RotateTowardsTarget()
    {
        if(pointsToVisit.Count > 0)
        {
            Vector3 goal = pointsToVisit[0];
            Vector3 targetDirection = goal - transform.position;
            targetDirection = new Vector3(targetDirection.x, 0f, targetDirection.z);

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }      
    }

    public void RotateTowardsPlayer()
    {
        Vector3 goal = player.transform.position;
        Vector3 targetDirection = goal - transform.position;
        targetDirection = new Vector3(targetDirection.x, 0f, targetDirection.z);

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    public void Stop()
    {
        pointsToVisit.Clear();
        rb.velocity = Vector3.zero;
    }
}
