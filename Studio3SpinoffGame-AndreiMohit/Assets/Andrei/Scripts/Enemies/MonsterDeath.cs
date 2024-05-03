using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeath : MonoBehaviour
{
    [SerializeField] int lives;
    public int hitCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            hitCount++;
            if(hitCount == lives)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            hitCount++;
            if (hitCount == lives)
                Destroy(gameObject);
        }
    }
}
