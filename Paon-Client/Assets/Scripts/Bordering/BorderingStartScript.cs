using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingStartScript : MonoBehaviour
    {
        public bool starting = false;

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
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
