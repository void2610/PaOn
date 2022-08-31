using UnityEngine;

namespace MediaPipe.HandPose
{
    [
        CreateAssetMenu(
            fileName = "HandPose",
            menuName = "ScriptableObjects/HandPose Resource")
    ]
    public class HandPoseResource : ScriptableObject
    {
        public MediaPipe.BlazePalm.ResourceSet blazePalmResource;

        public MediaPipe.HandLandmark.ResourceSet handLandmarkResource;

        public ComputeShader commonCS;

        public ComputeShader handCS;
    }
}
