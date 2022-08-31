using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using Paon.NNetwork.Shared.Hubs;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;

namespace Paon
{
    public class GamingHubClient : IGamingHubReceiver
    {
        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();
        IGamingHub _client;

        public async Task<GameObject> ConnectAsync(Channel grpcChannel, string roomName, string playerName)
        {
            _client = StreamingHubClient.Connect<IGamingHub, IGamingHubReceiver>(grpcChannel, this);

            var roomPlayers = await _client.JoinAsync(roomName, playerName, Vector3.zero, Vector3.zero);
            foreach (var player in roomPlayers)
            {
                (this as IGamingHubReceiver).OnJoin(player);
            }

            return _players[playerName];
        }

        // methods send to server.

        public Task LeaveAsync()
        {
            return _client.LeaveAsync();
        }

        public Task MoveAsync(Vector3 position, Vector3 rotation)
        {
            return _client.MoveAsync(position, rotation);
        }

        // dispose client-connection before channel.ShutDownAsync is important!
        public Task DisposeAsync()
        {
            return _client.DisposeAsync();
        }

        // You can watch connection state, use this for retry etc.
        public Task WaitForDisconnect()
        {
            return _client.WaitForDisconnect();
        }

        // Receivers of message from server.

        void IGamingHubReceiver.OnJoin(Player player)
        {
            Debug.Log("Join Player:" + player.Name);

            //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.name = player.Name;
            //cube.transform.SetPositionAndRotation(player.Position,Quaternion.Euler(player.Rotation.x,player.Rotation.y,player.Rotation.z));
            //_players[player.Name] = cube;
        }

        void IGamingHubReceiver.OnLeave(Player player)
        {
            Debug.Log("Leave Player:" + player.Name);

            if (_players.TryGetValue(player.Name, out var cube))
            {
                GameObject.Destroy(cube);
                _players.Remove(player.Name);
            }
        }

        void IGamingHubReceiver.OnMove(Player player)
        {
            Debug.Log("Move Player:" + player.Name);

            //if (_players.TryGetValue(player.Name, out var cube))
            //{
            //    cube.transform.SetPositionAndRotation(player.Position, Quaternion.Euler(player.Rotation.x, player.Rotation.y, player.Rotation.z));
            //}
        }
    }
}