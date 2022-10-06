using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NAudio
{
    public class ConflictSEScript : MonoBehaviour
    {
        public AudioClip SE;

        private float speed = 0.1f;

        void OnCollisionEnter(Collision collision)
        {
            if (
                this.GetComponent<AudioSource>() &&
                this.GetComponent<Rigidbody>()
            )
            {
                if (this.GetComponent<Rigidbody>().velocity.magnitude > speed)
                {
                    GetComponent<AudioSource>().PlayOneShot(SE);
                }
            }
        }
    }
}
