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
	public class PlayerMove : MonoBehaviour
	{
		MoveInputProvider inputProvider;

		public GameObject player;

		public GameObject es;

		public bool canMove = true;

		public Player _Player = new Player();

		public bool p;

		async void Start()
		{
			inputProvider = es.GetComponent<MoveInputProvider>();
			player = this.gameObject;
			_Player.name = PlayerPrefs.GetString("Name", "NoName");
		}

		async void Update()
		{
			p = _Player.playingBordering;
			if (canMove)
			{
				if (inputProvider.GetInput() == "space")
				{
					player
							.GetComponent<Rigidbody>()
							.AddForce(Vector3.up * 0.25f, ForceMode.Impulse);
				}
				else if (inputProvider.GetInput() == "up")
				{
					player.transform.Translate(Vector3.forward * 0.1f);
				}
				else if (inputProvider.GetInput() == "down")
				{
					player.transform.Translate(Vector3.back * 0.03f);
				}
				else if (inputProvider.GetInput() == "left")
				{
					player.transform.Rotate(0, -0.8f, 0);
				}
				else if (inputProvider.GetInput() == "right")
				{
					player.transform.Rotate(0, 0.8f, 0);
				}
			}
		}
	}
}
