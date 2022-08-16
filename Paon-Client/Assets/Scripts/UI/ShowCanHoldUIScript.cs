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
            Blue = GameObject.Find("BlueCircle");
            rht = GameObject.Find("RightHandTrigger");
            lht = GameObject.Find("LeftHandTrigger");
            Player = GameObject.Find("PlayerBody");
        }

        // Update is called once per frame
        void Update()
        {
            rno = rht.GetComponent<RightHoldObjectScript>().NearObject;
            lno = lht.GetComponent<LeftHoldObjectScript>().NearObject;
            Red.transform.eulerAngles = Player.transform.eulerAngles;
            Blue.transform.eulerAngles = Player.transform.eulerAngles;

            /*Blue.transform.eulerAngles = new Vector3(Player.transform.eulerAngles.x,
                    -Player.transform.eulerAngles.y,
                    Player.transform.eulerAngles.z);*/
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
