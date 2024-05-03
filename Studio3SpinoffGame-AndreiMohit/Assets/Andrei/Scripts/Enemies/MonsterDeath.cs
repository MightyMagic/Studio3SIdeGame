using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeath : MonoBehaviour
{
    [SerializeField] int lives;
    public int hitCount = 0;

    

    void Start()
    {
        PlayerPrefs.SetInt("Kills", 0);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            hitCount++;
            if(hitCount == lives)
            {
                Destroy(gameObject);
                PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            hitCount++;
            if (hitCount == lives)
            {
                Destroy(gameObject);
                PlayerPrefs.SetInt("Kills", PlayerPrefs.GetInt("Kills") + 1);
            }
        }
    }
}
