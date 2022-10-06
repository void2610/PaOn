using System.Collections;
using System.Collections.Generic;
using Paon.NBordering;
using Paon.NInput;
using UnityEngine;

namespace Paon.NPlayer
{
    public class PlayerCameraScript : MonoBehaviour
    {
        private GameObject mip;

        private BorderingGoalScript goal;

        private BorderingStartScript start;

        private bool tmp = false;

        public int stat = 0;

        //0:最初~ゴール前,1:ゴール後~降りるまで
        private int crouch = 0;

        void Start()
        {
            mip = GameObject.Find("MoveInputProvider");
            if (GameObject.Find("BorderingGoal") != null)
            {
                goal =
                    GameObject
                        .Find("BorderingGoal")
                        .GetComponent<BorderingGoalScript>();
            }
            if (GameObject.Find("BorderingStart") != null)
            {
                start =
                    GameObject
                        .Find("BorderingStart")
                        .GetComponent<BorderingStartScript>();
            }
        }

        void Update()
        {
            //Debug.Log(this.gameObject.transform.eulerAngles.x);
            Debug.Log (stat);
            crouch = mip.GetComponent<MoveInputProvider>().crouch;
            float y = this.gameObject.transform.position.y;
            Vector3 rot = this.gameObject.transform.eulerAngles;
            Vector3 lp = new Vector3(0, 2.3f, 0);

            if (goal != null)
            {
                if (!tmp && goal.goaling)
                {
                    //ゴールした瞬間、1
                    stat = 1;
                }
            }

            if (
                PlayerPrefs.GetString("Room", "none") == "Nature1" ||
                PlayerPrefs.GetString("Room", "none") == "Nature2"
            )
            {
                if (crouch == 1)
                {
                    rot =
                        new Vector3(25,
                            this.gameObject.transform.eulerAngles.y,
                            this.gameObject.transform.eulerAngles.z);
                    lp = new Vector3(0, 1.3f, 0);
                }
                else
                {
                    rot =
                        new Vector3(0,
                            this.gameObject.transform.eulerAngles.y,
                            this.gameObject.transform.eulerAngles.z);
                    lp = new Vector3(0, 2.3f, 0);
                }
            }
            else if (
                PlayerPrefs.GetString("Room", "none") == "Bordering1" ||
                PlayerPrefs.GetString("Room", "none") == "Bordering2"
            )
            {
                // if (stat == 1 && start.starting)
                // {
                //     //ゴールした後にまたスタートしたらまた登ってる判定
                //     stat = 2;
                //     Debug.Log("stat : " + stat);
                // }
                // if (stat == 1)
                // {
                //     //ゴール中だったら下向く
                //     rot =
                //         new Vector3(25,
                //             this.gameObject.transform.eulerAngles.y,
                //             this.gameObject.transform.eulerAngles.z);
                // }
                // else if (stat == 2 && y < 0.5f)
                // {
                //     //登ってるけど下の方の時は正面向く
                //     rot =
                //         new Vector3(0,
                //             this.gameObject.transform.eulerAngles.y,
                //             this.gameObject.transform.eulerAngles.z);
                // }
                // else if (y > 0.5f && stat == 0)
                // {
                //     //初回登っているとき(だんだん上向く)
                //     if (y < 1.5f)
                //     {
                //         rot =
                //             new Vector3(-15 * y,
                //                 this.gameObject.transform.eulerAngles.y,
                //                 this.gameObject.transform.eulerAngles.z);
                //     }
                //     else
                //     {
                //         rot =
                //             new Vector3(-22.5f,
                //                 this.gameObject.transform.eulerAngles.y,
                //                 this.gameObject.transform.eulerAngles.z);
                //     }
                // }
                // else if (y > 0.5f && stat == 2)
                // {
                //     Debug.Log("stat2");
                //     if (y < 1.5f)
                //     {
                //         rot =
                //             new Vector3(-15 * y,
                //                 this.gameObject.transform.eulerAngles.y,
                //                 this.gameObject.transform.eulerAngles.z);
                //     }
                //     else
                //     {
                //         rot =
                //             new Vector3(-22.5f,
                //                 this.gameObject.transform.eulerAngles.y,
                //                 this.gameObject.transform.eulerAngles.z);
                //     }
                // }
                // else
                // {
                //     rot =
                //         new Vector3(0,
                //             this.gameObject.transform.eulerAngles.y,
                //             this.gameObject.transform.eulerAngles.z);
                // }
                if (stat == 0)
                {
                    if (y < 1.5f)
                    {
                        rot =
                            new Vector3(-15 * y,
                                this.gameObject.transform.eulerAngles.y,
                                this.gameObject.transform.eulerAngles.z);
                    }
                    else
                    {
                        rot =
                            new Vector3(-22.5f,
                                this.gameObject.transform.eulerAngles.y,
                                this.gameObject.transform.eulerAngles.z);
                    }
                }
                else if (stat == 1)
                {
                    if (y > 1.5f)
                    {
                        rot =
                            new Vector3(-22.5f,
                                this.gameObject.transform.eulerAngles.y,
                                this.gameObject.transform.eulerAngles.z);
                    }
                    else
                    {
                        rot =
                            new Vector3(-15 * y,
                                this.gameObject.transform.eulerAngles.y,
                                this.gameObject.transform.eulerAngles.z);
                    }
                }

                if (goal != null)
                {
                    tmp = goal.goaling;
                }
            }
            this.gameObject.transform.eulerAngles = rot;
        }
    }
}
