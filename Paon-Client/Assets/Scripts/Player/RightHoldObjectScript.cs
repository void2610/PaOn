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

        public GameObject Hand;

        public GameObject HandInputProvider;

        public GameObject Player;

        public RightHandMoveProvider rmip = null;

        public RightHandMove rhm = null;

        Vector3 DefoRotation;

        float dis = 999;

        bool tmp = false;

        Vector2 handBase;
        Vector3 bodyBase;

        ObjectHolder oh = new ObjectHolder();

        Vector2 coords;
        void Start()
        {
            rmip = HandInputProvider.GetComponent<RightHandMoveProvider>();
            rhm = Hand.GetComponent<RightHandMove>();
        }

        void Update()
        {
            coords = inputProvider.GetPosition();
            if (rmip.CheckHold() == 1)
            {
                Hand.transform.localScale = new Vector3(0.3f, 0.15f, 0.1f);
                if (NearObject != null && oh.NowHoldObject == null)
                {
                    DefoRotation = NearObject.transform.eulerAngles;
                    handBase = coords;
                    bodyBase =
                    oh.HoldObject(NearObject);
                }
            }
            else
            {
                Hand.transform.localScale = new Vector3(0.6f, 0.3f, 0.1f);
                if (oh.NowHoldObject != null)
                {
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
                oh.UnHold();
            }


            if (oh.Holding)
            {
                if(oh.NowHoldObject.tag == "BorderringHoldTag"){
                    rhm.canMove = false;
                    //腕を下げるとプレイヤーが上がるようにする
                    Hand.transform.position = new Vector3(Hand.transform.position.x, baseY, Hand.transform.position.z);
                }
                else if(oh.NowHoldObject.tag == "BorderringHoldTag"){
                    rhm.canMove = true;
                    oh.NowHoldObject.transform.position = this.transform.position;
                    oh.NowHoldObject.transform.eulerAngles = new Vector3(DefoRotation.x, Hand.transform.eulerAngles.y - DefoRotation.y, DefoRotation.z);
                    oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezeRotation;
                }
            }
            else{
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
            if (other.CompareTag("HoldableTag") || other.CompareTag("BorderingHoldTag"))
            {
                NearObject = other.gameObject;
            }
        }
    }
}
