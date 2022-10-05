using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paon.NInput;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
	public enum Phase
	{
		PoseEstimation,
		HandEstimation,
		Positioning,
		End,
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

	private float leftScore, rightScore;
	private GetKeypoints.Keypoint[] pose;
	private bool isRunning = false;

	[SerializeField]
	RawImage image;

	[SerializeField]
	WebCamInput webCamInput;

	[SerializeField]
	Text textObject;

	private string loadText = "";
	private string[] splitText;

	private int textIndex = 0;
	// Start is called before the first frame update
	void Start()
	{
		pose = gk.pose;
		state = Phase.PoseEstimation;
		loadText = (Resources.Load("CalibrationMessage", typeof(TextAsset)) as TextAsset).text;
		splitText = loadText.Split(char.Parse("\n"));
	}

	// Update is called once per frame
	void Update()
	{
		pose = gk.pose;
		left = _visualizer.GetLeftVert();
		right = _visualizer.GetRightVert();

		image.texture = webCamInput.inputImageTexture;

		leftScore = gk.GetScore(GetKeypoints.LeftOrRight.left);
		rightScore = gk.GetScore(GetKeypoints.LeftOrRight.right);

		if (pose[0] != null)
		{
			switch (state)
			{
				case Phase.PoseEstimation:
					Debug.Log("Phase1");
					if (pose[0].score > minConfidence && pose[15].score > minConfidence && pose[16].score > minConfidence)
					{
						state = Phase.HandEstimation;
					}
					return;

				case Phase.HandEstimation:
					Debug.Log("Phase2");
					if (!isRunning && leftScore > 0.7f && rightScore > 0.7f)
					{
						isRunning = true;
						StartCoroutine(nameof(DecideCloseThreshold));
					}
					return;

				case Phase.Positioning:
					Debug.Log("Phase3");
					if (!isRunning)
					{
						isRunning = true;
						StartCoroutine(nameof(DecideWalkThreshold));
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
		yield return new WaitForSeconds(5);
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
		state = Phase.Positioning;

	}

	IEnumerator DecideWalkThreshold()
	{
		yield return new WaitForSeconds(5);
		float[] buffer = new float[60];
		float delta = 99;
		float result = 0;
		for (int i = 0; i < 150; i++)
		{
			delta = mo.GetDelta();
			buffer[i] = delta;
			yield return null;
		}
		result = buffer.Average();
		result = result - result * 0.1f;
		mo.forwardThreshold = result;
		PlayerPrefs.SetFloat("WalkThreshold", result);
		Debug.Log("WalkThreshold is determined");
		isRunning = false;
		state = Phase.End;
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
