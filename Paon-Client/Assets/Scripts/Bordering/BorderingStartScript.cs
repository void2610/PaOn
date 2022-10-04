using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingStartScript : MonoBehaviour
    {
        public bool starting = false;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                starting = true;
            }
            else
            {
                starting = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            starting = false;
        }
    }
}
