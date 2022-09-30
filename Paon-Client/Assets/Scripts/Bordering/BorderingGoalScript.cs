using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingGoalScript : MonoBehaviour
    {
        GameObject bm;

        GameObject Player;

        public bool goaling = false;

        bool tmp = false;

        float sTime = -10.0f;

        public AudioClip SE;

        void Start()
        {
            bm = GameObject.Find("BorderingManager");
            Player = GameObject.Find("PlayerBody");
        }

        // Update is called once per frame
        void Update()
        {
            if (!tmp && goaling)
            {
                sTime = Time.time;
                this.GetComponent<AudioSource>().PlayOneShot(SE);
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
                bm.GetComponent<BorderingTimerScript>().Timer.CountStop();
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
