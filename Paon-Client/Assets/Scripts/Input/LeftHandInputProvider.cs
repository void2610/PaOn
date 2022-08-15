using System.Collections;
using System.Collections.Generic;
using OpenPose;
using UnityEngine;

namespace Paon.NInput
{
    public class LeftHandInputProvider : MonoBehaviour
    {
        GameObject GK;

        private GetKeypoints gk;

        private GetKeypoints.Keypoint[] previous;

        private GetKeypoints.Keypoint[] hand;

        string key = "";

        int hold = 0;

        private float CulculateDistance(Vector2 start, Vector2 end)
        {
            return (float) Vector2.Distance(start, end);
        }

        private Vector2 CalculateDelta(Vector2 pre, Vector2 now)
        {
            float dx = pre.x - now.x;
            float dy = pre.y - now.y;
            return new Vector2(dx, dy);
        }

        ///<summary>
        ///入力されているキーを返すメソッド
        ///</summary>
        /// <returns>入力されているキー</returns>
        public string GetInput()
        {
            return key;
        }

        ///<summary>
        ///推定された手の座標を返すメソッド
        ///</summary>
        /// <returns>手の座標</returns>
        public Vector2 GetPosition()
        {
            try
            {
                return hand[0].coords;
            }
            catch (System.NullReferenceException e)
            {
                return new Vector2(0, 0);
            }
        }

        public int CheckHold()
        {
            return hold;
        }

        void Start()
        {
            GK = GameObject.Find("GetKeypoints");
            gk = GK.GetComponent<GetKeypoints>();
        }

        void Update()
        {
            previous = gk.left;
            if (Input.GetKey(KeyCode.W))
            {
                key = "up";
            }
            else if (Input.GetKey(KeyCode.A))
            {
                key = "left";
            }
            else if (Input.GetKey(KeyCode.S))
            {
                key = "down";
            }
            else if (Input.GetKey(KeyCode.D))
            {
                key = "right";
            }
            else
            {
                key = "none";
            }

            if (Input.GetKey(KeyCode.Q))
            {
                hold = 1;
            }
            else
            {
                hold = 0;
            }
        }

        void LateUpdate()
        {
            hand = gk.left;
            if (hand != null && previous[0] != null)
            {
                //Debug.Log(hand[0].coords.x);
                //Vector2 delta = CalculateDelta(previous[0].coords, hand[0].coords);
                float fd =
                    (float) Vector2.Distance(hand[4].coords, hand[12].coords);
                float thumScore = hand[4].s;
                float MidScore = hand[12].s;

                //Debug.Log("thum: " + thumScore);
                //Debug.Log("Mid: " + MidScore);
                if (fd < 40)
                {
                    //hold = 1;
                }
                else
                {
                    //hold = 0;
                }
                //Debug.Log("LeftKey: " + key);
            }
        }
    }
}
