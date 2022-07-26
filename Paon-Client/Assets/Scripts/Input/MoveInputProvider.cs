using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NInput
{
    public class MoveInputProvider : MonoBehaviour
    {
        public GameObject GK;

        private GetKeypoints gk;

        private GetKeypoints.Keypoint[] previous;

        private GetKeypoints.Keypoint[] pose;

        string key = "";

        int crouch = 0;
        float def1;
        float def2;
        float predef1;
        float predef2;
        float Rleg;
        float Lleg;

        public int th = 30;

        public string GetInput()
        {
            return key;
        }

        private Vector2 CalculateDelta(Vector2 pre, Vector2 now)
        {
            float dx = pre.x - now.x;
            float dy = pre.y - now.y;
            //Debug.Log(dx);
            return new Vector2(dx, dy);
        }

        void Start()
        {
            gk = GK.GetComponent<GetKeypoints>();
        }

        void Update()
        {
            previous = gk.pose;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                key = "left";
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                key = "right";
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                key = "up";
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                key = "down";
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                key = "space";
            }
            else
            {
                key = "none";
            }
            //aaa
        }

        void LateUpdate()
        {
            pose = gk.pose;
            if (pose != null && previous[8] != null)
            {
                //‘Og
                def1 = pose[11].coords.y - pose[0].coords.y;
                def2 = pose[14].coords.y - pose[0].coords.y;

                if (Mathf.Abs(def1 - predef1) > th || Mathf.Abs(def2 - predef2) > th)
                {
                    key = "up";
                }


                //‰ñ“]
                Rleg = Mathf.Abs(pose[11].coords.x - pose[8].coords.x);
                Lleg = Mathf.Abs(pose[20].coords.x - pose[8].coords.x);
                //Debug.Log(Rleg);
                if (Rleg > 150)
                {
                    key = "right";
                }
                else if (Lleg > 150)
                {
                    key = "left";
                }

                //‚µ‚á‚ª‚Ý
                float Backlength =
                    (float)
                    Vector2.Distance(pose[8].coords, pose[11].coords);
                if (Backlength < 100)
                {
                    crouch = 1;
                }
                else
                {
                    crouch = 0;
                }

                predef1 = def1;
                predef2 = def2;
            }

        }
    }
}
