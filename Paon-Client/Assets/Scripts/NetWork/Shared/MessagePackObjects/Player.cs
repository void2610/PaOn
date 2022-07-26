using MessagePack;
using UnityEngine;

namespace Paon.NNetwork.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class Player
    {
        [Key(0)] public string Name { get; set; }
        [Key(1)] public Vector3 Position { get; set; }
        [Key(2)] public Vector3 Rotation { get; set; }
        [Key(3)] public int ID { get; set; }
        public Player(string Name)
        {
            this.Name = Name;
            Position = new Vector3(0, 0, 0);
            ID = -1;
        }
    }
}