using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingGoalScript : MonoBehaviour
    {
        public AudioClip SE;

        public bool goaling = false;

        private GameObject BorderingManager;

        private GameObject Player;

        private bool tmp = false;

        private float goalTime = -10.0f;

        private float transferTime = 1.5f;

        private float goalCooldown = 30.0f;

        void Start()
        {
            BorderingManager = GameObject.Find("BorderingManager");
            Player = GameObject.Find("PlayerBody");
        }

        void Update()
        {
            Debug.Log("Goaled : " + goalTime);
            if (!tmp && goaling)
            {
                goalTime = Time.time;
                if (this.GetComponent<AudioSource>())
                {
                    this.GetComponent<AudioSource>().PlayOneShot(SE);
                }
            }
            if (Time.time - goalTime <= transferTime)
            {
                if (Time.time - goalTime == transferTime)
                {
                    goalTime = -10.0f;
                }
                else
                {
                    Player.transform.Translate(0, 0.1f, 0.1f);
                }
            }
            if (Time.time - goalTime < goalCooldown)
            {
                goaling = false;
            }
            tmp = goaling;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                this
                    .BorderingManager
                    .GetComponent<BorderingTimerScript>()
                    .Timer
                    .CountStop();
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
