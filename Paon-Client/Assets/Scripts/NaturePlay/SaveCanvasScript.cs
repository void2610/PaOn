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

        public GameObject Canvas;

        float startTime = 0.0f;

        float endTime = 0.0f;

        void Start()
        {
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
                        Time.time - startTime > 0.6f &&
                        Time.time - endTime > cooldown
                    )
                    {
                        Debug.Log("Save");
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
