using System;
using System.Collections;
using System.Collections.Generic;
using MediaPipe.BlazePalm;
using MediaPipe.HandLandmark;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPipe.HandPose
{
    public class HandEstimation : MonoBehaviour
    {
        public int handVertexCount => HandLandmarkDetector.VertexCount;

        public ComputeBuffer leftHandVertexBuffer;

        public ComputeBuffer rightHandVertexBuffer;

        const int imageWidth = 128;

        const int handCropImageSize = HandLandmarkDetector.ImageSize;

        ComputeShader commonCS;

        ComputeShader handCS;

        PalmDetector palmDetector;

        HandLandmarkDetector handLandmarkDetector;

        RenderTexture _render;

        ComputeBuffer handsRegionFromPalm;

        ComputeBuffer handCropBuffer;

        ComputeBuffer deltaLeftHandVertexBuffer;

        ComputeBuffer deltaRightHandVertexBuffer;

        public HandEstimation(HandPoseResource resource)
        {
            commonCS = resource.commonCS;
            handCS = resource.handCS;

            palmDetector = new PalmDetector(resource.blazePalmResource);
            handLandmarkDetector =
                new HandLandmarkDetector(resource.handLandmarkResource);

            leftHandVertexBuffer =
                new ComputeBuffer(handVertexCount + 1, sizeof(float) * 4);
            rightHandVertexBuffer =
                new ComputeBuffer(handVertexCount + 1, sizeof(float) * 4);

            _render =
                new RenderTexture(imageWidth,
                    imageWidth,
                    0,
                    RenderTextureFormat.ARGB32);
            _render.enableRandomWrite = true;
            _render.Create();

            handsRegionFromPalm = new ComputeBuffer(2, sizeof(float) * 24);
            handCropBuffer =
                new ComputeBuffer(handCropImageSize * handCropImageSize * 3,
                    sizeof(float));
            deltaLeftHandVertexBuffer =
                new ComputeBuffer(handVertexCount, sizeof(float) * 4);
            deltaRightHandVertexBuffer =
                new ComputeBuffer(handVertexCount, sizeof(float) * 4);
        }

        public void Dispose()
        {
            palmDetector.Dispose();
            handLandmarkDetector.Dispose();

            leftHandVertexBuffer.Dispose();
            rightHandVertexBuffer.Dispose();

            _render.Release();

            handsRegionFromPalm.Dispose();

            handCropBuffer.Dispose();
            deltaLeftHandVertexBuffer.Dispose();
            deltaRightHandVertexBuffer.Dispose();
        }

        public void ProcessImage(Texture input, float threshold = 0.7f)
        {
            var scale =
                new Vector2(Mathf.Max((float) input.height / input.width, 1),
                    Mathf.Max(1, (float) input.width / input.height));
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
