using System.Collections.Generic;
// Import HolisticBarracuda
using MediaPipe.Holistic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    WebCamInput webCamInput;

    [SerializeField]
    RawImage image;

    [SerializeField]
    Shader poseShader;

    [SerializeField, Range(0, 1)]
    float humanExistThreshold = 0.5f;

    [SerializeField]
    Shader handShader;

    [SerializeField, Range(0, 1)]
    float handScoreThreshold = 0.5f;

    // Set "Packages/HolisticBarracuda/ResourceSet/Holistic.asset" on the Unity Editor.
    [SerializeField]
    HolisticResource holisticResource;

    // Select inference type with pull down on the Unity Editor.
    [SerializeField]
    HolisticInferenceType holisticInferenceType = HolisticInferenceType.full;

    HolisticPipeline holisticPipeline;

    Material poseMaterial;

    Material handMaterial;

    // Lines count of body's topology.
    const int BODY_LINE_NUM = 35;

    // Pairs of vertex indices of the lines that make up body's topology.
    // Defined by the figure in https://google.github.io/mediapipe/solutions/pose.
    readonly List<Vector4> linePair = new List<Vector4> {
            new Vector4(0, 1), new Vector4(1, 2),
            new Vector4(2, 3), new Vector4(3, 7),
            new Vector4(0, 4), new Vector4(4, 5),
            new Vector4(5, 6), new Vector4(6, 8),
            new Vector4(9, 10), new Vector4(11, 12),
            new Vector4(11, 13), new Vector4(13, 15),
            new Vector4(15, 17), new Vector4(17, 19),
            new Vector4(19, 15), new Vector4(15, 21),
            new Vector4(12, 14), new Vector4(14, 16),
            new Vector4(16, 18), new Vector4(18, 20),
            new Vector4(20, 16), new Vector4(16, 22),
            new Vector4(11, 23), new Vector4(12, 24),
            new Vector4(23, 24), new Vector4(23, 25),
            new Vector4(25, 27), new Vector4(27, 29),
            new Vector4(29, 31), new Vector4(31, 27),
            new Vector4(24, 26), new Vector4(26, 28),
            new Vector4(28, 30), new Vector4(30, 32),
            new Vector4(32, 28)
    };

    public bool isDebugEnabled = false;

    void Start()
    {
        // Make instance of HolisticPipeline
        holisticPipeline = new HolisticPipeline();

        poseMaterial = new Material(poseShader);
        handMaterial = new Material(handShader);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.PageUp)) isDebugEnabled = !isDebugEnabled;
        image.texture = webCamInput.inputImageTexture;

        // Inference. Switchable inference type anytime.
        holisticPipeline.ProcessImage(webCamInput.inputImageTexture, holisticInferenceType);
    }

    void OnRenderObject()
    {
        if (holisticInferenceType != HolisticInferenceType.face_only)
            PoseRender();
        if (holisticInferenceType == HolisticInferenceType.pose_only) return;

        if (holisticInferenceType == HolisticInferenceType.full ||
            holisticInferenceType == HolisticInferenceType.pose_and_hand)
        {
            HandRender(false);
            HandRender(true);
        }
    }

    void PoseRender()
    {
        var w = image.rectTransform.rect.width;
        var h = image.rectTransform.rect.height;

        // Set inferenced pose landmark results.
        poseMaterial
                        .SetBuffer("_vertices", holisticPipeline.poseLandmarkBuffer);

        // Set pose landmark counts.
        poseMaterial.SetInt("_keypointCount", holisticPipeline.poseVertexCount);
        poseMaterial.SetFloat("_humanExistThreshold", humanExistThreshold);
        poseMaterial.SetVector("_uiScale", new Vector2(w, h));
        poseMaterial.SetVectorArray("_linePair", linePair);

        if (isDebugEnabled)
        {
            // Draw 35 body topology lines.
            poseMaterial.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, BODY_LINE_NUM);

            // Draw 33 landmark points.
            poseMaterial.SetPass(1);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, holisticPipeline.poseVertexCount);
        }
    }

    void HandRender(bool isRight)
    {
        var w = image.rectTransform.rect.width;
        var h = image.rectTransform.rect.height;
        handMaterial.SetVector("_uiScale", new Vector2(w, h));
        handMaterial
                        .SetVector("_pointColor", isRight ? Color.cyan : Color.yellow);
        handMaterial.SetFloat("_handScoreThreshold", handScoreThreshold);

        // Set inferenced hand landmark results.
        handMaterial.SetBuffer("_vertices", isRight ? holisticPipeline.rightHandVertexBuffer : holisticPipeline.leftHandVertexBuffer);

        if (isDebugEnabled)
        {
            // Draw 21 key point circles.
            handMaterial.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, 96, holisticPipeline.handVertexCount);

            // Draw skeleton lines.
            handMaterial.SetPass(1);
            Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 4 * 5 + 1);
        }


    }

    void OnDestroy()
    {
        // Must call Dispose method when no longer in use.
        holisticPipeline.Dispose();
    }

    public Vector3[] GetLeftVert()
    {
        var result = new Vector4[holisticPipeline.handVertexCount + 1];
        holisticPipeline.leftHandVertexBuffer.GetData(result);
        var res = new Vector3[holisticPipeline.handVertexCount + 1];
        for (int i = 0; i < result.Length; i++)
        {
            res[i] = result[i];
        }
        return res;
    }

    public Vector3[] GetRightVert()
    {
        var result = new Vector4[holisticPipeline.handVertexCount + 1];
        holisticPipeline.rightHandVertexBuffer.GetData(result);
        var res = new Vector3[holisticPipeline.handVertexCount + 1];
        for (int i = 0; i < result.Length; i++)
        {
            res[i] = result[i];
        }
        return res;
    }

    public Vector3[] GetPoseVertices()
    {
        var result = new Vector4[holisticPipeline.poseVertexCount];
        holisticPipeline.poseLandmarkWorldBuffer.GetData(result);
        var res = new Vector3[holisticPipeline.poseVertexCount];
        for (int i = 0; i < result.Length; i++)
        {
            res[i] = result[i];
        }
        return res;
    }
}
