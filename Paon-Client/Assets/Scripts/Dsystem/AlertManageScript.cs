using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dsystem
{
    public class AlertManageScript : MonoBehaviour
    {
        private Image img;

        private Text nameText;
        private Text descriptionText;

        public bool isAlerted = false;

        public string nowName;
        public string nowDescription;

        private float sTime;
        private float showTime = 2.0f;

        private bool tmp = false;

        void Start()
        {
            nameText = GameObject.Find("AlertName").GetComponent<Text>();
            descriptionText = GameObject.Find("AlertDes").GetComponent<Text>();
            img = GameObject.Find("RedImage").GetComponent<Image>();
            img.color = Color.clear;
            nowName = "あぶないよ";
            nowDescription = "やめてね\nyametene";
        }

        // Update is called once per frame
        void Update()
        {
            if(!tmp && isAlerted){
                sTime = Time.time;
            }
            if(Time.time - sTime > showTime){
                isAlerted = false;
            }
            if (isAlerted)
            {
                nameText.text = nowName;
                descriptionText.text = nowDescription;
                this.img.color = new Color(0.5f, 0f, 0f, 0.4f);
            }
            else
            {
                nameText.text = "";
                descriptionText.text = "";
                this.img.color = new Color(0, 0, 0, 0);
            }

            tmp = isAlerted;
        }
    }
}
