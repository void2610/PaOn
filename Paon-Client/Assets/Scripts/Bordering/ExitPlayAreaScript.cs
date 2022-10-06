using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NBordering
{
    public class ExitPlayAreaScript : MonoBehaviour
    {
        private GameObject NextPosition;

        private GameObject BM;

        private GameObject client;

        void Start()
        {
            NextPosition = GameObject.Find("SpawnPositionAnchor");
            BM = GameObject.Find("BorderingManager");
            client = GameObject.Find("Border");
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other
                    .gameObject
                    .GetComponent<PlayerMove>()
                    ._Player
                    .playingBordering = false;
                other.gameObject.transform.position =
                    NextPosition.transform.position;
                client.GetComponent<Border>().OutBorder();
            }
        }
    }
}
