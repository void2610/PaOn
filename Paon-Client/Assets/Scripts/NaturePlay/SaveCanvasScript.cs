using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class SaveCanvasScript : MonoBehaviour
    {
        bool startSave = false;

        public int cooldown = 5;

        public GameObject cameraPrefab;

        GameObject Trigger;

        float startTime = 0.0f;

        float endTime = 0.0f;

        void Start()
        {
            Trigger = GameObject.Find("CanvasTrigger");
            //Canvasが複数あってもいいように将来的にインスペクタ上で設定できるようにする
        }

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
                        Instantiate(cameraPrefab,
                        new Vector3(Trigger.transform.position.x,
                            Trigger.transform.position.y,
                            Trigger.transform.position.z),
                        new Quaternion(Trigger.transform.rotation.x,
                            Trigger.transform.rotation.y + 180,
                            Trigger.transform.rotation.z,
                            Trigger.transform.rotation.w));

                        //Trigger.GetComponent<FixObjectScript>().saving = true;
                        endTime = Time.time;
                    }
                }
            }
        }
    }
}
