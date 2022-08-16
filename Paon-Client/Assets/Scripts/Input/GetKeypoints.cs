using System.Collections;
using System.Collections.Generic;
using System.IO;
using OpenPose;
using OpenPose.Example;
using UnityEngine;

namespace Paon.NInput
{
    public class GetKeypoints : MonoBehaviour
    {
        //aaa
        // private OPDatum datum;
        // public OpenPoseUserScript op;
        [SerializeField]
        private GameObject PoseEstimatior;

        private OPDatum datum;

        public OpenPoseUserScript op;

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

        public Keypoint[] right = new Keypoint[17];

        public Keypoint[] left = new Keypoint[17];

        public void LoadDatum(int length, KeyPointType type)
        {
            int index = 0;
            try
            {


                for (int i = 0; i < length * 3; i += 3)
                {
                    if (type == KeyPointType.pk)
                    {
                        pose[index] =
                            new Keypoint(datum.poseKeypoints[i],
                                datum.poseKeypoints[i + 1],
                                datum.poseKeypoints[i + 2]);
                        index++;
                    }
                    else if (type == KeyPointType.lhk)
                    {
                        left[index] =
                            new Keypoint(datum.handKeypoints[0][i],
                                datum.handKeypoints[0][i + 1],
                                datum.handKeypoints[0][i + 2]);
                        index++;
                    }
                    else if (type == KeyPointType.rhk)
                    {
                        right[index] =
                            new Keypoint(datum.handKeypoints[1][i],
                                datum.handKeypoints[1][i + 1],
                                datum.handKeypoints[1][i + 2]);
                        index++;
                    }
                }
            }
            catch (System.NullReferenceException e) {
                
            }
        }

        void Start()
        {
            op = GameObject.Find("OpenPose").GetComponent<OpenPoseUserScript>();
        }

        void LateUpdate()
        {
            datum = op.datum;
            LoadDatum(25, KeyPointType.pk);
            LoadDatum(17, KeyPointType.rhk);
            LoadDatum(17, KeyPointType.lhk);
            //Debug.Log(datum.poseKeypoints[0]);
        }
    }
}
