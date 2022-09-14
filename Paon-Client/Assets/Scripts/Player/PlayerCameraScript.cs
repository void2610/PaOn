using System.Collections;
using System.Collections.Generic;
using Paon.NBordering;
using UnityEngine;

namespace Paon.NPlayer
{
    public class PlayerCameraScript : MonoBehaviour
    {
        BorderingGoalScript goal;

        BorderingStartScript start;

        public int stat = 0;

        bool tmp = false;

        void Start()
        {
            goal =
                GameObject
                    .Find("BorderingGoal")
                    .GetComponent<BorderingGoalScript>();
            start =
                GameObject
                    .Find("BorderingStart")
                    .GetComponent<BorderingStartScript>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!tmp && goal.goaling)
            {
                stat = 1;
            }
            if (stat == 1 && start.starting)
            {
                stat = 2;
            }
            float y = this.gameObject.transform.position.y;
            Vector3 rot;
            if (stat == 1)
            {
                rot =
                    new Vector3(25,
                        this.gameObject.transform.eulerAngles.y,
                        this.gameObject.transform.eulerAngles.z);
            }
            else if (stat == 2 && y < 0.5f)
            {
                rot =
                    new Vector3(0,
                        this.gameObject.transform.eulerAngles.y,
                        this.gameObject.transform.eulerAngles.z);
            }
            else if (y > 0.5f && stat == 0)
            {
                if (y < 1.5f)
                {
                    rot =
                        new Vector3(-15 * y,
                            this.gameObject.transform.eulerAngles.y,
                            this.gameObject.transform.eulerAngles.z);
                }
                else
                {
                    rot =
                        new Vector3(-22.5f,
                            this.gameObject.transform.eulerAngles.y,
                            this.gameObject.transform.eulerAngles.z);
                }
            }
            else if (y > 0.5f && stat == 2)
            {
                Debug.Log("stat2");
                if (y < 1.5f)
                {
                    rot =
                        new Vector3(-15 * y,
                            this.gameObject.transform.eulerAngles.y,
                            this.gameObject.transform.eulerAngles.z);
                }
                else
                {
                    rot =
                        new Vector3(-22.5f,
                            this.gameObject.transform.eulerAngles.y,
                            this.gameObject.transform.eulerAngles.z);
                }
            }
            else
            {
                rot =
                    new Vector3(0,
                        this.gameObject.transform.eulerAngles.y,
                        this.gameObject.transform.eulerAngles.z);
            }

            this.gameObject.transform.eulerAngles = rot;
            tmp = goal.goaling;
        }
    }
}
