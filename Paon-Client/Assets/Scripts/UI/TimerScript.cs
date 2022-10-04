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

        private GamingHubClient client = new GamingHubClient();

        public bool counting = false;

        public bool display = false;

        private float time = 0.0f;

        private float cooldown = Mathf.Infinity;

        ///<summary>
        ///タイマーをスタートするメソッド
        ///</summary>
        /// <returns>void</returns>
        public void CountStart()
        {
            time = 0.0f;
            counting = true;
        }

        ///<summary>
        ///タイマーをストップするメソッド
        ///</summary>
        /// <returns>void</returns>
        public void CountStop()
        {
            counting = false;
        }

        void Start()
        {
            GoalText = GameObject.Find("GoalText");
        }

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
                }
                client
                    .TimeAsync(PlayerPrefs.GetString("Name", "NULLTYAN"), time);
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
