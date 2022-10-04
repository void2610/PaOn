using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingWaitManager : MonoBehaviour
    {
        public int NowPeople = 0;

        public int MaxPeople = 3;

        public GameObject[] WaitAreas = new GameObject[3];

        void Start()
        {
            WaitAreas[0] = GameObject.Find("WaitArea1");
            WaitAreas[1] = GameObject.Find("WaitArea2");
            WaitAreas[2] = GameObject.Find("WaitArea3");
        }

        void Update()
        {
            if (NowPeople < MaxPeople)
            {
                //人数に空きがあって、待機エリアに人がいる場合、プレイ中にしてテレポートさせる
                if (
                    WaitAreas[0].GetComponent<WaitAreaScript>().ReadyPlayer !=
                    null
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
                }
            }

            //次の待機エリアにテレポートさせる
            //なんか違う気がする
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
