using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSEScript : MonoBehaviour
{
    void Update()
    {
        Vector3 v = this.GetComponent<Rigidbody>().velocity;
        v.y = 0;
        if (v.magnitude > 0.1f)
        {
            if (this.GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            if (this.GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().Stop();
            }
        }
    }
}
