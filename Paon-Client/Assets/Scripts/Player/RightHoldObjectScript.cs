using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;

//hand�̎q�I�u�W�F�N�g��HandTrigger�ɃA�^�b�`����
namespace Paon.NPlayer
{
    public class RightHoldObjectScript : MonoBehaviour
    {
        public GameObject NearObject;

        GameObject Hand;

        GameObject HandInputProvider;

        GameObject Player;

        RightHandInputProvider rmip = null;

        RightHandMove rhm = null;

        Vector3 DefoRotation;

        float dis = 999;

        Vector2 handBase;

        Vector3 bodyBase;

        ObjectHolder oh = new ObjectHolder();

        Vector2 coords;

        void Start()
        {
            HandInputProvider = GameObject.Find("RightHandInputProvider");
            Player = GameObject.Find("PlayerBody");
            Hand = GameObject.Find("RightHand");
            rmip = HandInputProvider.GetComponent<RightHandInputProvider>();
            rhm = Hand.GetComponent<RightHandMove>();
        }

        void Update()
        {
            coords = rmip.GetPosition();
            if (rmip.CheckHold() == 1)
            {
                Hand.transform.localScale = new Vector3(0.3f, 0.15f, 0.1f);
                if (NearObject != null && oh.NowHoldObject == null)
                {
                    DefoRotation = NearObject.transform.eulerAngles;
                    handBase = coords;
                    bodyBase = Player.transform.position;
                    oh.HoldObject (NearObject);
                }
            }
            else
            {
                Hand.transform.localScale = new Vector3(0.6f, 0.3f, 0.1f);
                if (oh.NowHoldObject != null)
                {
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.None;
                }
                oh.UnHold();
            }

            if (oh.Holding)
            {
                if (oh.NowHoldObject.tag == "BorderringHoldTag")
                {
                    rhm.canMove = false;

                    //腕を下げるとプレイヤーが上がるようにする
                    Player.transform.position =
                        new Vector3((handBase.x - Hand.transform.position.x) +
                            bodyBase.x,
                            (handBase.y - Hand.transform.position.y) +
                            bodyBase.y);
                }
                else if (oh.NowHoldObject.tag == "BorderringHoldTag")
                {
                    rhm.canMove = true;
                    oh.NowHoldObject.transform.position =
                        this.transform.position;
                    oh.NowHoldObject.transform.eulerAngles =
                        new Vector3(DefoRotation.x,
                            Hand.transform.eulerAngles.y - DefoRotation.y,
                            DefoRotation.z);
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.FreezeRotation;
                }
            }
            else
            {
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
                NearObject = null;
            }
        }

        //�ڐG�����I�u�W�F�N�g������other�Ƃ��ēn�����
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("HoldableTag") || other.CompareTag("HOLDTag"))
            {
                NearObject = other.gameObject;
            }
        }
    }
}
