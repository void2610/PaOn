using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using MediaPipe.HandPose;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
//     [SerializeField]
//     WebCamInput _webcam;

//     [SerializeField]
//     RawImage _image = null;

//     [SerializeField]
//     Shader _shader = null;

//     [SerializeField, Range(0, 1)]
//     float handScoreThreshold = 0.5f;

//     [SerializeField]
//     Resource resource = null;

//     HandPose _handPose;

//     Material _material;

//     // Start is called before the first frame update
//     void Start()
//     {
//         _handPose = new HandPose(resource);
//         _material = new Material(_shader);
//     }

//     void OnDestroy()
//     {
//         Destroy (_material);
//     }

//     void LateUpdate()
//     {
//         _image.texture = _webcam.inputTexture;
//         _handPose.ProcessImage(_webcam.inputImageTexture);
//     }

// void HandRender(bool isRight){
// 	var w = _image.rectTransform..rect.width;
// 	var h = _image.rectTransform..rect.height;
// 	_material.setVector("_uiScale", new Vector2(w, h));
// 	_material.setVector("_pointColor", isRight ? Color.cyan : Color.yellow);
// 	_material.setFloat("_handScoreThreshold", handScoreThreshold);
// }

//     void OnDestroy()
//     {
//         HandPose.Dispose();
//     }
}
