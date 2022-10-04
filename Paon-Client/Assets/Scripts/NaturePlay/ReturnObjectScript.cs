using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class ReturnObjectScript : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (
                collision.gameObject.tag == "Player" ||
                collision.gameObject.tag == "HoldableTag"
            )
            {
                collision.gameObject.transform.position =
                    new Vector3(collision.gameObject.transform.position.x,
                        4,
                        collision.gameObject.transform.position.z);
            }
        }
    }
}
