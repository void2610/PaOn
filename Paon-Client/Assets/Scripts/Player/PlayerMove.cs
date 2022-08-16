using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;
using Grpc.Core;
using MagicOnion.Client;
using Paon.NNetWork.Shared.Services;
using Paon;
using Paon.NNetwork.Shared.MessagePackObjects;

namespace Paon.NPlayer
{
    public class PlayerMove : MonoBehaviour
    {
        MoveInputProvider inputProvider;
        public GameObject player;
        public GameObject es;
        private GamingHubClient _hub;
        private Channel _channel;
        private IMyFirstService _service;

        void Awake()
        {
            _channel = new Channel("192.168.11.2", 5032, ChannelCredentials.Insecure);
            _service = MagicOnionClient.Create<IMyFirstService>(_channel);
        }

        async void Start()
        {
            var x = Random.Range(0, 1000);
            var y = Random.Range(0, 1000);
            var result = await _service.SumAsync(x, y);
            Debug.Log($"Result: {result}");

            inputProvider = es.GetComponent<MoveInputProvider>();
            player = this.gameObject;
            var id = Random.Range(0, 10000);
            _hub = new GamingHubClient();
            await _hub.ConnectAsync(_channel, "Room", $"Player-{id}");
        }

        async void Update()
        {
            if (inputProvider.GetInput() == "space")
            {
                player.GetComponent<Rigidbody>().AddForce(Vector3.up * 0.25f, ForceMode.Impulse);
            }
            else if (inputProvider.GetInput() == "up")
            {
                player.transform.Translate(Vector3.forward * 0.1f);
                await _hub.MoveAsync(player.transform.position, new Vector3());
            }
            else if (inputProvider.GetInput() == "down")
            {
                player.transform.Translate(Vector3.back * 0.03f);
                await _hub.MoveAsync(player.transform.position, new Vector3());
            }
            else if (inputProvider.GetInput() == "left")
            {
                player.transform.Rotate(0, -0.15f, 0);
                await _hub.MoveAsync(new Vector3(), player.transform.eulerAngles);
            }
            else if (inputProvider.GetInput() == "right")
            {
                player.transform.Rotate(0, 0.15f, 0);
                await _hub.MoveAsync(new Vector3(), player.transform.eulerAngles);
            }
            //Debug.Log(inputProvider.GetInput());
        }

        async void OnDestroy()
        {
            if (_hub != null)
            {
                await _hub.DisposeAsync();
            }
            if (_channel != null)
            {
                await _channel.ShutdownAsync();
            }
        }
    }
}
