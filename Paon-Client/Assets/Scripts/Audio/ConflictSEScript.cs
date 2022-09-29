using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NAudio
{
    public class ConflictSEScript : MonoBehaviour
    {
        // Start is called before the first frame update
        public AudioClip SE;

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(this.GetComponent<Rigidbody>().velocity.magnitude);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                if (this.GetComponent<AudioSource>())
                {
                    GetComponent<AudioSource>().PlayOneShot(SE);
                }
            }
        }
    }
}
