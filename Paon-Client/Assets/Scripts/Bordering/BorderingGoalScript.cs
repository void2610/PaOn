using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingGoalScript : MonoBehaviour
    {
        public AudioClip SE;

        public bool goaling = false;

        private GameObject GoalPosition;

        private GameObject BorderingManager;

        private GameObject Player;

        private bool tmp = false;

        private float goalTime = -30.0f;

        private float transferTime = 1.5f;

        private float goalCooldown = 30.0f;

        void Start()
        {
            BorderingManager = GameObject.Find("BorderingManager");
            Player = GameObject.Find("PlayerBody");
            GoalPosition = GameObject.Find("GoalAnchor");
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
                this
                    .BorderingManager
                    .GetComponent<BorderingTimerScript>()
                    .Timer
                    .CountStop();
                Player.transform.position = GoalPosition.transform.position;
            }

            // if (Time.time - goalTime <= transferTime)
            // {
            //     if (Time.time - goalTime == transferTime)
            //     {
            //         goalTime = -10.0f;
            //     }
            //     else
            //     {
            //         Player.transform.Translate(0, 0.1f, 0.1f);
            //     }
            // }
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
