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

        ObjectHolder oh = new ObjectHolder();

        Vector2 coords;

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
                    DefoRotation = NearObject.transform.eulerAngles;
                    handBase = coords;
                    bodyBase = Player.transform.position;

                    oh.HoldObject (NearObject);

                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.FreezeRotation;
                    oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                        false;
                }
            }
            else
            {
                Hand.transform.localScale = new Vector3(0.6f, 0.3f, 0.1f);
                if (oh.NowHoldObject != null)
                {
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.None;
                    oh.NowHoldObject.GetComponent<Rigidbody>().useGravity =
                        true;
                }
                oh.UnHold();
            }

            if (oh.Holding)
            {
                if (oh.NowHoldObject.tag == "BorderingHOLDTag")
                {
                    lhm.canMove = false;

                    //腕を下げるとプレイヤーが上がるようにする
                    Player.transform.position =
                        new Vector3((handBase.x - Hand.transform.position.x) +
                            bodyBase.x,
                            (handBase.y - Hand.transform.position.y) +
                            bodyBase.y);
                }
                else if (oh.NowHoldObject.tag == "HoldableTag")
                {
                    lhm.canMove = true;
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
