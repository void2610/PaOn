using System.Collections;
using System.Collections.Generic;
using Paon.NUI;
using UnityEngine;

namespace Paon.NBordering
{
    public class BorderingTimerScript : MonoBehaviour
    {
        TimerScript Timer;

        void Start()
        {
            Timer = GameObject.Find("Timer").GetComponent<TimerScript>();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
