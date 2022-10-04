using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
    public class Player
    {
        public int id;

        public string name;

        public float timer;

        public bool playingBordering;

        public Player()
        {
            id = 0;
            name = PlayerPrefs.GetString("Name", "NoName");
            timer = 0.0f;
            playingBordering = false;
        }
    }
}
