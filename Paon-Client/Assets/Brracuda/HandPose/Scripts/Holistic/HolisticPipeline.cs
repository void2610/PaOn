using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mediapipe.BlazePose;
using MediaPipe.BlazePalm;
using MediaPipe.HandLandmark;
using UnityEngine;

namespace MediaPipe.Holistic
{
    public class HolisticPipeline : System.IDisposable
    {
        #region public variables
        // Count of pose landmarks vertices.
        public int poseVertexCount => blazePoseDetecter.vertexCount;

        /*
    Pose landmark 	result Buffer.
    'poseLandmarkBuffer' is array of float4 type.
    0~32 index datas are pose landmark.
        Check below Mediapipe document about relation between index and landmark position.
        https://google.github.io/mediapipe/solutions/pose#pose-landmark-model-blazepose-ghum-3d
        Each data factors are
            x: x cordinate value of pose landmark ([0, 1]).
            y: y cordinate value of pose landmark ([0, 1]).
            z: Landmark depth with the depth at the midpoint of hips being the origin.
               The smaller the value the closer the landmark is to the camera. ([0, 1]).
            w: The score of whether the landmark position is visible ([0, 1]).

    33 index data is the score whether human pose is visible ([0, 1]).
    This data is (score, 0, 0, 0).
    */
        public ComputeBuffer poseLandmarkBuffer =>
            blazePoseDetecter.outputBuffer;

        /*
    Pose world landmark result Buffer.
    'poseLandmarkWorldBuffer' is array of float4 type.
    0~32 index datas are pose world landmark.
        Each data factors are
            x, y and z: Real-world 3D coordinates in meters with the origin at the center between hips.
            w: The score of whether the world landmark position is visible ([0, 1]).
    33 index data is the score whether human pose is visible ([0, 1]). This data is (score, 0, 0, 0).
    */
        public ComputeBuffer poseLandmarkWorldBuffer =>
            blazePoseDetecter.worldLandmarkBuffer;

        // Count of hand landmarks vertices.
        public int handVertexCount => HandLandmarkDetector.VertexCount;

        /*
    Hand landmark result buffer.
    0~20 index datas are hand landmark.
        Check below Mediapipe document about relation between index and landmark position.
        https://google.github.io/mediapipe/solutions/hands.html#hand-landmark-model
        Each data factors are
            x: x cordinate value of hand landmark ([0, 1]).
            y: y cordinate value of hand landmark ([0, 1]).
            z: Landmark depth with the depth at the wrist being the origin.
               The smaller the value the closer the landmark is to the camera.
            w: 1.
    21 index data is the score whether hand is visible ([0, 1]) and handedness (0.5 or more is right hand).
    This data is (score, handedness, 0, 0).
    */
        public ComputeBuffer leftHandVertexBuffer;

        public ComputeBuffer rightHandVertexBuffer;
        #endregion



        #region constant number
        const int letterboxWidth = 128;

        const int handCropImageSize = HandLandmarkDetector.ImageSize;
        #endregion



        #region private variables
        ComputeShader commonCs;

        ComputeShader handCs;

        BlazePoseDetecter blazePoseDetecter;

        PalmDetector palmDetector;

        HandLandmarkDetector handLandmarkDetector;

        // HandLandmark _landmark;
        RenderTexture letterBoxTexture;

        ComputeBuffer handsRegionFromPalm;

        ComputeBuffer leftHandRegionFromPose;

        ComputeBuffer rightHandRegionFromPose;

        ComputeBuffer handCropBuffer;

        ComputeBuffer deltaLeftHandVertexBuffer;

        ComputeBuffer deltaRightHandVertexBuffer;
        #endregion



