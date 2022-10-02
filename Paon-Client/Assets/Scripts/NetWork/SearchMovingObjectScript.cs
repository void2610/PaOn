using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNetwork
{
    public class SearchMovingObjectScript : MonoBehaviour
    {
        GameObject[] Holdables;

        private GameClient client = new GameClient();

        void Start()
        {
            Holdables = GameObject.FindGameObjectsWithTag("HoldableTag");
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < Holdables.Length; i++)
            {
                if (
                    Holdables[i].GetComponent<Rigidbody>().velocity.magnitude >
                    0.05f
                )
                {
                    client.SendMovingObject(Holdables[i]);
                }
            }
        }
    }
}
