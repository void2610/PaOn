using System;
using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using Paon.NPlayer;
using UnityEngine;

public class comocmo : MonoBehaviour
{
	[SerializeField]
	Visualizer visualizer;

	[SerializeField]
	GetKeypoints gk;

	[SerializeField]
	GetKeypoints.Keypoint left, right, nose;

	public float threshhold = 70f;

	public bool Open = false;

	bool check = false;

	float delta = 0;

	void Start()
	{

	}

	void Update()
	{
		left = gk.leftWrist;
		right = gk.rightWrist;
		delta = Mathf.Abs(left.coords.y - right.coords.y);
		if (delta > threshhold && !check) check = true;
		if (check) StartCoroutine(nameof(CommoRose));

	}

	private IEnumerator CommoRose()
	{
		yield return new WaitForSeconds(0.5f);
		if (delta > threshhold)
		{
			check = false;
			Open = true;
		}
	}
}
