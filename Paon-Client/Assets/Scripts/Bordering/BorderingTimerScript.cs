using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using Paon.NUI;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingTimerScript : MonoBehaviour
    {
        public TimerScript Timer;

        GameObject Player;

        public void StartTimer()
        {
            if (Player.GetComponent<PlayerMove>()._Player.playingBordering)
            {
                if (!Timer.counting)
                {
                    Timer.CountStart();
                }
            }
        }

        void Start()
        {
            Timer = GameObject.Find("Timer").GetComponent<TimerScript>();
            Player = GameObject.Find("PlayerBody");
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
