using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    WebCamInput _webcam;

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
