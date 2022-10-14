using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class WaitAreaScript : MonoBehaviour
    {
        public GameObject ReadyPlayer = null;

        public GameObject NextPosition;

        ///<summary>
        ///次の待機エリアにプレイヤーを移動させるメソッド
        ///</summary>
        /// <returns>void</returns>
        public void TeleportPlayer()
        {
            if (ReadyPlayer != null)
            {
                ReadyPlayer.transform.position =
                    NextPosition.transform.position;
                ReadyPlayer = null;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
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

        void OnTriggerExit(Collider other)
        {
            ReadyPlayer = null;
        }
    }
}
