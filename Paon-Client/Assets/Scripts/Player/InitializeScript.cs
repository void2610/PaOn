using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
    public class InitializeScript : MonoBehaviour
    {
        void Start()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        }

        void Update()
        {
            if (Time.time < 10)
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
