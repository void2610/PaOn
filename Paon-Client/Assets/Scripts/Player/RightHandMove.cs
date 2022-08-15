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
    public class RightHandMove : MonoBehaviour
    {
        GameObject hand;

        GameObject handProvider;

        RightHandInputProvider inputProvider;

        GameObject player;

        public bool canMove = true;

        Vector2 coords;

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

            hand = this.gameObject;
            inputProvider =
                GameObject
                    .Find("RightHandInputProvider")
                    .GetComponent<RightHandInputProvider>();
            var id = Random.Range(0, 10000);
            _hub = new GamingHubClient();
            await _hub.ConnectAsync(_channel, "Room", $"Player-{id}");
        }

        async void Update()
        {
            coords = inputProvider.GetPosition();
            if (canMove)
            {
                hand.transform.localPosition =
                    new Vector3((coords.x - 480) * -0.01f,
                        (coords.y - 300) * -0.01f,
                        hand.transform.localPosition.z);
                if (inputProvider.GetInput() == "up")
                {
                    hand.transform.Translate(Vector3.up * 0.01f);
                    await _hub.MoveAsync(player.transform.position, new Vector3());
                }
                else if (inputProvider.GetInput() == "down")
                {
                    hand.transform.Translate(Vector3.down * 0.01f);
                    await _hub.MoveAsync(player.transform.position, new Vector3());
                }
                else if (inputProvider.GetInput() == "left")
                {
                    hand.transform.Translate(Vector3.left * 0.01f);
                    await _hub.MoveAsync(player.transform.position, new Vector3());
                }
                else if (inputProvider.GetInput() == "right")
                {
                    hand.transform.Translate(Vector3.right * 0.01f);
                    await _hub.MoveAsync(player.transform.position, new Vector3());
                }
            }
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
