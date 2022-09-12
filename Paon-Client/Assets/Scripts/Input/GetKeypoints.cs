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

        private PoseEstimate _PoseEstimate;

        private Utils.Keypoint[] poseKeypoints;

        // private OPDatum datum;
        // public OpenPoseUserScript op;
        private Visualizer _handVisuallizer;

        public enum KeyPointType : byte
        {
            pk = 0,
            lhk,
            rhk
        }

        public class Keypoint
        {
            public Vector3 coords;

            public float s;

            public Keypoint(float x, float y, float z, float s)
            {
                this.coords.x = x;
                this.coords.y = y;
                this.coords.z = z;
                this.s = s;
            }
        }

        public Keypoint[] pose = new Keypoint[25];

        public Keypoint[] right = new Keypoint[21];

        public Keypoint[] left = new Keypoint[21];

        // public void LoadDatum(int length, KeyPointType type)
        // {
        //     int index = 0;
        //     try
        //     {
        //         for (int i = 0; i < length * 3; i += 3)
        //         {
        //             if (type == KeyPointType.pk)
        //             {
        //                 pose[index] =
        //                     new Keypoint(datum.poseKeypoints[i],
        //                         datum.poseKeypoints[i + 1],
        //                         datum.poseKeypoints[i + 2]);
        //                 index++;
        //             }
        //             else if (type == KeyPointType.lhk)
        //             {
        //                 left[index] =
        //                     new Keypoint(datum.handKeypoints[0][i],
        //                         datum.handKeypoints[0][i + 1],
        //                         datum.handKeypoints[0][i + 2]);
        //                 index++;
        //             }
        //             else if (type == KeyPointType.rhk)
        //             {
        //                 right[index] =
        //                     new Keypoint(datum.handKeypoints[1][i],
        //                         datum.handKeypoints[1][i + 1],
        //                         datum.handKeypoints[1][i + 2]);
        //                 index++;
        //             }
        //         }
        //     }
        //     catch (System.NullReferenceException e)
        //     {
        //     }
        // }
        void Start()
        {
            // op = GameObject.Find("OpenPose").GetComponent<OpenPoseUserScript>();
            _PoseEstimate = PoseEstimator.GetComponent<PoseEstimate>();
            _handVisuallizer = HandEstimatior.GetComponent<Visualizer>();
        }

        void LateUpdate()
        {
            // datum = op.datum;
            // LoadDatum(25, KeyPointType.pk);
            // LoadDatum(17, KeyPointType.rhk);
            // LoadDatum(17, KeyPointType.lhk);
            //Debug.Log(datum.poseKeypoints[0]);
            poseKeypoints = _PoseEstimate.GetKeypoints();
            int cnt = 0;
            foreach (Utils.Keypoint key in poseKeypoints)
            {
                pose[cnt] =
                    new Keypoint(key.position.x, key.position.y, 0, key.score);

                // Debug.Log("pose[" + cnt + "]: " + pose[cnt].coords);
                cnt++;
            }

            // Vector3[] pos = _handVisualizer.GetKey();
            Vector3[] pos = new Vector3[21];

            //Debug.Log(pos[20]);
            for (int i = 0; i < left.Length; i++)
            {
                left[i] = new Keypoint(pos[i].x, pos[i].y, pos[i].z, 0);
            }
        }
    }
}
