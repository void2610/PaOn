using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
    public class CloseCommoroseScript : MonoBehaviour
    {
        SelectEmojiScript2 SES2;

        void Start()
        {
            SES2 =
                GameObject.Find("RightHand").GetComponent<SelectEmojiScript2>();
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.name == "LeftHand")
            {
                SES2.isSelecting = false;
            }
        }
    }
}
