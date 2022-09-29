using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using Paon.NNetwork.Shared.Hubs;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;

namespace Paon.NNetwork
{
    public class GamingHubClient : IGamingHubReceiver
    {
        public GameObject[] Dolls = new GameObject[8];
        // 部屋に参加しているユーザ全員の GameObject (アバター)を保持する
        Dictionary<string, GameObject>
            players = new Dictionary<string, GameObject>();

        // サーバ側の関数を実行するための変数
        IGamingHub client;

        // 指定したルームに入室するための関数
        // StreamingHubClient で使用する gRPC チャネル及び、参加したい部屋名、使用するユーザ名を引数に指定する
        public async Task<GameObject>
        ConnectAsync(Channel grpcChannel, string roomName, string playerName)
        {
            // サーバ側の関数を実行するための StreamingHubClient を生成する
            client =
                StreamingHubClient
                    .Connect<IGamingHub, IGamingHubReceiver>(grpcChannel, this);

            // JoinAsync 関数を実行して部屋に入室すると同時に、
            // 既に入室済みのユーザ全員の情報を配列で取得する
            var roomPlayers =
                await client
                    .JoinAsync(roomName,
                    playerName,
                    Vector3.zero,
                    Vector3.zero,
                    Vector3.zero,
                    Quaternion.identity);

            // 自ユーザ以外を OnJoin 関数に渡して、
            // this.players に部屋の他ユーザ全員の情報をセットする
            // 自ユーザの情報は await で JoinAsync を実行した段階で、
            // OnJoin がコールバックで呼ばれているためセット済みの状態となっている
            foreach (var player in roomPlayers)
            {
                if (player.Name != playerName)
                {
                    (this as IGamingHubReceiver).OnJoin(player);
                }
            }

            // 自ユーザの情報を返却する
            return players[playerName];
        }

        // 部屋から退出し、部屋の他ユーザ全員に退出したことをブロードキャスト送信する
        public Task LeaveAsync()
        {
            return client.LeaveAsync();
        }

        // 自ユーザの位置(Vector3) と回転(Quaternion) を更新すると同時に
        // 部屋の他ユーザ全員にブロードキャスト送信する
        public Task
        MoveAsync(
            Vector3 _body,
            Vector3 _right,
            Vector3 _left,
            Quaternion rotation
        )
        {
            return client.MoveAsync(_body, _right, _left, rotation);
        }

        // StreamingHubClient の解放処理
        // gRPC のチャネルを破棄する前に実行する必要がある
        public Task DisposeAsync()
        {
            return client.DisposeAsync();
        }

        // 部屋に新しいユーザが入室したときに呼び出される関数
        // または ConnectAsync 関数を実行したときに呼び出される関数
        void IGamingHubReceiver.OnJoin(Player player)
        {
            // ユーザの GameObject (アバター)を Player 情報を元に生成して
            // this.players に player.Name をキーにして保持する
            // 部屋に入室しているユーザの数だけワールド上にキューブを出現する

            Dolls[player.ID % 8] = GameClient.MakeDolls(player);
            Dolls[player.ID % 8].name = player.Name;
            Dolls[player.ID % 8].transform.SetPositionAndRotation(player.BodyPosition, player.Rotation);
            Dolls[player.ID % 8].transform.SetPositionAndRotation(player.BodyPosition, player.Rotation);
            Dolls[player.ID % 8].transform.SetPositionAndRotation(player.BodyPosition, player.Rotation);
            players[player.Name] = Dolls[player.ID % 8];
        }

        // 他ユーザが部屋から退出した際に呼び出される関数
        void IGamingHubReceiver.OnLeave(Player player)
        {
            // this.players に保持していた GameObject (アバター)を破棄する
            // ワールド上から該当する GameObject (アバター)のキューブが消滅する
            if (players.TryGetValue(player.Name, out var doll))
            {
                GameObject.Destroy (doll);
            }
        }

        // 部屋の中でいずれかのユーザが動いたときに呼び出される関数
        void IGamingHubReceiver.OnMove(Player player)
        {
            // 引数の player の Name を元に this.players 内から GameObject を取得する
            // ワールド上の該当する GameObject (アバター)の位置(Vector3)と回転(Quaternion) の値を最新のものに更新する
            if (players.TryGetValue(player.Name, out var doll))
            {
                doll
                    .transform
                    .SetPositionAndRotation(player.BodyPosition,
                    player.Rotation);
            }
        }
    }
}
