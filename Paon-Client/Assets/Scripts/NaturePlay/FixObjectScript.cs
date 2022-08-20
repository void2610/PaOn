using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NaturePlay
{
    public class FixObjectScript : MonoBehaviour
    {
        public CanvasData canvasData;

        public bool saving = false;

        void Start()
        {
        }

        void Update()
        {
        }

        //接触したオブジェクトが引数otherとして渡される
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HoldableTag"))
            {
                other.gameObject.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeAll;
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                other.gameObject.transform.parent =
                    GameObject.Find("FixedObjects").transform;
                Debug.Log("fixed");
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (saving)
                if (
                    other.CompareTag("HoldableTag") ||
                    other.CompareTag("LineObjectTag")
                )
                {
                    canvasData.AddObject(other.gameObject);
                }
            //全部セーブし終わったらsavingをfalseにする
        }
    }
}
