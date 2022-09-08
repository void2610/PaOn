using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class WaitAreaScript : MonoBehaviour
    {
        public GameObject ReadyPlayer = null;

        public GameObject NextPosition;

        public void TeleportPlayer()
        {
            if (ReadyPlayer != null)
            {
                ReadyPlayer.transform.position =
                    NextPosition.transform.position;
                ReadyPlayer = null;
            }
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (ReadyPlayer == null)
                {
                    ReadyPlayer = other.gameObject;
                }
            }
            else
            {
                ReadyPlayer = null;
            }
        }
    }
}