        #region public methods
        public HolisticPipeline(
            HolisticResource resource,
            BlazePoseModel blazePoseModel = BlazePoseModel.full
        )
        {
            commonCs = resource.commonCs;
            handCs = resource.handCs;

            blazePoseDetecter =
                new BlazePoseDetecter(resource.blazePoseResource,
                    blazePoseModel);
            palmDetector = new PalmDetector(resource.blazePalmResource);
            handLandmarkDetector =
                new HandLandmarkDetector(resource.handLandmarkResource);

            // Output length is hand landmark count(21) + score(1).
            leftHandVertexBuffer =
                new ComputeBuffer(handVertexCount + 1, sizeof(float) * 4);
            rightHandVertexBuffer =
                new ComputeBuffer(handVertexCount + 1, sizeof(float) * 4);

            letterBoxTexture =
                new RenderTexture(letterboxWidth,
                    letterboxWidth,
                    0,
                    RenderTextureFormat.ARGB32);
            letterBoxTexture.enableRandomWrite = true;
            letterBoxTexture.Create();

            handsRegionFromPalm = new ComputeBuffer(2, sizeof(float) * 24);
            leftHandRegionFromPose = new ComputeBuffer(1, sizeof(float) * 24);
            rightHandRegionFromPose = new ComputeBuffer(1, sizeof(float) * 24);
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
            blazePoseDetecter.Dispose();
            palmDetector.Dispose();
            handLandmarkDetector.Dispose();

            leftHandVertexBuffer.Dispose();
            rightHandVertexBuffer.Dispose();

            letterBoxTexture.Release();

            handsRegionFromPalm.Dispose();

            leftHandRegionFromPose.Dispose();
            rightHandRegionFromPose.Dispose();
            handCropBuffer.Dispose();
            deltaLeftHandVertexBuffer.Dispose();
            deltaRightHandVertexBuffer.Dispose();
        }

        public void ProcessImage(
            Texture inputTexture,
            HolisticInferenceType inferenceType = HolisticInferenceType.full,
            BlazePoseModel blazePoseModel = BlazePoseModel.full,
            float poseDetectionThreshold = 0.75f,
            float poseDetectionIouThreshold = 0.3f
        )
        {
            // Inference pose landmark with BlazePoseBarracuda.
            if (inferenceType != HolisticInferenceType.face_only)
                blazePoseDetecter
                    .ProcessImage(inputTexture,
                    blazePoseModel,
                    poseDetectionThreshold,
                    poseDetectionIouThreshold);
            if (inferenceType == HolisticInferenceType.pose_only) return;

            // Letterboxing scale factor
            var scale =
                new Vector2(Mathf
                        .Max((float)inputTexture.height / inputTexture.width,
                        1),
                    Mathf
                        .Max(1,
                        (float)inputTexture.width / inputTexture.height));

            // Image scaling and padding
            // Output image is letter-box image.
            // For example, top and bottom pixels of `letterboxTexture` are black if `inputTexture` size is 1920(width)*1080(height)
            commonCs.SetVector("_spadScale", scale);
            commonCs.SetInt("_letterboxWidth", letterboxWidth);
            commonCs.SetTexture(0, "_letterboxInput", inputTexture);
            commonCs.SetTexture(0, "_letterboxTexture", letterBoxTexture);
            commonCs.Dispatch(0, letterboxWidth / 8, letterboxWidth / 8, 1);

            var position = new Vector4[poseVertexCount];
            poseLandmarkWorldBuffer.GetData(position);

            // UnityEngine.Debug.Log("left: " + position[15]);
            // UnityEngine.Debug.Log("right: " + position[16]);
            // Inference hands landmark.
            if (
                inferenceType == HolisticInferenceType.full ||
                inferenceType == HolisticInferenceType.pose_and_hand
            ) HandProcess(inputTexture, letterBoxTexture, scale);
        }
        #endregion



