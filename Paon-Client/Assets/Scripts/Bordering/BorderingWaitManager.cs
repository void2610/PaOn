using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingWaitManager : MonoBehaviour
    {
        bool Flags;

        public int NowPeople = 0;

        public int MaxPeople = 2;

        public GameObject[] WaitAreas = new GameObject[3];

        private GameObject client;

        public void FlagCheck(bool Flag)
        {
            Flags = Flag;
        }

        void Start()
        {
            WaitAreas[0] = GameObject.Find("WaitArea1");
            WaitAreas[1] = GameObject.Find("WaitArea2");
            WaitAreas[2] = GameObject.Find("WaitArea3");
            client = GameObject.Find("Border");
        }

        void Update()
        {
            if (NowPeople < MaxPeople)
            {
                //人数に空きがあって、待機エリアに人がいる場合、プレイ中にしてテレポートさせる
                if (
                    WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer !=
                    null && Flags == true
                )
                {
                    WaitAreas[0]
                        .GetComponent<WaitAreaScript>()
                        .ReadyPlayer
                        .GetComponent<PlayerMove>()
                        ._Player
                        .playingBordering = true;

                    WaitAreas[0]
                        .GetComponent<WaitAreaScript>()
                        .TeleportPlayer();
                    NowPeople++;

                    client.GetComponent<Border>().StartBorder();
                }
            }

            //次の待機エリアにテレポートさせる
            if (
                WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer ==
                null &&
                WaitAreas[1].GetComponent<WaitAreaScript>().ReadyPlayer != null
            )
            {
                WaitAreas[1].GetComponent<WaitAreaScript>().TeleportPlayer();
            }
            if (
                WaitAreas[1].GetComponent<WaitAreaScript>().ReadyPlayer ==
                null &&
                WaitAreas[2].GetComponent<WaitAreaScript>().ReadyPlayer != null
            )
            {
                WaitAreas[2].GetComponent<WaitAreaScript>().TeleportPlayer();
            }
        }
    }
}
