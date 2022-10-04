using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
    public class ObjectHolder
    {
        public bool Holding = false;

        public GameObject NowHoldObject;

        public GameObject HoldObject(GameObject go)
        {
            if (go == null)
            {
                Holding = false;
                return null;
            }
            else
            {
                NowHoldObject = go;
                Holding = true;
                return NowHoldObject;
            }
        }

        ///<summary>
        ///掴んでいるオブジェクトを解除するメソッド
        ///</summary>
        ///<returns>void</returns>
        public void UnHold()
        {
            Holding = false;
            NowHoldObject = null;
        }
    }
}
