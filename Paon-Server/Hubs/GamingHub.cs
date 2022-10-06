using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using Paon.NNetwork.Shared.Hubs;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;
using System;
using System.Collections.Generic;


namespace Paon.NNetwork.Hubs
{
    public class GamingHub : StreamingHubBase<IGamingHub, IGamingHubReceiver>, IGamingHub
    {
        // IGroup を使用することで同一のグループに所属している他ユーザ全員に対して
        // 一斉にブロードキャスト送信を行うことが出来る (オンラインゲームで言うルームの概念)
        IGroup room;

        // ルーム内での自分の情報 (IGamingHub.cs で定義した Player の情報)
        Player self;

        //ルーム内でのItemの情報
        Item[] mono;

        // ルームに入室しているユーザ全員（自分も含む）の情報を保持して扱うための変数
        IInMemoryStorage<Player> storage;
        IInMemoryStorage<Item> memory;
        //IInMemoryStorage<Counter> Count;

        // 指定したルームに入室するための関数
        // 入室するルーム名及び、ユーザ自身の情報(ユーザ名,位置(Vector3),回転(Quaternion)) を引数に取る
        public async Task<Player[]> JoinAsync(string roomName, string userName, Vector3 _body, Vector3 _right, Vector3 _left, Quaternion rotation, float Red, float Blue, float Green)
        {
            self = new Player() { Name = userName, BodyPosition = _body, RightPosition = _right, LeftPosition = _left, Rotation = rotation, red = Red, blue = Blue, green = Green};
            // ルームにユーザが入室する
            (room, storage) = await Group.AddAsync(roomName, self);

            Console.WriteLine(storage.AllValues.ToArray().Length);

            if (storage.AllValues.ToArray().Length == 0)
            {
                Broadcast(room).FiastPlayer();
            }

            // ルームに入室している他ユーザ全員に
            // 入室したユーザの情報をブロードキャスト送信する
            Broadcast(room).OnJoin(self, self.red, self.blue, self.green);

            // ルームに入室している他ユーザ全員の情報を配列で取得する
            return storage.AllValues.ToArray();
        }

        // ユーザがルームから退出する
        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);

            // ルームに入室している他ユーザ全員に
            // ルームから退出したことをユーザの情報と共にブロードキャスト送信する
            Broadcast(room).OnLeave(self);
        }

        public async Task ItemJoin(string name, Vector3 position, Quaternion rotation, int i)
        {
            mono[i] = new Item() { Name = name, Position = position, Rotation = rotation };
        }

        // ユーザがルームの中で動く
        public async Task MoveAsync(Vector3 _body, Vector3 _right, Vector3 _left, Quaternion rotation)
        {
            // 動いたユーザの位置(xyz) と回転(quaternion) を設定する
            self.BodyPosition = _body;
            self.RightPosition = _right;
            self.LeftPosition = _left;
            self.Rotation = rotation;

            // 動いたユーザの最新の位置(Vector3)と回転(Quaternion) を
            // ルームに入室している他ユーザ全員にユーザの最新情報 (Player) をブロードキャスト送信する
            Broadcast(room).OnMove(self);
        }

        public async Task ItemAsync(string name, Vector3 position, Quaternion rotation, int i)
        {
            Console.WriteLine(name);

            mono[i].Name = name;
            mono[i].Position = position;
            mono[i].Rotation = rotation;
            
            Broadcast(room).OnItem(mono[i]);
        }

        public async Task FaceAsync(int FaceID)
        {
            //Console.WriteLine("hi");

            Broadcast(room).ChengeFace(self, FaceID);
        }

        public async Task TimeAsync(string name, float time)
        {
            Console.WriteLine("Goalした人がいるよ。");

            Broadcast(room).OnGoal(name, time);
        }
    }
}