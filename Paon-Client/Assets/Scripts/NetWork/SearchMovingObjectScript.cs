using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNetwork
{
	public class SearchMovingObjectScript : MonoBehaviour
	{
		GameObject[] Holdables;

		private GameObject client;

		void Start()
		{
			Holdables = GameObject.FindGameObjectsWithTag("HoldableTag");
			client = GameObject.Find("GameClient");
		}

		// Update is called once per frame
		void Update()
		{
			for (int i = 0; i < Holdables.Length; i++)
			{
				if (
						Holdables[i].GetComponent<Rigidbody>().velocity.magnitude >
						0.25f
				)
				{
				client.GetComponent<GameClient>().SendMovingObject(Holdables[i]);
				}
			}
		}
	}
}
