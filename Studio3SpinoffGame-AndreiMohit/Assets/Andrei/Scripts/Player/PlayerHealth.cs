using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] Slider healthSlider;

    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
       
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Kills") >= 2)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }
    }

    public void GotHit()
    {
        currentHealth--;
        healthSlider.value = currentHealth;
        if(currentHealth < 0 )
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(2);
        }
    }
}
