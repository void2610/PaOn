using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NatuePlay
{
    public class FixObjectScript : MonoBehaviour
    {
        CanvasData canvasData;

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
    }
}
