using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dsystem
{
    public class AlertScript : MonoBehaviour
    {
        private Image img;

        private Text text;

        public bool isAlerted = false;

        void Start()
        {
            text = GameObject.Find("AlertText").GetComponent<Text>();
            img = GameObject.Find("RedImage").GetComponent<Image>();
            img.color = Color.clear;
        }

        // Update is called once per frame
        void Update()
        {
            if (isAlerted)
            {
                text.text = "やめてね";
                this.img.color = new Color(0.5f, 0f, 0f, 0.4f);
            }
            else
            {
                text.text = "";
                this.img.color = new Color(0, 0, 0, 0);
            }
        }
    }
}
