using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;
using UnityFx.Outline;

//handの子オブジェクトのHandTriggerにアタッチする
namespace Paon.NPlayer
{
    public class RightHoldObjectScript : MonoBehaviour
    {
        public Mesh OpenHand;

        public Mesh CloseHand;

        public GameObject NearObject;

        GameObject Hand;

        GameObject HandInputProvider;

        GameObject Player;

        public RightHandInputProvider rmip = null;

        RightHandMove rhm = null;

        Vector3 DefoRotation;

        float dis = 999;

        public ObjectHolder oh = new ObjectHolder();

        Vector2 coords;

        void Start()
        {
            Player = GameObject.Find("PlayerBody");
            Hand = GameObject.Find("RightHand");
            rmip =
                GameObject
                    .Find("RightHandInputProvider")
                    .GetComponent<RightHandInputProvider>();
            rhm = Hand.GetComponent<RightHandMove>();
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
                    //物を離したときの処理
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.None;
                    oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                        true;
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
            else
            {
                Player.GetComponent<PlayerMove>().canMove = true;
                rhm.canMove = true;
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
                        if (NearObject.GetComponent<OutlineBehaviour>())
                        {
                            NearObject
                                .GetComponent<OutlineBehaviour>()
                                .OutlineWidth = 1;
                        }
                    }

                    NearObject = other.gameObject;
                    if (NearObject.GetComponent<OutlineBehaviour>())
                    {
                        NearObject
                            .GetComponent<OutlineBehaviour>()
                            .OutlineWidth = 4;
                        NearObject
                            .GetComponent<OutlineBehaviour>()
                            .OutlineColor = Color.red;
                    }
                }
            }
        }
    }
}
