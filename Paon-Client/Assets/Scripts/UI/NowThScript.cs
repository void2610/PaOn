using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NUI
{
    public class NowThScript : MonoBehaviour
    {
        private GameObject InputText;

        void Start()
        {
            InputText = GameObject.Find("Th");
        }

        void Update()
        {
            this.GetComponent<Text>().text =
                InputText.GetComponent<InputField>().text;
        }
    }
}
