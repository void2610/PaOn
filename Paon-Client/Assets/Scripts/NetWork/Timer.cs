﻿using System.Collections;
using System.Collections.Generic;
using Paon.NNetwork.Shared.MessagePackObjects;
using Grpc.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NNetwork
{
    public class Timer : MonoBehaviour
    {
        private GamingHubClient client = new GamingHubClient();

        private GameObject GoalText;

        //private int Count = 0;

        private float time;

        public bool counting = false;

        public bool display = false;

        private float cooldown = Mathf.Infinity;

        public void CountStart()
        {
            time = 0.0f;
            counting = true;
        }

        public void CountStop()
        {
            counting = false;
        }

        void Start()
        {
            GoalText = GameObject.Find("GoalText");
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
