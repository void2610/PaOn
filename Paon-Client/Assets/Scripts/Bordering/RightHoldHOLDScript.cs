using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using Paon.NPlayer;
using UnityEngine;
using UnityFx.Outline;

namespace Paon.NBordering
{
    public class RightHoldHOLDScript : MonoBehaviour
    {
        public GameObject NearObject;

        GameObject Hand;

        GameObject Player;

        public RightHandInputProvider lmip = null;

        LeftHoldObjectScript lhos = null;

        Vector3 bodyBase;

        public ObjectHolder oh = new ObjectHolder();

        RightHandMove rhm = null;

        string tmp = "none";

        float dis = 999;

        void Start()
        {
            Player = GameObject.Find("PlayerBody");
            Hand = GameObject.Find("RightHand");
            lmip =
                GameObject
                    .Find("RightHandInputProvider")
                    .GetComponent<RightHandInputProvider>();
            rhm = Hand.GetComponent<RightHandMove>();
            lhos =
                GameObject
                    .Find("LeftHandTrigger")
                    .GetComponent<LeftHoldObjectScript>();
        }

        // Update is called once per frame
        void Update()
        {
            //掴んでいるかどうか
            if (lmip.CheckHold() == 1)
            {
                //新しく物をつかんだときの処理
                if (NearObject != null && oh.NowHoldObject == null)
                {
                    oh.HoldObject (NearObject);
                    if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                    {
                        Player.GetComponent<Rigidbody>().useGravity = false;
                        bodyBase = Player.transform.position;
                    }
                }
            }
            else
            {
                if (oh.NowHoldObject != null)
                {
                    //物を離したときの処理
                    if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                    {
                        //もう片方が掴んでいなかったら固定解除
                        if (lhos.lmip.CheckHold() == 0)
                        {
                            Player.GetComponent<Rigidbody>().useGravity = true;
                        }

                        //手の位置を戻す
                        Hand.transform.localPosition = new Vector3(2f, 0, 3.4f);
                    }
                }
                oh.UnHold();
            }

            //ホールドを掴んでいるときの処理
            if (oh.Holding)
            {
                //手動かなくする
                Player.GetComponent<PlayerMove>().canMove = false;
                rhm.canMove = false;
                Hand.transform.position = oh.NowHoldObject.transform.position;

                //この条件式、実際に手で操作できるようになったらいらない
                if (
                    Mathf
                        .Abs(Vector3
                            .Distance(Player.transform.position, bodyBase)) <
                    0.6f
                )
                {
                    //手を動かして体移動
                    if (lmip.GetInput() == "up")
                    {
                        Player.transform.Translate(Vector3.up * 0.03f);
                    }
                    else if (lmip.GetInput() == "down")
                    {
                        Player.transform.Translate(Vector3.down * 0.03f);
                    }
                    else if (lmip.GetInput() == "left")
                    {
                        Player.transform.Translate(Vector3.left * 0.03f);
                    }
                    else if (lmip.GetInput() == "right")
                    {
                        Player.transform.Translate(Vector3.right * 0.03f);
                    }
                }
                else
                {
                    //ここも手で操作できるようになったらいらないかも
                    if (tmp == "up")
                    {
                        Player.transform.Translate(Vector3.up * -0.03f);
                    }
                    else if (tmp == "down")
                    {
                        Player.transform.Translate(Vector3.down * -0.03f);
                    }
                    else if (tmp == "left")
                    {
                        Player.transform.Translate(Vector3.left * -0.03f);
                    }
                    else if (tmp == "right")
                    {
                        Player.transform.Translate(Vector3.right * -0.03f);
                    }
                }
            }
            else
            {
                //掴んで無かったら手動かなくしない
                Player.GetComponent<PlayerMove>().canMove = true;
                rhm.canMove = true;
            }

            //近くの物との距離を計算
            if (NearObject != null)
            {
                dis =
                    Vector3
                        .Distance(this.gameObject.transform.position,
                        NearObject.transform.position);
            }

            tmp = lmip.GetInput();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BorderingHOLDTag"))
            {
                if (NearObject != other.gameObject)
                {
                    if (NearObject != null)
                    {
                        if (NearObject.GetComponent<OutlineBehaviour>())
                        {
                            NearObject
                                .GetComponent<OutlineBehaviour>()
                                .OutlineWidth = 1;
                        }
                    }
                }
                NearObject = other.gameObject;
                if (NearObject.GetComponent<OutlineBehaviour>())
                {
                    NearObject.GetComponent<OutlineBehaviour>().OutlineWidth =
                        4;
                    NearObject.GetComponent<OutlineBehaviour>().OutlineColor =
                        Color.red;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            //つかめない距離になったらアウトラインを消す
            if (other.CompareTag("BorderingHOLDTag"))
            {
                if (NearObject == other.gameObject)
                {
                    if (NearObject.GetComponent<OutlineBehaviour>())
                    {
                        NearObject
                            .GetComponent<OutlineBehaviour>()
                            .OutlineWidth = 1;
                    }
                    NearObject = null;
                }
            }
        }
    }
}
