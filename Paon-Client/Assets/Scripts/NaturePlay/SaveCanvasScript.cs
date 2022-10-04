using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NNaturePlay
{
    public class SaveCanvasScript : MonoBehaviour
    {
        public AudioClip SE;

        public GameObject cameraPrefab;

        public GameObject Canvas;

        public GameObject LogText;

        private GameObject[] Switches;

        public int cooldown = 6;

        private float startTime = 0.0f;

        private float endTime = -6.0f;

        private bool startSave = false;

        private bool log = false;

        public bool display = false;

        private bool canHide = true;

        void Start()
        {
            Switches = GameObject.FindGameObjectsWithTag("SwitchTag");
        }

        void Update()
        {
            canHide = true;
            for (int i = 0; i < Switches.Length; i++)
            {
                if (Switches[i] != this.gameObject)
                {
                    if (Switches[i].GetComponent<SaveCanvasScript>().display)
                    {
                        canHide = false;
                        break;
                    }
                }
            }

            if (canHide)
            {
                LogText.GetComponent<Text>().text = "";
            }
            else
            {
                LogText.GetComponent<Text>().text = "セーブしました！";
            }
            if (Time.time - endTime < cooldown)
            {
                display = true;
            }
            else
            {
                display = false;
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
