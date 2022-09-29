//using System;
//using MagicOnion;
//using MagicOnion.Server;
//using MagicOnion.Server.Hubs;
//using Paon.NNetWork.Shared.Services;
//using System.ServiceProcess;
//using Paon.NNetwork.Shared.MessagePackObjects;
//using UnityEngine;


//namespace Paon.NetWork.Services
//{
//    // Implements RPC service in the server project.
//    // The implementation class must inehrit `ServiceBase<IMyFirstService>` and `IMyFirstService`
//    public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
//    {
//        int count = -1;
//        int maxPlayer = 8;
//        public Player[] PlayerArray = new Player[8];
//        IInMemoryStorage<Player> PlayerStrage;

//        void Substitution()
//        {
//            for (int i = 0; i < maxPlayer; i++)
//            {
//                PlayerArray[i] = new Player("whoareyou");
//            }
//        }

//        // `UnaryResult<T>` allows the method to be treated as `async` method.
//        public async UnaryResult<int> SumAsync(int x, int y)
//        {
//            Substitution();
//            Console.WriteLine($"Received:{PlayerArray[2].Name}");
//            Console.WriteLine($"Received:{x}, {y}");
//            Console.WriteLine($"init end");
//            return x + y;
//        }
//        //public async UnaryResult<int> GetMyid(Player player)
//        //{
//        //    Console.WriteLine($"get start");
//        //    player.ID = -1;
//        //    Console.WriteLine($"Received:{count}");
//        //    if (player.ID == -1)
//        //    {
//        //        count++;
//        //        Console.WriteLine($"{PlayerArray[0].Name}");
//        //        PlayerArray[count].Name = player.Name;
//        //        PlayerArray[count].ID = count;
//        //        player.ID = PlayerArray[count].ID;
//        //    }
//        //    Console.WriteLine($"Received:{PlayerArray[count].ID}");
//        //    return player.ID;
//        //}
//        //public async UnaryResult<int> GiveMyPosition(Player player)
//        //{
//        //    Console.WriteLine($"Received:{player.ID}");
//        //    PlayerArray[player.ID] = player;
//        //    for (int i = 0; maxPlayer > i; i++)
//        //    {
//        //        if (PlayerArray[i].ID == player.ID)
//        //        {
//        //            //PlayerArray[i].Position = player.Position;
//        //            //Console.WriteLine($"Received:{PlayerArray[0].ID}");
//        //            Console.WriteLine($"Received:aaa");
//        //        }
//        //        else
//        //        {
//        //            Console.WriteLine($"Received:bbb");
//        //        }
//        //    }
//        //    return player.ID;
//        //}
//        //public async UnaryResult<Player> GetOthersPosition(int ClientID)
//        //{
//        //    Console.WriteLine($"Received: GetOthersPosition {PlayerArray[0].ID}");
//        //    return PlayerArray[ClientID];
//        //}
//    }
//}