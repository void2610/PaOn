using Grpc.Core;
using MagicOnion.Client;
using Paon.NNetWork.Shared.Services;
using UnityEngine;
using Paon.NNetwork.Shared.MessagePackObjects;


namespace Paon.NNetwork
{
    public class MyFirstController : MonoBehaviour
    {
        private Channel _channel;
        private IMyFirstService _service;
        private float _moveTimer;
        private float _leaveTimer;
        private GamingHubClient _hub;
        private Player Me = new Player("test01");
        private Player[] PlayerArray = new Player[8];
        float x = 0;
        float y = 2;
        string key = "none";


        void Awake()
        {
            _channel = new Channel("106.165,109.38", 5032, ChannelCredentials.Insecure);
            _service = MagicOnionClient.Create<IMyFirstService>(_channel);
        }

        async void Start()
        {
            var x = Random.Range(0, 1000);
            var y = Random.Range(0, 1000);
            var result = await _service.SumAsync(x, y);
            Debug.Log($"Result: {result}");

            //Me.ID = await _service.GetMyid(Me);
            //Debug.Log($"Result: MyID is {Me.ID}");

            //Me.Position = new Vector3(10, 10, 10);
            //int aaa;
            //aaa = await _service.GiveMyPosition(Me);
            //if (aaa == 0)
            //{
            //    Debug.Log($"Result: {Me.Position.x}");
            //}

            //PlayerArray[1] = await _service.GetOthersPosition(Me.ID);
            //Debug.Log($"Result: {PlayerArray[1].ID}");

            var id = Random.Range(0, 10000);
            _hub = new GamingHubClient();
            await _hub.ConnectAsync(_channel, "Room", $"Player-{id}");
        }

        async void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                x -= 0.1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                x += 0.1f;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                y += 0.1f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                y -= 0.1f;
            }

            if (_hub == null)
            {
                return;
            }
            _moveTimer += Time.deltaTime;
            
            if (_moveTimer > 0.1f)
            {
                _moveTimer = 0f;
                await _hub.MoveAsync(new Vector3(x, y, 3), new Vector3());
            }
            if (_leaveTimer > 30f)
            {
                await _hub.LeaveAsync();
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
