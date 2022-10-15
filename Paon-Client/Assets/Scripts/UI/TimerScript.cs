using System.Collections;
using System.Collections.Generic;
using Paon.NNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NUI
{
    public class TimerScript : MonoBehaviour
    {
        private GameObject GoalText;

        private GameObject client;

        public float time = 0.0f;

        public bool counting = false;

        public bool display = false;

        private float cooldown = Mathf.Infinity;

        public void CountStart()
        {
            counting = true;
        }

        public void CountStop()
        {
            counting = false;
        }

        public void CountReset()
        {
            time = 0.0f;
        }

        void Start()
        {
            GoalText = GameObject.Find("GoalText");
            client = GameObject.Find("GamingHubClient");
        }

        // Update is called once per frame
        void Update()
        {
            if (counting)
            {
                time += Time.deltaTime;
                cooldown = Time.time;
            }
            if (Time.time - cooldown < 30.0f && Time.time - cooldown >= 0.0f)
            {
                display = true;
            }
            else
            {
                display = false;
            }

            if (display)
            {
                this.gameObject.GetComponent<Text>().text = time.ToString("F2");
                if (GoalText != null && !counting)
                {
                    GoalText.GetComponent<Text>().text = "ゴール！";

                    client
                        .GetComponent<GamingHubClient>()
                        .TimeAsync(PlayerPrefs.GetString("Name", "NULLTYAN"),
                        time);
                }
            }
            else
            {
                this.gameObject.GetComponent<Text>().text = "";
                if (GoalText != null)
                {
                    GoalText.GetComponent<Text>().text = "";
                }
            }
        }
    }
}
