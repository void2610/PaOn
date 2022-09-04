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
        Debug.Log(this.gameObject.transform.eulerAngles);
        if (this.gameObject.transform.position.y > 0.75f)
        {
            this.gameObject.transform.eulerAngles =
                new Vector3(-30,
                    this.gameObject.transform.eulerAngles.y,
                    this.gameObject.transform.eulerAngles.z);
        }
        else
        {
            this.gameObject.transform.eulerAngles =
                new Vector3(0,
                    this.gameObject.transform.eulerAngles.y,
                    this.gameObject.transform.eulerAngles.z);
        }
    }
}
