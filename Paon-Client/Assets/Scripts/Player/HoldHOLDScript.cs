using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;

//hand�̎q�I�u�W�F�N�g��HandTrigger�ɃA�^�b�`����
namespace Paon.NPlayer
{
    public class HoldHOLDScript : MonoBehaviour
    {
        public GameObject NearObject;

        public GameObject Hand;

        public GameObject HandInputProvider;

        public RightHandMoveProvider rmip = null;

        Vector3 DefoRotation;

        float dis = 999;

        ObjectHolder oh = new ObjectHolder();



        void Start()
        {
            rmip = HandInputProvider.GetComponent<RightHandMoveProvider>();
        }

        void Update()
        {
            if (rmip.CheckHold() == 1)
            {
                Hand.transform.localScale = new Vector3(0.3f, 0.15f, 0.1f);
                if (NearObject != null && oh.NowHoldObject == null)
                {
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

            if(oh.Holding && !tmp){
                baseY = Hand.transform.position.y;
            }
            if (oh.Holding)
            {
                //腕を下げるとプレイヤーが上がるようにする
                Hand.transform.position = new Vector3(Hand.transform.position.x, baseY, Hand.transform.position.z);


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
            tmp = oh.Holding;
        }

        //�ڐG�����I�u�W�F�N�g������other�Ƃ��ēn�����
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BorderingHoldTag"))
            {
                NearObject = other.gameObject;
            }
        }
    }
}
