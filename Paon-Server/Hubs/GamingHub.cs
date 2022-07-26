using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using Paon.NNetwork.Shared.Hubs;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;

namespace Paon.NNetwork.Hubs
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        // this class is instantiated per connected so fields are cache area of connection.
        IGroup _room;
        Player _self;
        IInMemoryStorage<Player> _storage;

        public async Task<Player[]> JoinAsync(string roomName, string userName, Vector3 position, Vector3 rotation)
        {
            _self = new Player(userName);

            // Group can bundle many connections and it has inmemory-storage so add any type per group. 
            (_room, _storage) = await Group.AddAsync(roomName, _self);

            // Typed Server->Client broadcast.
            BroadcastExceptSelf(_room).OnJoin(_self);

            return _storage.AllValues.ToArray();
        }

        public async Task LeaveAsync()
        {
            Broadcast(_room).OnLeave(_self);
            await _room.RemoveAsync(Context);
        }

        public async Task MoveAsync(Vector3 position, Vector3 rotation)
        {
            _self.Position = position;
            _self.Rotation = rotation;
            Broadcast(_room).OnMove(_self);
        }

        // You can hook OnConnecting/OnDisconnected by override.
        protected override async ValueTask OnDisconnected()
        {
            // on disconnecting, if automatically removed this connection from group.
            await CompletedTask;
        }
    }
}