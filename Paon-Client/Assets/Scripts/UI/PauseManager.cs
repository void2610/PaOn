using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool pause = false;

    GameObject PauseMenu;

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

    // Update is called once per frame
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
