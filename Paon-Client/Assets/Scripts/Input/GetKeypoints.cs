using System.Collections;
using System.Collections.Generic;
using System.IO;
using MediaPipe.Holistic;
using UnityEngine;

namespace Paon.NInput
{
    public class GetKeypoints : MonoBehaviour
    {
        //aaa
        // private OPDatum datum;
        // public OpenPoseUserScript op;
        [SerializeField]
        private GameObject HandEstimatior;

        [SerializeField]
        private GameObject PoseEstimator;

        private PoseEstimator _PoseEstimator;

        private Utils.Keypoint[] poseKeypoints;

        // private OPDatum datum;
        // public OpenPoseUserScript op;
        private Visualizer _handVisualizer;

        public enum KeyPointType : byte
        {
            pk = 0,
            lhk,
            rhk
        }

        public class Keypoint
        {
            public Vector3 coords;

            public float score;

            public Keypoint(float x, float y, float z, float score)
            {
                this.coords.x = x;
                this.coords.y = y;
                this.coords.z = z;
                this.score = score;
            }
        }

        public Keypoint[] pose = new Keypoint[25];

        public Keypoint[] right = new Keypoint[21];

        public Keypoint[] left = new Keypoint[21];

        public Keypoint leftWrist = new Keypoint(0, 0, 0, 0);

        public Keypoint rightWrist = new Keypoint(0, 0, 0, 0);

        void Start()
        {
            // op = GameObject.Find("OpenPose").GetComponent<OpenPoseUserScript>();
            _PoseEstimator = PoseEstimator.GetComponent<PoseEstimator>();
            _handVisualizer = HandEstimatior.GetComponent<Visualizer>();
        }

        void LateUpdate()
        {
            poseKeypoints = _PoseEstimator.GetKeypoints();
            int cnt = 0;
            foreach (Utils.Keypoint key in poseKeypoints)
            {
                pose[cnt] =
                    new Keypoint(key.position.x, key.position.y, 0, key.score);

                // Debug.Log("pose[" + cnt + "]: " + pose[cnt].coords);
                cnt++;
            }

            leftWrist = pose[9];

            rightWrist = pose[10];

            Vector3[] leftTemp = _handVisualizer.GetLeftVert();
            Vector3[] rightTemp = _handVisualizer.GetRightVert();

            for (int i = 0; i < left.Length; i++)
            {
                left[i] =
                   new Keypoint(leftTemp[i].x,
                       leftTemp[i].y,
                       leftTemp[i].z,
                       0);
            }

            Debug.Log("l0: " + left[0].coords);
            Debug.Log("l12: " + left[12].coords);

            for (int i = 0; i < right.Length; i++)
            {
                right[i] =
                    new Keypoint(rightTemp[i].x,
                        rightTemp[i].y,
                        rightTemp[i].z,
                        0);
            }
        }
    }
}
