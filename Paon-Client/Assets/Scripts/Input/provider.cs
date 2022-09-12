using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class provider : MonoBehaviour
{
    [SerializeField]
    private GameObject Estimatior;

    private PoseEstimate _PoseEstimate;

    private Utils.Keypoint[] pose;

    // Start is called before the first frame update
    void Start()
    {
        _PoseEstimate = Estimatior.GetComponent<PoseEstimate>();
    }

    // Update is called once per frame
    void Update()
    {
        pose = _PoseEstimate.GetKeypoints();
        foreach (Utils.Keypoint key in pose)
        {
            Debug.Log(key.position);
        }
    }
}
