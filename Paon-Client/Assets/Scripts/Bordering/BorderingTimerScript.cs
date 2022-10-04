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

        private GameObject Player;

        ///<summary>
        ///ボルダリングのタイマーを開始するメソッド
        ///</summary>
        /// <returns>void</returns>
        public void StartTimer()
        {
            if (Player.GetComponent<PlayerMove>())
            {
                if (Player.GetComponent<PlayerMove>()._Player.playingBordering)
                {
                    if (!Timer.counting)
                    {
                        Timer.CountStart();
                    }
                }
            }
        }

        void Start()
        {
            if (GameObject.Find("Timer").GetComponent<TimerScript>())
            {
                Timer = GameObject.Find("Timer").GetComponent<TimerScript>();
            }
            Player = GameObject.Find("PlayerBody");
        }
    }
}
