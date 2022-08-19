using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MediaPipe.HandPose;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    WebCamInput _webcam;

    [SerializeField]
    RawImage _image;

    [SerializeField]
    Shader _shader;

    [SerializeField, Range(0, 1)]
    float handScoreThreshold = 0.5f;

    [SerializeField]
    HandPoseResource resource;

    HandEstimation _handPose;

    Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _handPose = new HandEstimation(resource);
        _material = new Material(_shader);
    }

    void LateUpdate()
    {
        _image.texture = _webcam.inputImageTexture;
        _handPose.ProcessImage(_webcam.inputImageTexture);
    }

    void HandRender(bool isRight)
    {
        var w = _image.rectTransform.rect.width;
        var h = _image.rectTransform.rect.height;

        // _material.setVector("_uiScale", new Vector2(w, h));
        // _material.setVector("_pointColor", isRight ? Color.cyan : Color.yellow);
        // _material.setFloat("_handScoreThreshold", handScoreThreshold);
        _material
            .SetBuffer("_vertices",
            isRight
                ? _handPose.rightHandVertexBuffer
                : _handPose.leftHandVertexBuffer);

        _material.SetPass(0);
        Graphics
            .DrawProceduralNow(MeshTopology.Triangles,
            96,
            _handPose.handVertexCount);

        _material.SetPass(1);
        Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 4 * 5 + 1);
    }

    void OnDestroy()
    {
        _handPose.Dispose();
    }
}
