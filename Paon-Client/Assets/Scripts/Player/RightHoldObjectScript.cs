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
                    DefoRotation = NearObject.transform.eulerAngles;
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

                oh.NowHoldObject.transform.position = this.transform.position;
                oh.NowHoldObject.transform.eulerAngles = new Vector3(DefoRotation.x, Hand.transform.eulerAngles.y - DefoRotation.y, DefoRotation.z);
                oh.NowHoldObject.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeRotation;

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
            if (other.CompareTag("HoldableTag"))
            {
                NearObject = other.gameObject;
            }
        }
    }
}
