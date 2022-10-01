using Grpc.Core;
using MagicOnion.Client;
using Paon.NNetWork.Shared.Services;
using UnityEngine;
using Paon.NNetwork.Shared.MessagePackObjects;
using System.Threading.Tasks;
using System;


namespace Paon.NNetwork
{
    public class GameClient : MonoBehaviour
    {
        [SerializeField]
        static GameObject Doll;
        GameObject body, right, left;
        // プレイヤーの Transform (今回はメインカメラの Transform を指定)
        [SerializeField]
        Transform m_PlayerTransform;

        // 部屋に参加するときに使用するユーザ名 (何でも設定可)
        [SerializeField]
        string m_UserName;

        // 参加したい部屋のルーム名
        // (StreamingHub クライアント同士で交流したい場合は、
        // 各クライアントで同一の名前を設定する必要がある)
        [SerializeField]
        string m_RoomName;

        // StreamingHub クライアントで使用する gRPC チャネルを生成
        private Channel channel = new Channel("192.168.10.7", 5032, ChannelCredentials.Insecure);

        // StreamingHub サーバと通信を行うためのクライアント生成
        private GamingHubClient client = new GamingHubClient();

        async Task Start()
        {
            m_UserName = PlayerPrefs.GetString("Name", "NULLTYAN");
            m_RoomName = PlayerPrefs.GetString("Room", "MAIGO");
            m_RoomName = "poipoi";
            Doll = (GameObject)Resources.Load("Doll");
            body = GameObject.Find("PlayerBody");
            right = GameObject.Find("RightHand");
            left = GameObject.Find("LeftHand");
            // ゲーム起動時に設定した部屋名のルームに設定したユーザ名で入室する。
            await this.client.ConnectAsync(this.channel, this.m_RoomName, this.m_UserName);
        }

        public void SendFaceID(int FaceID)
        {
            client.FaceAsync(FaceID);
        }

        public static GameObject MakeDolls(Player player)
        {
            GameObject doll = Instantiate(Doll, player.BodyPosition, player.Rotation);
            return doll;
        }

        // Update is called once per frame
        void Update()
        {
            // 毎フレームプレイヤーの位置(Vector3) と回転(Quaternion) を更新し、
            // ルームに入室している他ユーザ全員にブロードキャスト送信する
            client.MoveAsync(body.transform.position, right.transform.position, left.transform.position, body.transform.rotation);
            //client.MoveAsync(m_PlayerTransform.position, m_PlayerTransform.rotation);
        }

        async Task OnDestroy()
        {
            // GameClient が破棄される際の StreamingHub クライアント及び gRPC チャネルの解放処理
            await this.client.LeaveAsync();
            await this.client.DisposeAsync();
            await this.channel.ShutdownAsync();
        }
    }

}
