using System.Threading.Tasks;
using MagicOnion;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;

namespace Paon.NNetwork.Shared.Hubs
{
    /// <summary>
    /// クライアント -> サーバ
    /// </summary>
    public interface IGamingHubReceiver
    {
        void OnJoin(Player player);
        void OnLeave(Player player);
        void OnMove(Player player);
    }

    // クライアントがサーバ側で gRPC 実行可能な関数を定義する
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<Player[]> JoinAsync(string roomName, string userName, Vector3 _body, Vector3 _right, Vector3 _left, Quaternion rotation);
        Task LeaveAsync();
        Task MoveAsync(Vector3 _body, Vector3 _right, Vector3 _left, Quaternion rotation);
    }
}