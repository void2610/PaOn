using System.Collections;
using System.Collections.Generic;
using Paon.NBordering;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Paon.NInput
{
    public class PauseManager : MonoBehaviour
    {
        private GameObject PauseMenu;

        private GameObject BM;

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
            BM = GameObject.Find("BorderingManager");
            PauseMenu = GameObject.Find("PauseMenu");
            PauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && pause == false)
            {
                BM.GetComponent<BorderingTimerScript>().Timer.CountStop();
                PauseMenu.SetActive(true);
                pause = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pause == true)
            {
                BM.GetComponent<BorderingTimerScript>().Timer.CountStart();
                PauseMenu.SetActive(false);
                pause = false;
            }
        }
    }
}
