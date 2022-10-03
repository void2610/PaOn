using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NAudio
{
    public class WalkingSEScript : MonoBehaviour
    {
        private GameObject Player;

        private Vector3 latestPos;

        private float speed;

        private bool e = false;

        private bool tmp = false;

        void Start()
        {
            Player = GameObject.Find("PlayerBody");
        }

        void Update()
        {
            speed =
                ((Player.transform.position - latestPos) / Time.deltaTime)
                    .magnitude;
            if (speed > 0.1f)
            {
                e = true;
            }
            else
            {
                e = false;
            }
            if (this.GetComponent<AudioSource>())
            {
                if (e && !tmp)
                {
                    this.GetComponent<AudioSource>().Play();
                }
                if (!e && tmp)
                {
                    this.GetComponent<AudioSource>().Stop();
                }
            }
            latestPos = Player.transform.position;
            tmp = e;
        }
    }
}
