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
	public class RightHandMove : MonoBehaviour
	{
		private GameObject hand;

		private GameObject handProvider;

		private GameObject player;

		private RightHandInputProvider inputProvider;

		private MoveInputProvider mp;

		private Vector3 coords;

		private Vector2 delta;

		public bool canMove = true;

		private bool crouch = false;

		public bool isCalib = false;

		async void Start()
		{
			hand = this.gameObject;
			inputProvider = GameObject.Find("RightHandInputProvider").GetComponent<RightHandInputProvider>();
			mp = GameObject.Find("MoveInputProvider").GetComponent<MoveInputProvider>();
		}

		async void FixedUpdate()
		{
			coords = inputProvider.GetPosition();
			delta = inputProvider.GetDelta();

			if (mp.crouch == 1) { crouch = true; }
			else
			if (mp.crouch == 0) { crouch = false; }

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
					hand.transform.localPosition = new Vector3(coords.x / 70 - 1, -coords.y / 70 + 1, hand.transform.localPosition.z);
				else if (crouch)
				{
					hand.transform.localPosition = new Vector3(-coords.x / 40 + 4, -coords.y / 30 + 3, hand.transform.localPosition.z);
					hand.transform.eulerAngles = new Vector3(hand.transform.eulerAngles.x, 25, hand.transform.eulerAngles.z);
				}
				else
					hand.transform.localPosition = new Vector3(-coords.x / 40 + 4, -coords.y / 30 + 3.5f, hand.transform.localPosition.z);
			}
		}
	}
}
