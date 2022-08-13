using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NUI
{
    public class ShowCanHoldUIScript : MonoBehaviour
    {
        public GameObject Red;

        public GameObject Blue;

        public GameObject rht;

        public GameObject lht;

        GameObject rno;

        GameObject lno;

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            rno = rht.GetComponent<RightHoldObjectScript>().NearObject;
            lno = lht.GetComponent<LeftHoldObjectScript>().NearObject;
            if (rno != null)
            {
                Red.SetActive(true);
                Red.transform.position = rno.transform.position;
            }
            else
            {
                Red.SetActive(false);
            }
            if (lno != null)
            {
                Blue.SetActive(true);
                Blue.transform.position = lno.transform.position;
            }
            else
            {
                Blue.SetActive(false);
            }
        }
    }
}
