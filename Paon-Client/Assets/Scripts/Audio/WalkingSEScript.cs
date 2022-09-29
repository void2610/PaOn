using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NAudio
{
    public class WalkingSEScript : MonoBehaviour
    {
        Vector3 latestPos;

        Vector3 speed;

        GameObject Player;

        void Start()
        {
            Player = GameObject.Find("PlayerBody");
        }

        void Update()
        {
            speed =
                ((Player.transform.position - latestPos) / Time.deltaTime)
                    .magnitude;
            Debug.Log (speed);
            if (v.magnitude > 0.1f)
            {
                this.GetComponent<AudioSource>().Play();
            }
            else
            {
                this.GetComponent<AudioSource>().Stop();
            }
            latestPos = Player.transform.position;
        }
    }
}
