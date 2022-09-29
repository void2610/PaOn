using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NAudio
{
    public class WalkingSEScript : MonoBehaviour
    {
        Vector3 latestPos;

        float speed;

        GameObject Player;

        bool e = false;

        bool tmp = false;

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
            if (e && !tmp)
            {
                this.GetComponent<AudioSource>().Play();
            }
            if (!e && tmp)
            {
                this.GetComponent<AudioSource>().Stop();
            }
            latestPos = Player.transform.position;
            tmp = e;
        }
    }
}
