using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float y = this.gameObject.transform.position.y;
        Vector3 rot;
        if (y > 0.5f && y < 1.5f)
        {
            //登り始め
            rot =
                new Vector3(-30 * (y - 0.5f),
                    this.gameObject.transform.eulerAngles.y,
                    this.gameObject.transform.eulerAngles.z);
        }
        else if (y > 1.5f && y < 4)
        {
            rot =
                new Vector3(-45,
                    this.gameObject.transform.eulerAngles.y,
                    this.gameObject.transform.eulerAngles.z);
        }
        else if (y > 4 && y < 5)
        {
            rot =
                new Vector3(10 * (y - 4),
                    this.gameObject.transform.eulerAngles.y,
                    this.gameObject.transform.eulerAngles.z);
        }
        else if (y > 5)
        {
            rot =
                new Vector3(50,
                    this.gameObject.transform.eulerAngles.y,
                    this.gameObject.transform.eulerAngles.z);
        }
        else
        {
            rot = new Vector3(0, y, this.gameObject.transform.eulerAngles.z);
        }

        this.gameObject.transform.eulerAngles = rot;
    }
}
