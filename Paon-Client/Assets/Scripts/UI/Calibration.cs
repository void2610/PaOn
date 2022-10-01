using System;
using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using UnityEngine;

public class Calibration : MonoBehaviour
{
	public enum Phase
	{
		PoseEstimation,
		HandEstimation,
		Positioning,
	};

	public Phase state;

	public float minConfidence = 0.7f;

	[SerializeField]
	private Visualizer _visualizer;

	private Vector3[] left, right;

	[SerializeField]
	private GameObject GetKeypoints;

	private GetKeypoints gk;

	private GetKeypoints.Keypoint[] pose;

	// Start is called before the first frame update
	void Start()
	{
		gk = GetKeypoints.GetComponent<GetKeypoints>();
		pose = gk.pose;
		state = Phase.PoseEstimation;
	}

	// Update is called once per frame
	void Update()
	{
		pose = gk.pose;
		left = _visualizer.GetLeftVert();
		right = _visualizer.GetRightVert();

		if (pose[0] != null)
		{
			switch (state)
			{
				case Phase.PoseEstimation:
					Debug.Log("Phase1");
					if (pose[0].score > minConfidence && pose[15].score > minConfidence && pose[16].score > minConfidence)
					{
						state = Phase.Positioning;
					}
					return;
				case Phase.HandEstimation:
					Debug.Log("Phase2");
					return;
				case Phase.Positioning:
					Debug.Log("Phase3");
					return;
				default:
					Debug.Log("Calibration end");
					return;
			}
		}


		void InitPose()
		{

		}

	}
}
