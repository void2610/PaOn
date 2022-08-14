using System.Collections;
using System.Collections.Generic;
using System.IO;
using MediaPipe.HandPose;
// using OpenPose;
// using OpenPose.Example;
using UnityEngine;

namespace Paon.NInput
{
    public class GetKeypoints : MonoBehaviour
    {
        // private OPDatum datum;
        // public OpenPoseUserScript op;
        [SerializeField]
        private GameObject PoseEstimatior;

        [SerializeField]
        private GameObject HandEstimatior;

        private PoseEstimate _PoseEstimate;

        private Utils.Keypoint[] poseKeypoints;

        private HandVisualizer _handVisualizer;

        public enum KeyPointType : byte
        {
            pk = 0,
            lhk,
            rhk
        }

        public class Keypoint
        {
            public Vector2 coords;

            public float s;

            public Keypoint(float x, float y, float s)
            {
                this.coords.x = x;
                this.coords.y = y;
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
            _PoseEstimate = PoseEstimatior.GetComponent<PoseEstimate>();
            _handVisualizer = HandEstimatior.GetComponent<HandVisualizer>();
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
                    new Keypoint(key.position.x, key.position.y, key.score);

                // Debug.Log("pose[" + cnt + "]: " + pose[cnt].coords);
                cnt++;
            }

            for (int i = 0; i < left.Length; i++)
            {
                left[i].coords =
                    new Vector2(_handVisualizer.KeyPoint[i].x,
                        _handVisualizer.KeyPoint[i].y);
            }
        }
    }
}
