using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] int swarmIndex;
    [SerializeField] float noClumpingRadius;
    [SerializeField] float localAreaRadius;
    [SerializeField] float speed;
    [SerializeField] float steeringSpeed;

    [SerializeField] float separationFraction;
    [SerializeField] float alignmentFraction;
    [SerializeField] float cohesionFraction;

    [Header("Sterring vectors")]
    private Vector3 steering;
    private Vector3 separationDirection;
    private Vector3 alignmentDirection;
    private Vector3 cohesionDirection;
    private Vector3 targetPoint;


    float distance;
    float leaderAngle;
    BoidController leaderBoid;
    float angle;

    public void SimulateMovement(List<BoidController> other, float time, Transform targetObject)
    {

        steering = Vector3.zero;
        separationDirection = Vector3.zero;
        alignmentDirection = Vector3.zero;
        cohesionDirection = Vector3.zero;
        targetPoint = Vector3.zero;

        leaderAngle = 0f;
        leaderBoid = null;
        angle = 0f;

        foreach (BoidController boid in other)
        {
            
            //skip self
            if (boid == this)
                continue;

            distance = Vector3.Distance(boid.transform.position, this.transform.position);

            //identify close local neighbours
            if (distance < noClumpingRadius)
            {
                separationDirection += boid.transform.position - transform.position;
            }

            //identify local area neighbours
            if (distance < localAreaRadius)
            {
                alignmentDirection += boid.transform.forward;

                cohesionDirection += boid.transform.position - transform.position;

                angle = Vector3.Angle(boid.transform.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = boid;
                    leaderAngle = angle;
                }
            }
        }

        //flip and normalize
        separationDirection = -separationDirection.normalized;

        // cohesion relative to itself
        cohesionDirection -= transform.position;

        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized * 0.5f;

        steering += separationDirection.normalized * separationFraction;
        steering += alignmentDirection.normalized * alignmentFraction;
        steering += cohesionDirection.normalized * cohesionFraction;

        targetPoint = targetObject.position;
        steering += (targetPoint - transform.position).normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 0.3f * localAreaRadius, LayerMask.GetMask("Obstacle")) && Vector3.Distance(targetObject.transform.position, transform.position) < 10f)
        {
            steering = -(hitInfo.point - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * 3f);

            print("about to hit");
        }
        else
        {
            //apply steering
            if (steering != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * time);
        }
        
        //move 
        transform.position += transform.forward * time * speed;
    }
}
