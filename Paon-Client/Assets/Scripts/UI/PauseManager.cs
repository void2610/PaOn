using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private GameObject PauseMenu;

    private bool pause = false;

    public void returnButton()
    {
        PauseMenu.SetActive(false);
        pause = false;
    }

    public void quitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        PauseMenu = GameObject.Find("PauseMenu");
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pause == false)
        {
            PauseMenu.SetActive(true);
            pause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause == true)
        {
            PauseMenu.SetActive(false);
            pause = false;
        }
    }
}
