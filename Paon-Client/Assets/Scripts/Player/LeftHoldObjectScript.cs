using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;
using UnityFx.Outline;

//handの子オブジェクトのHandTriggerにアタッチする
namespace Paon.NPlayer
{
    public class LeftHoldObjectScript : MonoBehaviour
    {
        public Mesh OpenHand;

        public Mesh CloseHand;

        public GameObject NearObject;

        GameObject Hand;

        GameObject HandInputProvider;

        GameObject Player;

        public LeftHandInputProvider lmip = null;

        LeftHandMove lhm = null;

        RightHoldObjectScript rhos = null;

        Vector3 DefoRotation;

        float dis = 999;

        Vector3 handBase;

        Vector3 bodyBase;

        public ObjectHolder oh = new ObjectHolder();

        Vector2 coords;

        string tmp = "none";

        void Start()
        {
            HandInputProvider = GameObject.Find("LeftHandInputProvider");
            Player = GameObject.Find("PlayerBody");
            Hand = GameObject.Find("LeftHand");
            lmip = HandInputProvider.GetComponent<LeftHandInputProvider>();
            lhm = Hand.GetComponent<LeftHandMove>();
            rhos =
                GameObject
                    .Find("RightHandTrigger")
                    .GetComponent<RightHoldObjectScript>();
        }

        void Update()
        {
            coords = lmip.GetPosition();
            if (lmip.CheckHold() == 1)
            {
                Hand.GetComponent<MeshFilter>().mesh = CloseHand;

                //新しく物をつかんだときの処理
                if (NearObject != null && oh.NowHoldObject == null)
                {
                    oh.HoldObject (NearObject);
                    if (oh.NowHoldObject.tag == "HoldableTag")
                    {
                        DefoRotation = NearObject.transform.eulerAngles;
                        oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                            RigidbodyConstraints.FreezeRotation;
                        oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                            false;
                    }
                    else if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                    {
                        Player.GetComponent<Rigidbody>().useGravity = false;
                        handBase = Hand.transform.position;
                        bodyBase = Player.transform.position;
                    }
                    else if (oh.NowHoldObject.tag == "CrayonTag")
                    {
                        oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                            RigidbodyConstraints.FreezeRotation;
                        oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                            false;
                    }
                }
            }
            else
            {
                Hand.GetComponent<MeshFilter>().mesh = OpenHand;
                if (oh.NowHoldObject != null)
                {
                    //物を離したときの処理
                    if (
                        oh.NowHoldObject.tag == "HoldableTag" ||
                        oh.NowHoldObject.tag == "CrayonTag"
                    )
                    {
                        oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                            RigidbodyConstraints.None;
                        oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                            true;
                    }
                    else if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                    {
                        //もう片方が掴んでいなかったら固定解除
                        if (rhos.rmip.CheckHold() == 0)
                        {
                            Player.GetComponent<Rigidbody>().useGravity = true;
                        }
                        Hand.transform.localPosition =
                            new Vector3(-2f, 0, 3.4f);
                    }
                }
                oh.UnHold();
            }

            if (oh.Holding)
            {
                if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                {
                    //手動かなくする
                    Player.GetComponent<PlayerMove>().canMove = false;
                    lhm.canMove = false;

                    //手の位置を固定
                    Hand.transform.position = handBase;
                    if (
                        Mathf
                            .Abs(Vector3
                                .Distance(Player.transform.position,
                                bodyBase)) <
                        0.6f
                    )
                    {
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
                else if (oh.NowHoldObject.tag == "HoldableTag")
                {
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.None;
                    oh.NowHoldObject.transform.position =
                        this.transform.position;
                    oh.NowHoldObject.transform.eulerAngles =
                        new Vector3(DefoRotation.x,
                            Hand.transform.eulerAngles.y - DefoRotation.y,
                            DefoRotation.z);
                }
                else if (oh.NowHoldObject.tag == "CrayonTag")
                {
                    oh.NowHoldObject.transform.position =
                        this.transform.position;
                }
            }
            else
            {
                Player.GetComponent<PlayerMove>().canMove = true;
                lhm.canMove = true;
            }
            if (NearObject != null)
            {
                dis =
                    Vector3
                        .Distance(this.gameObject.transform.position,
                        NearObject.transform.position);
            }
            if (dis > 0.5)
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
                NearObject = null;
            }
            tmp = lmip.GetInput();

            if (
                GameObject
                    .Find("LeftHandTrigger")
                    .GetComponent<LeftHoldObjectScript>()
                    .oh
                    .Holding
            )
            {
                Player.GetComponent<PlayerMove>().canMove = false;
            }
        }

        //接触したオブジェクトが引数otherとして渡される
        void OnTriggerEnter(Collider other)
        {
            if (
                other.CompareTag("HoldableTag") ||
                other.CompareTag("BorderingHOLDTag") ||
                other.CompareTag("CrayonTag")
            )
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
                        Color.blue;
                }
            }
        }
    }
}
