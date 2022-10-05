using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NUI
{
    public class NowTextScript : MonoBehaviour
    {
        GameObject InputText;

        void Start()
        {
            InputText = GameObject.Find("Name");
        }

        // Update is called once per frame
        void Update()
        {
            this.GetComponent<Text>().text =
                InputText.GetComponent<InputField>().text;
        }
    }
}
