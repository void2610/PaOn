using System.Collections;
using System.Collections.Generic;
using Paon.NNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NBordering
{
    public class BorderingGoalScript : MonoBehaviour
    {
        public AudioClip SE;

        public bool goaling = false;

        private GameObject GoalPosition;

        private GameObject GoalText;

        private GameObject BorderingManager;

        private GameObject Player;

        private GameObject client;

        private bool tmp = false;

        private float goalTime = -30.0f;

        private float goalCooldown = 30.0f;

        void Start()
        {
            BorderingManager = GameObject.Find("BorderingManager");
            Player = GameObject.Find("PlayerBody");
            GoalPosition = GameObject.Find("GoalAnchor");
            GoalText = GameObject.Find("GoalText");
            client = GameObject.Find("GamingHubClient");
        }

        void Update()
        {
            Debug.Log("Goaled : " + goalTime);
            if (!tmp && goaling && (Time.time - goalTime > goalCooldown))
            {
                goalTime = Time.time;
                if (this.GetComponent<AudioSource>())
                {
                    this.GetComponent<AudioSource>().PlayOneShot(SE);
                }
                client
                    .GetComponent<GamingHubClient>()
                    .TimeAsync(PlayerPrefs.GetString("Name", "NULLTYAN"),
                    goalTime);
                this
                    .BorderingManager
                    .GetComponent<BorderingTimerScript>()
                    .Timer
                    .CountEnd();
                Player.transform.position = GoalPosition.transform.position;
                Player.GetComponent<Rigidbody>().useGravity = true;
            }
            if (Time.time - goalTime < 5)
            {
                GoalText.GetComponent<Text>().text = "ゴール！！";
            }
            else
            {
                GoalText.GetComponent<Text>().text = "";
            }
            tmp = goaling;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                goaling = true;
            }
            else
            {
                goaling = false;
            }
        }

        void OnTriggerExit(Collider other)
        {
            goaling = false;
        }
    }
}
