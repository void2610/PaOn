using System;
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

		private MoveInputProvider mp;

		private Vector3 coords;

		private Vector2 delta;

		public bool canMove = true;

		public bool isCalib = false;

		private bool isDebugEnabled = false;
		private DebugManager debugger;

		private bool crouch = false;

		async void Start()
		{
			hand = this.gameObject;
			inputProvider = GameObject.Find("LeftHandInputProvider").GetComponent<LeftHandInputProvider>();
			mp = GameObject.Find("MoveInputProvider").GetComponent<MoveInputProvider>();
			player = this.gameObject;
			debugger = GameObject.Find("DebugManager").GetComponent<DebugManager>();
		}

		async void FixedUpdate()
		{
			isDebugEnabled = debugger.isDebugEnabled;
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

				if (!isDebugEnabled)
				{
					if (isCalib)
					{
						hand.transform.localPosition = new Vector3(coords.x / 70 - 2, -coords.y / 70 + 1, hand.transform.localPosition.z);
					}
					else if (crouch)
					{
						hand.transform.localPosition = new Vector3(-coords.x / 40 + 4, -coords.y / 30 + 3, hand.transform.localPosition.z);
						hand.transform.eulerAngles = new Vector3(-25, hand.transform.eulerAngles.y, hand.transform.eulerAngles.z);
					}
					else
					{
						hand.transform.localPosition = new Vector3(-coords.x / 40 + 4, -coords.y / 30 + 2.5f, hand.transform.localPosition.z);
						hand.transform.eulerAngles = new Vector3(0, hand.transform.eulerAngles.y, hand.transform.eulerAngles.z);
					}
				}
			}
		}
	}
}
