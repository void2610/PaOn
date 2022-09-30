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
		GameObject hand;

		GameObject handProvider;

		LeftHandInputProvider inputProvider;

		GameObject player;

		public bool canMove = true;

		Vector2 coords;
		Vector2 delta;

		async void Start()
		{
			hand = this.gameObject;
			inputProvider =
					GameObject
							.Find("LeftHandInputProvider")
							.GetComponent<LeftHandInputProvider>();
			player = this.gameObject;
		}

		async void FixedUpdate()
		{
			coords = inputProvider.GetPosition();
			delta = inputProvider.GetDelta();
			// Debug.Log(delta);
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
				hand.transform.localPosition = new Vector3(-coords.x / 100 + 1, -coords.y / 100 + 2, hand.transform.localPosition.z);
			}
		}
	}
}
