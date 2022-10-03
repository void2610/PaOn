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

        private float sTime = -10.0f;

        void Start()
        {
            BorderingManager = GameObject.Find("BorderingManager");
            Player = GameObject.Find("PlayerBody");
        }

        // Update is called once per frame
        void Update()
        {
            if (!tmp && goaling)
            {
                sTime = Time.time;
                if (this.GetComponent<AudioSource>())
                {
                    this.GetComponent<AudioSource>().PlayOneShot(SE);
                }
            }
            if (Time.time - sTime <= 1.5f)
            {
                if (Time.time - sTime == 1.5f)
                {
                    sTime = -10.0f;
                }
                else
                {
                    Player.transform.Translate(0, 0.1f, 0.1f);
                }
            }
            tmp = goaling;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                BorderingManager
                    .GetComponent<BorderingTimerScript>()
                    .Timer
                    .CountStop();
                goaling = true;
            }
            else
            {
                goaling = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            goaling = false;
        }
    }
}
