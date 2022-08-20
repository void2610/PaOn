using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NNaturePlay
{
    public class LoadCanvasScript : MonoBehaviour
    {
        string data;

        string inputString = Resources.Load<TextAsset>("test").ToString();

        CanvasData cd = new CanvasData();

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            data = Resources.Load<TextAsset>("NaturePlay/").ToString();
        }
    }
}
