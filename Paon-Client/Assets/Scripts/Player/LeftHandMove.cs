using System.Collections;
using System.Collections.Generic;
using Grpc.Core;
using MagicOnion.Client;
using Paon.NNetwork;
using Paon.NInput;
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

        private GamingHubClient _hub;

        private Channel _channel;

        private IMyFirstService _service;

        // void Awake()
        // {
        //     _channel =
        //         new Channel("106.165.109.38", 5032, ChannelCredentials.Insecure);
        //     _service = MagicOnionClient.Create<IMyFirstService>(_channel);
        // }
        async void Start()
        {
            //var x = Random.Range(0, 1000);
            //var y = Random.Range(0, 1000);
            //var result = await _service.SumAsync(x, y);
            //Debug.Log($"Result: {result}");
            hand = this.gameObject;
            inputProvider =
                GameObject
                    .Find("LeftHandInputProvider")
                    .GetComponent<LeftHandInputProvider>();
            player = this.gameObject;
            //var id = Random.Range(0, 10000);
            //_hub = new GamingHubClient();
            //await _hub.ConnectAsync(_channel, "Room", $"Player-{id}");
        }

        async void LateUpdate()
        {
            coords = inputProvider.GetPosition();
            delta = inputProvider.GetDelta();
            if (canMove)
            {
                // hand.transform.localPosition =
                //     new Vector3((coords.x - 520) * -0.01f,
                //         (coords.y - 300) * -0.01f,
                //         hand.transform.localPosition.z);
                if (inputProvider.GetInput() == "up")
                {
                    hand.transform.Translate(Vector3.left * 0.01f);
                    //await _hub
                    //    .MoveAsync(player.transform.position, new Vector3());
                }
                else if (inputProvider.GetInput() == "down")
                {
                    hand.transform.Translate(Vector3.right * 0.01f);
                    //await _hub
                    //    .MoveAsync(player.transform.position, new Vector3());
                }
                else if (inputProvider.GetInput() == "left")
                {
                    hand.transform.Translate(Vector3.up * 0.01f);
                    //await _hub
                    //    .MoveAsync(player.transform.position, new Vector3());
                }
                else if (inputProvider.GetInput() == "right")
                {
                    hand.transform.Translate(Vector3.down * 0.01f);
                    //await _hub
                    //   .MoveAsync(player.transform.position, new Vector3());
                }

                hand.transform.localPosition += new Vector3(-delta.x / 100, -delta.y / 100, 0f);
            }
        }

        // async void OnDestroy()
        // {
        //     if (_hub != null)
        //     {
        //         await _hub.DisposeAsync();
        //     }
        //     if (_channel != null)
        //     {
        //         await _channel.ShutdownAsync();
        //     }
        // }
    }
}
