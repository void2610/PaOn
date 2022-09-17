using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class provider : MonoBehaviour
{
    [SerializeField]
    private GameObject Estimatior;

    private PoseEstimator _PoseEstimator;

    private Utils.Keypoint[] pose;

    // Start is called before the first frame update
    void Start()
    {
        _PoseEstimator = Estimatior.GetComponent<PoseEstimator>();
    }

    // Update is called once per frame
    void Update()
    {
        pose = _PoseEstimator.GetKeypoints();
        foreach (Utils.Keypoint key in pose)
        {
            Debug.Log(key.position);
        }
    }
}
