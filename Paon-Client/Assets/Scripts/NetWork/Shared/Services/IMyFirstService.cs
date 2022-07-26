using MagicOnion;
using Paon.NNetwork.Shared.MessagePackObjects;
using UnityEngine;

namespace Paon.NNetWork.Shared.Services
{
    // Defines .NET interface as a Server/Client IDL.
    // The interface is shared between server and client.
    public interface IMyFirstService : IService<IMyFirstService>
    {
        // The return type must be `UnaryResult<T>`.
        UnaryResult<int> SumAsync(int x, int y);
        //UnaryResult<int> GetMyid(Player player);
        //UnaryResult<int> GiveMyPosition(Player player);
        //UnaryResult<Player> GetOthersPosition(int ClientID);
    }
}