using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NNaturePlay
{
    public class SaveCanvasScript : MonoBehaviour
    {
        public AudioClip SE;

        bool startSave = false;

        public int cooldown = 5;

        public GameObject cameraPrefab;

        public GameObject Canvas;

        float startTime = 0.0f;

        float endTime = -6.0f;

        GameObject LogText;

        bool log = false;

        void Start()
        {
            LogText = GameObject.Find("LogText");
        }

        void Update()
        {
            //Debug.Log(Time.time - endTime);
            if (Time.time - endTime < cooldown)
            {
                LogText.GetComponent<Text>().text = "セーブしました！";
            }
            else
            {
                LogText.GetComponent<Text>().text = "";
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("HandTag"))
            {
                if (startTime == -1)
                {
                    startTime = Time.time;
                }
                else
                {
                    if (
                        Time.time - startTime > 0.6f &&
                        Time.time - endTime > cooldown
                    )
                    {
                        Debug.Log("Save");
                        GetComponent<AudioSource>().PlayOneShot(SE);
                        log = true;
                        Vector3 rot =
                            new Vector3(Canvas.transform.eulerAngles.x,
                                Canvas.transform.eulerAngles.y - 90,
                                Canvas.transform.eulerAngles.z);
                        Instantiate(cameraPrefab,
                        new Vector3(Canvas.transform.position.x +
                            (
                            1.3f *
                            Mathf
                                .Cos(Canvas.transform.eulerAngles.y *
                                Mathf.Deg2Rad)
                            ),
                            Canvas.transform.position.y,
                            Canvas.transform.position.z +
                            (
                            -1.3f *
                            Mathf
                                .Sin(Canvas.transform.eulerAngles.y *
                                Mathf.Deg2Rad)
                            )),
                        Quaternion.Euler(rot));
                        endTime = Time.time;
                    }
                }
            }
        }
    }
}
