using UnityEngine;
using UnityEngine.UI;

namespace MediaPipe.HandPose
{
    public sealed class HandVisualizer : MonoBehaviour
    {
#region Editable attributes
        [SerializeField]
        WebcamInput _webcam = null;

        [Space]
        [SerializeField]
        ResourceSet _resources = null;

        [SerializeField]
        Shader _keyPointShader = null;

        [SerializeField]
        Shader _handRegionShader = null;

        [Space]
        [SerializeField]
        RawImage _mainUI = null;

        [SerializeField]
        RawImage _cropUI = null;


#endregion



#region Private members

        HandPipeline _pipeline;

        (Material keys, Material region) _material;


#endregion


        public Vector3[] KeyPoint;


#region MonoBehaviour implementation

        void Start()
        {
            _pipeline = new HandPipeline(_resources);
            _material =
                (
                    new Material(_keyPointShader),
                    new Material(_handRegionShader)
                );

            // Material initial setup
            _material.keys.SetBuffer("_KeyPoints", _pipeline.KeyPointBuffer);
            _material
                .region
                .SetBuffer("_Image", _pipeline.HandRegionCropBuffer);

            // UI setup
            _cropUI.material = _material.region;
        }

        void OnDestroy()
        {
            _pipeline.Dispose();
            Destroy(_material.keys);
            Destroy(_material.region);
        }

        void LateUpdate()
        {
            // Feed the input image to the Hand pose pipeline.
            _pipeline.ProcessImage(_webcam.Texture);

            // UI update
            _mainUI.texture = _webcam.Texture;
            _cropUI.texture = _webcam.Texture;

            // Debug.Log(HandLandmarkDetector.Score);
            for (int i = 0; i < HandPipeline.KeyPointCount; i++)
            {
                KeyPoint[i] = _pipeline.GetKeyPoint(i);
                Debug.Log(i + ": " + KeyPoint[i]);
            }
        }

        void OnRenderObject()
        {
            // Key point circles
            _material.keys.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Triangles, 96, 21);

            // Skeleton lines
            _material.keys.SetPass(1);
            Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 4 * 5 + 1);
        }


#endregion
    }
} // namespace MediaPipe.HandPose
