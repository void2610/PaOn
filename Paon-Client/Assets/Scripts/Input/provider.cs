using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provider : MonoBehaviour
{
    [SerializeField]
    private GameObject Estimatior;

    private PoseEstimator _PoseEstimator;

    private Utils.Keypoint[] pose;

    void Start()
    {
        _PoseEstimator = Estimatior.GetComponent<PoseEstimator>();
    }

    void Update()
    {
        pose = _PoseEstimator.GetKeypoints();
        foreach (Utils.Keypoint key in pose)
        {
            Debug.Log(key.position);
        }
    }
}