        #region private methods
        void HandProcess(
            Texture inputTexture,
            Texture letterBoxTexture,
            Vector2 spadScale
        )
        {
            // Inference palm detection.
            palmDetector.ProcessImage(letterBoxTexture);

            int[] countReadCache = new int[1];
            palmDetector.CountBuffer.GetData(countReadCache, 0, 0, 1);
            var handDetectionCount = countReadCache[0];
            handDetectionCount = (int)Mathf.Min(handDetectionCount, 2);

            bool isNeedLeftFallback = (handDetectionCount == 0);
            bool isNeedRightFallback = (handDetectionCount == 0);

            if (handDetectionCount > 0)
            {
                // Hand region bounding box update
                handCs.SetInt("_detectionCount", handDetectionCount);
                handCs.SetFloat("_regionDetectDt", Time.deltaTime);
                handCs
                    .SetBuffer(0,
                    "_palmDetections",
                    palmDetector.DetectionBuffer);
                handCs
                    .SetBuffer(0, "_handsRegionFromPalm", handsRegionFromPalm);
                handCs.Dispatch(0, 1, 1, 1);
            }

            handCs.SetVector("_spadScale", spadScale);
            handCs.SetInt("_isVerticalFlip", 1);
            for (int i = 0; i < handDetectionCount; i++)
            {
                handCs.SetInt("_handRegionIndex", i);

                // Hand region cropping
                handCs.SetInt("_handCropImageSize", handCropImageSize);
                handCs.SetTexture(2, "_handCropInput", inputTexture);
                handCs.SetBuffer(2, "_handCropRegion", handsRegionFromPalm);
                handCs.SetBuffer(2, "_handCropOutput", handCropBuffer);
                handCs
                    .Dispatch(2,
                    handCropImageSize / 8,
                    handCropImageSize / 8,
                    1);

                // Inference hand landmark.
                handLandmarkDetector.ProcessImage(handCropBuffer);

                // var result = new Vector4[handVertexCount + 1];
                // leftHandVertexBuffer.GetData(result);

                // for (int j = 0; j < handVertexCount; j++)
                // {
                //     UnityEngine.Debug.Log(j + ": " + result[j]);
                // }
                var scoreCache = new Vector4[1];
                handLandmarkDetector.OutputBuffer.GetData(scoreCache, 0, 0, 1);
                float score = scoreCache[0].x;

                // UnityEngine.Debug.Log("Score: " + score);
                float handedness = scoreCache[0].y;

                // UnityEngine.Debug.Log("handedness: " + scoreCache[0].y);
                bool isRight = handedness > 0.5f;
                if (score < 0.5f)
                {
                    if (isRight)
                        isNeedRightFallback = true;
                    else
                        isNeedLeftFallback = true;
                    continue;
                }

                // Key point postprocess
                handCs.SetFloat("_handPostDt", Time.deltaTime);
                handCs
                    .SetBuffer(3,
                    "_handPostInput",
                    handLandmarkDetector.OutputBuffer);
                handCs.SetBuffer(3, "_handPostRegion", handsRegionFromPalm);
                handCs
                    .SetBuffer(3,
                    "_handPostOutput",
                    isRight ? rightHandVertexBuffer : leftHandVertexBuffer);
                handCs
                    .SetBuffer(3,
                    "_handPostDeltaOutput",
                    isRight
                        ? deltaRightHandVertexBuffer
                        : deltaLeftHandVertexBuffer);
                handCs.Dispatch(3, 1, 1, 1);
            }

            // Hand Re-track with pose landmark if hand is not detected or landmark's score is too low.
            if (isNeedRightFallback) HandProcessFromPose(inputTexture, true);
            if (isNeedLeftFallback) HandProcessFromPose(inputTexture, false);
        }

        void HandProcessFromPose(Texture inputTexture, bool isRight)
        {
            // UnityEngine.Debug.Log("HandProcessFromPose");
            // Calculate hand region with pose landmark
            handCs.SetInt("_isRight", isRight ? 1 : 0);
            handCs.SetFloat("_bboxDt", Time.deltaTime);
            handCs.SetBuffer(1, "_poseInput", blazePoseDetecter.outputBuffer);
            handCs
                .SetBuffer(1,
                "_bboxRegion",
                isRight ? rightHandRegionFromPose : leftHandRegionFromPose);
            handCs.Dispatch(1, 1, 1, 1);

            var scale = new Vector2(1, 1);
            handCs.SetVector("_spadScale", scale);
            handCs.SetInt("_isVerticalFlip", 0);
            handCs.SetInt("_handRegionIndex", 0);

            // Hand region cropping
            handCs.SetInt("_handCropImageSize", handCropImageSize);
            handCs.SetTexture(2, "_handCropInput", inputTexture);
            handCs
                .SetBuffer(2,
                "_handCropRegion",
                isRight ? rightHandRegionFromPose : leftHandRegionFromPose);
            handCs.SetBuffer(2, "_handCropOutput", handCropBuffer);
            handCs.Dispatch(2, handCropImageSize / 8, handCropImageSize / 8, 1);

            // Hand landmark detection
            handLandmarkDetector.ProcessImage(handCropBuffer);

            // Key point postprocess
            handCs.SetFloat("_handPostDt", Time.deltaTime);
            handCs
                .SetBuffer(3,
                "_handPostInput",
                handLandmarkDetector.OutputBuffer);
            handCs
                .SetBuffer(3,
                "_handPostRegion",
                isRight ? rightHandRegionFromPose : leftHandRegionFromPose);
            handCs
                .SetBuffer(3,
                "_handPostOutput",
                isRight ? rightHandVertexBuffer : leftHandVertexBuffer);
            handCs
                .SetBuffer(3,
                "_handPostDeltaOutput",
                isRight
                    ? deltaRightHandVertexBuffer
                    : deltaLeftHandVertexBuffer);
            handCs.Dispatch(3, 1, 1, 1);
        }
        #endregion
    }
}
