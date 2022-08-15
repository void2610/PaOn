using System.Collections;
using System.Collections.Generic;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NUI
{
    public class ShowCanHoldUIScript : MonoBehaviour
    {
        GameObject Red;

        GameObject Blue;

        GameObject rht;

        GameObject lht;

        GameObject rno;

        GameObject lno;

        GameObject Player;

        void Start()
        {
            Red = GameObject.Find("RedCircle");
            Red.GetComponent<RectTransform>().SetAsLastSibling();
            Blue = GameObject.Find("BlueCircle");
            Blue.GetComponent<RectTransform>().SetAsLastSibling();
            rht = GameObject.Find("RightHandTrigger");
            lht = GameObject.Find("LeftHandTrigger");
            Player = GameObject.Find("PlayerBody");
        }

        // Update is called once per frame
        void Update()
        {
            rno = rht.GetComponent<RightHoldObjectScript>().NearObject;
            lno = lht.GetComponent<LeftHoldObjectScript>().NearObject;
            if (rno != null)
            {
                if (rno.GetComponent<Rigidbody>() != null)
                {
                    if (rno.GetComponent<Rigidbody>().useGravity == true)
                    {
                        Red.SetActive(true);
                        Red.transform.position = rno.transform.position;
                        Red.transform.eulerAngles =
                            Player.transform.eulerAngles;
                    }
                    else
                    {
                        Red.SetActive(false);
                    }
                }
            }
            else
            {
                Red.SetActive(false);
            }
            if (lno != null)
            {
                if (lno.GetComponent<Rigidbody>() != null)
                {
                    if (lno.GetComponent<Rigidbody>().useGravity == true)
                    {
                        Blue.SetActive(true);
                        Blue.transform.position = lno.transform.position;
                        Blue.transform.eulerAngles =
                            Player.transform.eulerAngles;
                    }
                    else
                    {
                        Blue.SetActive(false);
                    }
                }
            }
            else
            {
                Blue.SetActive(false);
            }
        }
    }
}
