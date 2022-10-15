using MessagePack;
using UnityEngine;
using System;

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
        [Key(6)] public float red { get; set; }
        [Key(7)] public float blue { get; set; }
        [Key(8)] public float green { get; set; }
        [Key(9)] public bool Flag { get; set; }
        [Key(10)] public DateTime OutTime { get; set; }
        [Key(11)] public bool Check { get; set; }
    }

    [MessagePackObject]
    public class Item
    {
        [Key(0)] public string Name { get; set; }
        [Key(1)] public Vector3 Position { get; set; }
        [Key(2)] public Quaternion Rotation { get; set; }
        [Key(3)] public string Presenter { get; set; }
        [Key(4)] public DateTime ReleaseTime { get; set; }
    }

    [MessagePackObject]
    public class Counter
    {
        [Key(0)] public int Count { get; set; }
        [Key(1)] public string RoomName { get; set; }
    }
}