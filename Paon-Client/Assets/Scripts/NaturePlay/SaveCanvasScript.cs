using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NaturePlay
{
    public class SaveCanvasScript : MonoBehaviour
    {
        bool startSave = false;

        GameObject Trigger;

        float startTime;

        void Start()
        {
            Trigger = GameObject.Find("CanvasTrigger");
        }

        // Update is called once per frame
        void Update()
        {
            if (startSave)
            {
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("HandTag"))
            {
                if (startTime == -1)
                    startTime = Time.time;
                else
                {
                    if (Time.time - startTime > 3.0f)
                    {
                        Trigger.GetComponent<FixObjectScript>().saving = true;
                    }
                }
            }
        }
    }
}
