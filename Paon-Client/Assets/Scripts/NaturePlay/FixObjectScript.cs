using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class FixObjectScript : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "HoldableTag")
            {
                other.gameObject.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeAll;
                other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                other.gameObject.transform.eulerAngles =
                    new Vector3(90,
                        other.gameObject.transform.eulerAngles.y,
                        -90);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "HoldableTag")
            {
                other.gameObject.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.None;
                other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
