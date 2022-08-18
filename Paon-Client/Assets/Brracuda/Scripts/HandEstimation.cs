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

            commonCS.SetVector("_spadScale", scale);
            commonCS.SetInt("_spadWidth", input.width);
            commonCS.SetTexture(0, "_input", input);
            commonCS.SetTexture(0, "_render", _render);
            commonCS.Dispatch(0, imageWidth / 8, imageWidth / 8, 1);

            HandProcess (input, _render, scale);
        }

        void HandProcess(Texture input, Texture _render, Vector2 spadScale)
        {
            palmDetector.ProcessImage (_render);

            int[] countReadCache = new int[1];
            palmDetector.CountBuffer.GetData(countReadCache, 0, 0, 1);
            var handDetectionCount = countReadCache[0];
            handDetectionCount = (int) Mathf.Min(handDetectionCount, 2);

            if (handDetectionCount > 0)
            {
                handCS.SetInt("_detectionCount", handDetectionCount);
                handCS.SetFloat("_regionDetectDt", Time.deltaTime);
                handCS
                    .SetBuffer(0,
                    "_palmDetections",
                    palmDetector.DetectionBuffer);
                handCS
                    .SetBuffer(0, "_handsRegionFromPalm", handsRegionFromPalm);
                handCS.Dispatch(0, 1, 1, 1);
            }

            handCS.SetVector("_spadScale", spadScale);
            handCS.SetInt("_isMirrored", 1);
            for (int i = 0; i < handDetectionCount; i++)
            {
                handCS.SetInt("_handRegionIndex", i);

                handCS.SetInt("_handCropSize", handCropImageSize);
                handCS.SetTexture(2, "_handCropInput", inputTexture);
                handCS.SetBuffer(2, "_handCropRegion", handsRegionFromPalm);
                handCS.SetBuffer(2, "_handCropOutput", handCropBuffer);
                handCS
                    .Dispatch(2,
                    handCropImageSize / 8,
                    handCropImageSize / 8,
                    1);

                handLandmarkDetector.ProcessImage (handCropBuffer);

                var ScoreCache = new Vector4[1];
                handLandmarkDetector.OutputBuffer.GetData(scoreCache, 0, 0, 1);
                float score = scoreCache[0].x;
                float handedness = scoreCache[0].y;
                bool isRight = handedness > 0.5f;

                handCS.SetFloat("_handPostDt", Time.deltaTime);
                handCS
                    .SetBuffer(3,
                    "_handPostInput",
                    handLandmarkDetector.OutputBuffer);
                handCS.SetBuffer(3, "_handPostRegion", handsRegionFromPalm);
                handCS
                    .SetBuffer(3,
                    "_handPostOutput",
                    isRight ? rightHandVertexBuffer : leftHandVertexBuffer);
                handCS
                    .SetBuffer(3,
                    "_handPostDeltaOutput",
                    isRight
                        ? deltaRightHandVertexBuffer
                        : deltaLeftHandVertexBuffer);
                handCS.Dispatch(3, 1, 1, 1);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
