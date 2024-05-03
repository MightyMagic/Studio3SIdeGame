using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BoidController boidPrefab;
    [SerializeField] int spawnBoids;
    [SerializeField] float initialSpread;
    [SerializeField] Transform targetObject;

    [Header("Internal")]
    private List<BoidController> _boids;

    private void Start()
    {
        _boids = new List<BoidController>();

        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(boidPrefab.gameObject, 0);
        }
    }

    private void Update()
    {
        foreach (BoidController boid in _boids)
        {
            boid.SimulateMovement(_boids, Time.deltaTime, targetObject);
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        var boidInstance = Instantiate(prefab, this.transform.position, Quaternion.identity);
        boidInstance.transform.position += new Vector3(Random.Range(-initialSpread, initialSpread), Random.Range(-initialSpread, initialSpread), Random.Range(-initialSpread, initialSpread));
        BoidController boidController = boidInstance.GetComponent<BoidController>();
        _boids.Add(boidController);
    }
}
