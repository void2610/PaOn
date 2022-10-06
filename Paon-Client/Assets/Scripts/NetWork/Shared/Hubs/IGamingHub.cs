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
        void OnJoin(Player player, float Red, float Blue, float Green);
        void OnLeave(Player player);
        void OnMove(Player player);
        void OnItem(Item item, string PlayerName);
        void OnGoal(string name, float time);
        void ChengeFace(Player player, int FaceID);
        void ItemItem(Item item);
        void FiastPlayer();
    }

    // クライアントがサーバ側で gRPC 実行可能な関数を定義する
    public interface IGamingHub : IStreamingHub<IGamingHub, IGamingHubReceiver>
    {
        Task<Player[]> JoinAsync(string roomName, string userName, Vector3 _body, Vector3 _right, Vector3 _left, Quaternion rotation, float Red, float Blue, float Green);
        Task LeaveAsync();
        Task MoveAsync(Vector3 _body, Vector3 _right, Vector3 _left, Quaternion rotation);
        Task ItemAsync(string name, Vector3 position, Quaternion rotation, string PlayerName);
        Task FaceAsync(int FaceID);
        Task TimeAsync(string name, float time);
        Task ItemJoin(string name, Vector3 position, Quaternion rotation, string roomName);
    }
}