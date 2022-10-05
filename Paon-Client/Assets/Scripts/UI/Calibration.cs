using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	[SerializeField]
	GetKeypoints gk;

	[SerializeField]
	MoveInputProvider mo;

	private Vector3[] left, right;

	[SerializeField]
	private GameObject GetKeypoints;
	private GetKeypoints.Keypoint[] pose;

	private bool isRunning = false;

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
					if (!isRunning)
					{
						StartCoroutine(nameof(DecideCloseThreshold));
						isRunning = true;
					}
					return;

				case Phase.Positioning:
					Debug.Log("Phase3");
					if (!isRunning)
					{
						StartCoroutine(nameof(DecideWalkThreshold));
						isRunning = true;
					}
					return;

				default:
					Debug.Log("Calibration end");
					return;
			}
		}
	}

	IEnumerator DecideCloseThreshold()
	{
		float[] buffer = new float[60];
		float delta = 99;
		float result = 0;
		for (int i = 0; i < 30; i++)
		{
			delta = gk.GetDistance();
			buffer[i] = delta;
			yield return null;
		}
		result = buffer.Average();
		result = result - result * 0.1f;
		gk.closeThreshold = result;
		PlayerPrefs.SetFloat("CloseThreshold", result);
		Debug.Log("CloseThreshold is determined");
		isRunning = false;
	}

	IEnumerator DecideWalkThreshold()
	{
		float[] buffer = new float[60];
		float delta = 99;
		float result = 0;
		for (int i = 0; i < 30; i++)
		{
			delta = mo.GetDelta();
			buffer[i] = delta;
			yield return null;
		}
		result = buffer.Average();
		result = result - result * 0.1f;
		mo.forwardThreshold = result;
		PlayerPrefs.SetFloat("ForwardThreshold", result);
		Debug.Log("WalkThreshold is determined");
		isRunning = false;
	}

	IEnumerator DecideRotateThreshold()
	{
		float[] buffer = new float[60];
		float delta = 99;
		float result = 0;
		for (int i = 0; i < 30; i++)
		{
			delta = mo.GetDelta();
			yield return null;
		}
	}

	IEnumerator Decide() { yield return null; }

	void OnDestroy()
	{
		PlayerPrefs.Save();
	}
}
