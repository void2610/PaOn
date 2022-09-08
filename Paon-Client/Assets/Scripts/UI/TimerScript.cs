using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NUI
{
    public class TimerScript : MonoBehaviour
    {
        float time = 0.0f;

        public bool counting = false;

        void CountStart()
        {
            time = 0.0f;
            counting = true;
        }

        void CountStop()
        {
            counting = false;
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (counting)
            {
                time += Time.deltaTime;
            }
            if (time > 0)
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
