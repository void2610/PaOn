using System.Threading.Tasks;
using MagicOnion;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;

namespace Paon.NNetwork.Shared.Hubs
{
    public interface IGamingHubReceiver
    {
        // return type shuold be `void` or `Task`, parameters are free.
        void OnJoin(Player player);
        void OnLeave(Player player);
        void OnMove(Player player);
    }

    // Client -> Server definition
    // implements `IStreamingHub<TSelf, TReceiver>`  and share this type between server and client.
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        // return type shuold be `Task` or `Task<T>`, parameters are free.
        Task<Player[]> JoinAsync(string roomName, string userName, Vector3 position, Vector3 rotation);
        Task LeaveAsync();
        Task MoveAsync(Vector3 position, Vector3 rotation);
    }
}