using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NaturePlay
{
    public class FixObjectScript : MonoBehaviour
    {
        [SerializeField]
        public CanvasData canvasData = new CanvasData();

        public bool saving = false;

        void Start()
        {
        }

        void Update()
        {
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("HoldableTag"))
            {
                other.gameObject.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeAll;
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }

            if (saving)
            {
                if (other.gameObject.name != "PlayerBody")
                {
                    if (
                        other.CompareTag("HoldableTag") ||
                        other.CompareTag("LineObjectTag")
                    )
                    {
                        Debug.Log("try to save " + other.gameObject.name);
                        canvasData
                            .AddObject(other.gameObject,
                            new Vector3(this.gameObject.transform.position.x -
                                other.gameObject.transform.position.x,
                                this.gameObject.transform.position.y -
                                other.gameObject.transform.position.y,
                                this.gameObject.transform.position.z -
                                other.gameObject.transform.position.z));
                        GameObject.Destroy(other.gameObject);
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;

                        string name =
                            dt.Year.ToString() +
                            dt.Month.ToString() +
                            dt.Day.ToString() +
                            dt.Hour.ToString() +
                            dt.Minute.ToString() +
                            dt.Second.ToString();
                        canvasData.Save("Canvas_" + name);

                        //canvasData = new CanvasData();
                        saving = false;
                        Debug.Log("save successfull");
                    }
                }
                else
                {
                    Debug.Log("cant save Player");
                    saving = false;
                }
            }
        }
    }
}
