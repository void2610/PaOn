using System.Collections;
using System.Collections.Generic;
using Grpc.Core;
using MagicOnion.Client;
using Paon.NInput;
using Paon.NNetwork;
using Paon.NNetwork.Shared.MessagePackObjects;
using Paon.NNetWork.Shared.Services;
using UnityEngine;

namespace Paon.NPlayer
{
	public class LeftHandMove : MonoBehaviour
	{
		private GameObject hand;

		private GameObject handProvider;

		private GameObject player;

		private LeftHandInputProvider inputProvider;

		private Vector3 coords;

		private Vector2 delta;

		public bool canMove = true;

		public bool isCalib = false;

		async void Start()
		{
			hand = this.gameObject;
			inputProvider = GameObject.Find("LeftHandInputProvider").GetComponent<LeftHandInputProvider>();
			player = this.gameObject;
		}

		async void FixedUpdate()
		{
			coords = inputProvider.GetPosition();
			delta = inputProvider.GetDelta();
			if (canMove)
			{
				if (inputProvider.GetInput() == "up")
				{
					hand.transform.Translate(Vector3.left * 0.01f);
				}
				else if (inputProvider.GetInput() == "down")
				{
					hand.transform.Translate(Vector3.right * 0.01f);
				}
				else if (inputProvider.GetInput() == "left")
				{
					hand.transform.Translate(Vector3.up * 0.01f);
				}
				else if (inputProvider.GetInput() == "right")
				{
					hand.transform.Translate(Vector3.down * 0.01f);
				}

				if (isCalib)
				{
					hand.transform.localPosition = new Vector3(coords.x / 70 - 3, -coords.y / 70 + 1, hand.transform.localPosition.z);
				}
				else
					hand.transform.localPosition = new Vector3(-coords.x / 50 + 3, -coords.y / 70 + 1, hand.transform.localPosition.z);
			}
		}
	}
}
