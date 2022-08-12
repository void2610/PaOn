using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Estimate
{
    public sealed class Visualizer : MonoBehaviour
    {
        [SerializeField]
        WebcamInput _webcam = null;

        [SerializeField]
        RawImage _previewImage = null;

        Material _material = null;

        [SerializeField]
        Shader _shader = null;

        // Start is called before the first frame update
        void Start()
        {
            _material = new Material(_shader);
        }

        void OnDestroy()
        {
            Destroy (_material);
        }

        void LateUpdate()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
