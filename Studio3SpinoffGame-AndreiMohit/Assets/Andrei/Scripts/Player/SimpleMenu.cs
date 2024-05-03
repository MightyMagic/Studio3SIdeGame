using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadFromStart()
    {
        SceneManager.LoadScene(1);
    }

    public void StartLevelOne()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadFlashback()
    {
        SceneManager.LoadScene(3);
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("SimpleMainMenu");
    }

    public void LoadByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
