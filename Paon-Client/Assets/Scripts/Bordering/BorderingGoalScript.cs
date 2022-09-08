using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingGoalScript : MonoBehaviour
    {
        GameObject bm;

        void Start()
        {
            bm = GameObject.Find("BorderingManager");
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bm.GetComponent<BorderingTimerScript>().Timer.CountStop();
            }
        }
    }
}
