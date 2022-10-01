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
        int status = 0; //0:main menu, 1:stage menu, 3:option menu 4:toukei,5:nature play server menu,6: bordering server menu

        GameObject S;

        GameObject Option;

        GameObject Quit;

        GameObject SizenAsobi;

        GameObject Bordering;

        GameObject Back;

        GameObject Color;

        GameObject NServer1;

        GameObject NServer2;

        GameObject BServer1;

        GameObject BServer2;

        GameObject Name;

        GameObject NamePlate;

        GameObject Toukei;

        GameObject Player;

        GameObject Statistics;

        GameObject Reset;

        InputField Input;

        GameObject Volume;

        [SerializeField]
        Material Skin;

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
            if (status == 3 || status == 4 || status == 5 || status == 6)
                status = 0;
            else
                status -= 1;
        }

        public void ClickNaturePlayButton()
        {
            status = 5;
        }

        public void ClickBorderingButton()
        {
            status = 6;
        }

        public void ClickToukeiButton()
        {
            status = 4;
        }

        public void ApplyName()
        {
            PlayerPrefs.SetString("Name", Input.text);
            PlayerPrefs.Save();
        }

        public void ChangeColor()
        {
            float red = Random.value;
            float green = Random.value;
            float blue = Random.value;
            Skin.color = new Color(red, green, blue, 1.0f);
            PlayerPrefs.SetFloat("Color_Red", red);
            PlayerPrefs.SetFloat("Color_Green", green);
            PlayerPrefs.SetFloat("Color_Blue", blue);
        }

        public void ResetStatistics()
        {
            PlayerPrefs.SetInt("GiveItem", 0);
            PlayerPrefs.SetInt("GiveTurn", 0);
        }

        public void ValueChange()
        {
            PlayerPrefs.SetFloat("Volume", Volume.GetComponent<Slider>().value);
            AudioListener.volume = Volume.GetComponent<Slider>().value;
        }

        public void JoinNature1()
        {
            PlayerPrefs.SetString("Room", "Nature1");
            SceneManager.LoadScene("Main");
        }

        public void JoinNature2()
        {
            PlayerPrefs.SetString("Room", "Nature2");
            SceneManager.LoadScene("Main");
        }

        public void JoinBordering1()
        {
            PlayerPrefs.SetString("Room", "Bordering1");
            SceneManager.LoadScene("BorderingScene");
        }

        public void JoinBordering2()
        {
            PlayerPrefs.SetString("Room", "Bordering2");
            SceneManager.LoadScene("BorderingScene");
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
            Color = GameObject.Find("Color");
            NServer1 = GameObject.Find("NServer1");
            NServer2 = GameObject.Find("NServer2");
            BServer1 = GameObject.Find("BServer1");
            BServer2 = GameObject.Find("BServer2");
            Player = GameObject.Find("PlayerBody");
            Toukei = GameObject.Find("Toukei");
            Statistics = GameObject.Find("Statistics");
            Reset = GameObject.Find("Reset");
            Volume = GameObject.Find("Volume");
            Volume.GetComponent<Slider>().value =
                PlayerPrefs.GetFloat("Volume", 0.5f);
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
                Color.SetActive(false);
                Volume.SetActive(false);

                NServer1.SetActive(false);
                NServer2.SetActive(false);
                BServer1.SetActive(false);
                BServer2.SetActive(false);

                Toukei.SetActive(true);
                Statistics.SetActive(false);
                Reset.SetActive(false);

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
                Color.SetActive(false);
                Volume.SetActive(false);

                NServer1.SetActive(false);
                NServer2.SetActive(false);
                BServer1.SetActive(false);
                BServer2.SetActive(false);

                Toukei.SetActive(false);
                Statistics.SetActive(false);
                Reset.SetActive(false);

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
                Color.SetActive(true);
                Volume.SetActive(true);

                NServer1.SetActive(false);
                NServer2.SetActive(false);
                BServer1.SetActive(false);
                BServer2.SetActive(false);

                Toukei.SetActive(false);
                Statistics.SetActive(false);
                Reset.SetActive(false);

                Back.SetActive(true);
            }
            else if (status == 4)
            {
                S.SetActive(false);
                Option.SetActive(false);
                Quit.SetActive(false);

                SizenAsobi.SetActive(false);
                Bordering.SetActive(false);

                Name.SetActive(false);
                NamePlate.SetActive(false);
                Player.SetActive(false);
                Color.SetActive(false);
                Volume.SetActive(false);

                NServer1.SetActive(false);
                NServer2.SetActive(false);
                BServer1.SetActive(false);
                BServer2.SetActive(false);

                Toukei.SetActive(false);
                Statistics.SetActive(true);
                Reset.SetActive(true);

                Back.SetActive(true);
                string a =
                    "他の子どもに道具を譲った回数: " +
                    PlayerPrefs.GetInt("GiveItem", 0) +
                    "回";
                string b =
                    "他の子どもに順番を譲った回数: " +
                    PlayerPrefs.GetInt("GiveTurn", 0) +
                    "回";
                Statistics.GetComponent<Text>().text = a + "\n" + b + "\n";
            }
            else if (status == 5)
            {
                S.SetActive(false);
                Option.SetActive(false);
                Quit.SetActive(false);

                SizenAsobi.SetActive(false);
                Bordering.SetActive(false);

                Name.SetActive(false);
                NamePlate.SetActive(false);
                Player.SetActive(false);
                Color.SetActive(false);
                Volume.SetActive(false);

                NServer1.SetActive(true);
                NServer2.SetActive(true);
                BServer1.SetActive(false);
                BServer2.SetActive(false);

                Toukei.SetActive(false);
                Statistics.SetActive(false);
                Reset.SetActive(false);

                Back.SetActive(true);
            }
            else if (status == 6)
            {
                S.SetActive(false);
                Option.SetActive(false);
                Quit.SetActive(false);

                SizenAsobi.SetActive(false);
                Bordering.SetActive(false);

                Name.SetActive(false);
                NamePlate.SetActive(false);
                Player.SetActive(false);
                Color.SetActive(false);
                Volume.SetActive(false);

                NServer1.SetActive(false);
                NServer2.SetActive(false);
                BServer1.SetActive(true);
                BServer2.SetActive(true);

                Toukei.SetActive(false);
                Statistics.SetActive(false);
                Reset.SetActive(false);

                Back.SetActive(true);
            }
        }
    }
}
