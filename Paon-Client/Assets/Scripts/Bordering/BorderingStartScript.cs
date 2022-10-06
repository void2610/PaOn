using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingStartScript : MonoBehaviour
    {
        public bool starting = false;

        private GameObject Camera;

        void Start()
        {
            Camera = GameObject.Find("Main Camera");
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                starting = true;
                Debug.Log("Bordering Started");
                Camera.GetComponent<PlayerCameraScript>().stat = 0;
            }
        }

        void OnTriggerExit(Collider other)
        {
            starting = false;
        }
    }
}
