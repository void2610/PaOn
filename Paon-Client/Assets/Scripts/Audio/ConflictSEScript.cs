using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NAudio
{
    public class ConflictSEScript : MonoBehaviour
    {
        public AudioClip SE;

        void OnCollisionEnter(Collision collision)
        {
            if (
                this.GetComponent<AudioSource>() &&
                this.GetComponent<Rigidbody>()
            )
            {
                if (this.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
                {
                    GetComponent<AudioSource>().PlayOneShot(SE);
                }
            }
        }
    }
}
