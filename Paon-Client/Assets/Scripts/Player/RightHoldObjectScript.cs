using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;
using System;

//handの子オブジェクトのHandTriggerにアタッチする
namespace Paon.NPlayer
{
    public class RightHoldObjectScript : MonoBehaviour
    {
        public Mesh OpenHand;

        public Mesh CloseHand;

        public GameObject NearObject;

        private GameObject Hand;

        private GameObject HandInputProvider;

        private GameObject Player;

        public RightHandInputProvider rmip = null;

        private RightHandMove rhm = null;

        public ObjectHolder oh = new ObjectHolder();

        private Vector3 DefoRotation;

        private float dis = 999;

        private Vector2 coords;

        private GameObject client;

        private DateTime UnHoldTime;

        void Start()
        {
            Player = GameObject.Find("PlayerBody");
            Hand = GameObject.Find("RightHand");
            rmip =
                GameObject
                    .Find("RightHandInputProvider")
                    .GetComponent<RightHandInputProvider>();
            rhm = Hand.GetComponent<RightHandMove>();
            client = GameObject.Find("GameClient");
        }

        void Update()
        {
            coords = rmip.GetPosition();
            if (rmip.CheckHold() == 1)
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
                    UnHoldTime = DateTime.Now;

                    //物を離したときの処理
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.None;
                    oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                        true;
                    client.Givecheck(oh.NowHoldObject, UnHoldTime);
                }
                oh.UnHold();
            }

            if (oh.Holding)
            {
                if (oh.NowHoldObject.tag == "HoldableTag")
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
                    if (NearObject.GetComponent<Outline>())
                    {
                        NearObject.GetComponent<Outline>().OutlineWidth = 0;
                    }
                }
                NearObject = null;
            }
        }

        //接触したオブジェクトが引数otherとして渡される
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HoldableTag") || other.CompareTag("CrayonTag")
            )
            {
                if (NearObject != other.gameObject)
                {
                    if (NearObject != null)
                    {
                        if (NearObject.GetComponent<Outline>())
                        {
                            NearObject.GetComponent<Outline>().OutlineWidth = 0;
                        }
                    }

                    NearObject = other.gameObject;
                    if (NearObject.GetComponent<Outline>())
                    {
                        NearObject.GetComponent<Outline>().OutlineWidth = 8;
                        NearObject.GetComponent<Outline>().OutlineColor =
                            Color.red;
                    }
                }
            }
        }
    }
}
