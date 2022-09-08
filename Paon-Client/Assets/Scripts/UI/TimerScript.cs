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

        public bool display = false;

        float cooldown = Mathf.Infinity;

        public void CountStart()
        {
            time = 0.0f;
            counting = true;
        }

        public void CountStop()
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
                cooldown = Time.time;
            }
            if (Time.time - cooldown < 30.0f && Time.time - cooldown >= 0.0f)
            {
                display = true;
            }
            else
            {
                display = false;
            }

            if (display)
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
