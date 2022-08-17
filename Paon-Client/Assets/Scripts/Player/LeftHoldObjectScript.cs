using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;

//handの子オブジェクトのHandTriggerにアタッチする
namespace Paon.NPlayer
{
    public class LeftHoldObjectScript : MonoBehaviour
    {
        public GameObject NearObject;

        GameObject Hand;

        GameObject HandInputProvider;

        GameObject Player;

        LeftHandInputProvider lmip = null;

        LeftHandMove lhm = null;

        Vector3 DefoRotation;

        float dis = 999;

        Vector2 handBase;

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
        }

        void Update()
        {
            coords = lmip.GetPosition();
            if (lmip.CheckHold() == 1)
            {
                Hand.transform.localScale = new Vector3(0.3f, 0.15f, 0.1f);
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
                }
            }
            else
            {
                Hand.transform.localScale = new Vector3(0.6f, 0.3f, 0.1f);
                if (
                    oh.NowHoldObject != null &&
                    oh.NowHoldObject.tag == "HoldableTag"
                )
                {
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.None;
                    oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                        true;
                }
                else if (
                    oh.NowHoldObject != null &&
                    oh.NowHoldObject.tag == "BorderingHOLDTag"
                )
                {
                    Player.GetComponent<Rigidbody>().useGravity = true;
                    Hand.transform.localPosition = new Vector3(-1f, 0, 2.4f);
                }
                oh.UnHold();
            }

            if (oh.Holding)
            {
                if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                {
                    Player.GetComponent<PlayerMove>().canMove = false;
                    lhm.canMove = false;
                    Hand.transform.position = handBase;

                    //腕を下げるとプレイヤーが上がるようにする
                    // Player.transform.position =
                    //     new Vector3((handBase.x - Hand.transform.position.x) +
                    //         bodyBase.x,
                    //         (handBase.y - Hand.transform.position.y) +
                    //         bodyBase.y,
                    //         bodyBase.z);
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
                NearObject = null;
            }
            tmp = lmip.GetInput();
        }

        //接触したオブジェクトが引数otherとして渡される
        void OnTriggerEnter(Collider other)
        {
            if (
                other.CompareTag("HoldableTag") ||
                other.CompareTag("BorderingHOLDTag")
            )
            {
                NearObject = other.gameObject;
            }
        }
    }
}
