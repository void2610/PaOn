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

        // Update is called once per frame
        void Update()
        {
            if (NowPeople < MaxPeople)
            {
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
            if (WaitAreas[1].GetComponent<WaitAreaScript>().ReadyPlayer != null)
            {
                WaitAreas[1].GetComponent<WaitAreaScript>().TeleportPlayer();
            }
            if (WaitAreas[2].GetComponent<WaitAreaScript>().ReadyPlayer != null)
            {
                WaitAreas[2].GetComponent<WaitAreaScript>().TeleportPlayer();
            }
        }
    }
}
