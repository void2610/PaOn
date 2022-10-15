using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paon.NInput;
using UnityEngine;
using UnityEngine.SceneManagement;
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

	public bool end = false;

	[SerializeField]
	RawImage[] Lamps;

	Color green = new Color(0, 210f / 255f, 50f / 255f);

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
	Text message;

	private string loadText = "";
	private string[] splitText;

	private int textIndex = 0;
	// Start is called before the first frame update
	void Start()
	{
		pose = gk.pose;
		state = Phase.PoseEstimation;
		loadText = (Resources.Load("CalibrationResources/CalibrationMessage", typeof(TextAsset)) as TextAsset).text;
		splitText = loadText.Split(char.Parse("\n"));
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace)) DeleteThreshold();
		if (Input.GetKeyDown(KeyCode.Escape)) end = true;
		if (end) SceneManager.LoadScene("MainMenu");
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
					message.text = splitText[0];
					if (pose[0].score > minConfidence && pose[15].score > minConfidence && pose[16].score > minConfidence)
					{
						state = Phase.HandEstimation;
						Lamps[0].color = green;
					}
					return;

				case Phase.HandEstimation:
					Debug.Log("Phase2");
					if (!isRunning)
						message.text = splitText[1];
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
					message.text = splitText[4];
					end = true;
					Debug.Log("Calibration end");
					return;
			}

		}

	}

	IEnumerator DecideCloseThreshold()
	{
		yield return new WaitForSeconds(3);
		message.text = splitText[2];
		float[] buffer = new float[200];
		float delta, result;
		bool go = false;
		while (leftScore < 0.7f && rightScore < 0.7f) yield return null;
		for (int i = 0; i < 150; i++)
		{
			if (leftScore > 0.7f && rightScore > 0.7f)
			{
				delta = gk.GetDistance();
				buffer[i] = delta;
			}
			else i--;
			yield return null;
			if (i == 149) go = true;
		}
		if (go)
		{
			result = buffer.Average();
			result += result * 0.1f;
			gk.closeThreshold = result;
			PlayerPrefs.SetFloat("CloseThreshold", result);
			Debug.Log("CloseThreshold is determined");
			isRunning = false;
			state = Phase.Positioning;
			Lamps[1].color = green;
		}
	}

	IEnumerator DecideWalkThreshold()
	{
		message.text = splitText[3];
		yield return new WaitForSeconds(3);
		float[] buffer = new float[200];
		float delta, result;
		for (int i = 0; i < 150; i++)
		{
			delta = mo.GetDelta();
			buffer[i] = delta;
			yield return null;
		}
		result = buffer.Average();
		result += result * 0.15f;
		mo.forwardThreshold = result;
		PlayerPrefs.SetFloat("WalkThreshold", result);
		Debug.Log("WalkThreshold is determined");
		isRunning = false;
		state = Phase.End;
		Lamps[2].color = green;
	}

	IEnumerator DecideRotateThreshold()
	{
		yield return new WaitForSeconds(3);
		float[] buffer = new float[60];
		float delta, result;
		for (int i = 0; i < 30; i++)
		{
			delta = mo.GetDelta();
			buffer[i] = delta;
			yield return null;
		}
	}

	IEnumerator Decide() { yield return null; }

	void OnDestroy()
	{
		PlayerPrefs.Save();
	}

	void DeleteThreshold()
	{
		PlayerPrefs.DeleteKey("CloseThreshold");
		PlayerPrefs.DeleteKey("WalkThreshold");
		Debug.Log("Threshold deleted");
	}
}
