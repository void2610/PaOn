using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NaturePlay
{
    public class SaveCanvasScript : MonoBehaviour
    {
        bool startSave = false;

        public int cooldown = 5;

        GameObject Trigger;

        float startTime = 0.0f;

        float endTime = 0.0f;

        void Start()
        {
            Trigger = GameObject.Find("CanvasTrigger");
            //Canvasが複数あってもいいように将来的にインスペクタ上で設定できるようにする
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("HandTag"))
            {
                if (startTime == -1)
                    startTime = Time.time;
                else
                {
                    if (
                        Time.time - startTime > 3.0f &&
                        Time.time - endTime > cooldown
                    )
                    {
                        Debug.Log("SaveStart");
                        Trigger.GetComponent<FixObjectScript>().saving = true;
                        endTime = Time.time;
                    }
                }
            }
        }
    }
}
