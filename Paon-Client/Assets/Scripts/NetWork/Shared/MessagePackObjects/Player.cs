using MessagePack;
using UnityEngine;

namespace Paon.NNetwork.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class Player
    {
        [Key(0)] public int nowFace { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public Vector3 BodyPosition { get; set; }
        [Key(3)] public Vector3 RightPosition { get; set; }
        [Key(4)] public Vector3 LeftPosition { get; set; }
        [Key(5)] public Quaternion Rotation { get; set; }
    }

    [MessagePackObject]
    public class Item
    {
        [Key(0)] public string Name { get; set; }
        [Key(1)] public Vector3 Position { get; set; }
        [Key(2)] public Quaternion Rotation { get; set; }
    }

    [MessagePackObject]
    public class Counter
    {
        [Key(0)] public int Count { get; set; }
        [Key(1)] public string RoomName { get; set; }
    }
}