using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
    public class InitializeScript : MonoBehaviour
    {
        void Start()
        {
            AudioListener.volume = 0;
        }

        void Update()
        {
            if (Time.time < 5)
            {
                AudioListener.volume = 0;
            }
            else
            {
                AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
            }
        }
    }
}
