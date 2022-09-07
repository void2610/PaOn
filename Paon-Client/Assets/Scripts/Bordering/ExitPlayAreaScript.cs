using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NBordering
{
    public class ExitPlayAreaScript : MonoBehaviour
    {
        GameObject NextPosition;

        void Start()
        {
            NextPosition = GameObject.Find("SpawnPositionAnchor");
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.transform.position =
                    NextPosition.transform.position;
            }
        }
    }
}
