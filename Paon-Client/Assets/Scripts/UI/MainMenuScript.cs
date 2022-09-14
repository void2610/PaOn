using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Paon.NUI
{
    public class MainMenuScript : MonoBehaviour
    {
        int status = 0; //0:main menu, 1:stage menu, 2:server menu, 3:option menu

        GameObject S;

        GameObject Option;

        GameObject Quit;

        GameObject SizenAsobi;

        GameObject Bordering;

        GameObject Back;

        GameObject Option2;

        GameObject Server1;

        GameObject Server2;

        GameObject Name;

        GameObject NamePlate;

        GameObject Player;

        InputField Input;

        public void ClickStartButton()
        {
            status = 1;
        }

        public void ClickOptionButton()
        {
            status = 3;
        }

        public void ClickQuitButton()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        public void ClickBackButton()
        {
            if (status == 3)
                status = 0;
            else
                status -= 1;
        }

        public void ClickGameModeButton()
        {
            status = 2;
        }

        public void ClickServerButton()
        {
            SceneManager.LoadScene("Main");
        }

        public void ClickBrightnessButton()
        {
            RenderSettings.ambientIntensity = 0.5f;
            DynamicGI.UpdateEnvironment();
            Debug.Log("aaa");
        }

        public void ApplyName()
        {
            PlayerPrefs.SetString("Name", Input.text);
            PlayerPrefs.Save();
        }

        void Start()
        {
            S = GameObject.Find("Start");
            Option = GameObject.Find("Option");
            Quit = GameObject.Find("Quit");
            SizenAsobi = GameObject.Find("SizenAsobi");
            Bordering = GameObject.Find("Bordering");
            Back = GameObject.Find("Back");
            Name = GameObject.Find("Name");
            NamePlate = GameObject.Find("NamePlate");
            Input = Name.GetComponent<InputField>();
            Input.text = PlayerPrefs.GetString("Name", "なまえをにゅうりょく");
            Option2 = GameObject.Find("Option2");
            Server1 = GameObject.Find("Server1");
            Server2 = GameObject.Find("Server2");
            Player = GameObject.Find("PlayerBody");
        }

        void Update()
        {
            if (status == 0)
            {
                S.SetActive(true);
                Option.SetActive(true);
                Quit.SetActive(true);

                SizenAsobi.SetActive(false);
                Bordering.SetActive(false);

                Name.SetActive(false);
                NamePlate.SetActive(false);
                Player.SetActive(false);
                Option2.SetActive(false);

                Server1.SetActive(false);
                Server2.SetActive(false);

                Back.SetActive(false);
            }
            else if (status == 1)
            {
                S.SetActive(false);
                Option.SetActive(false);
                Quit.SetActive(false);

                SizenAsobi.SetActive(true);
                Bordering.SetActive(true);

                Name.SetActive(false);
                NamePlate.SetActive(false);
                Player.SetActive(false);
                Option2.SetActive(false);

                Server1.SetActive(false);
                Server2.SetActive(false);

                Back.SetActive(true);
            }
            else if (status == 2)
            {
                S.SetActive(false);
                Option.SetActive(false);
                Quit.SetActive(false);

                SizenAsobi.SetActive(false);
                Bordering.SetActive(false);

                Name.SetActive(false);
                NamePlate.SetActive(false);
                Player.SetActive(false);
                Option2.SetActive(false);

                Server1.SetActive(true);
                Server2.SetActive(true);

                Back.SetActive(true);
            }
            else if (status == 3)
            {
                S.SetActive(false);
                Option.SetActive(false);
                Quit.SetActive(false);

                SizenAsobi.SetActive(false);
                Bordering.SetActive(false);

                Name.SetActive(true);
                NamePlate.SetActive(true);
                NamePlate.GetComponent<Text>().text =
                    PlayerPrefs.GetString("Name", "なまえをにゅうりょく");
                Player.SetActive(true);
                Option2.SetActive(true);

                Server1.SetActive(false);
                Server2.SetActive(false);

                Back.SetActive(true);
            }
        }
    }
}
