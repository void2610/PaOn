using System.Collections;
using System.Collections.Generic;
using Paon.NNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NUI
{
    public class TimerScript : MonoBehaviour
    {
        public float time = 0.0f;

        public bool counting = false;

        public void CountStart()
        {
            time = 0;
            counting = true;
        }

        public void CountReStart()
        {
            counting = true;
        }

        public void CountStop()
        {
            counting = false;
        }

        public void CountEnd()
        {
            counting = false;
        }

        public void CountReset()
        {
            time = 0.0f;
        }

        void Update()
        {
            if (counting)
            {
                time += Time.deltaTime;
            }
            if (time != 0)
            {
                this.gameObject.GetComponent<Text>().text = time.ToString("F2");
            }
            else
            {
                this.gameObject.GetComponent<Text>().text = "";
            }
        }
    }
}
