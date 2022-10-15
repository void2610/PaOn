using System;
using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using Paon.NPlayer;
using UnityEngine;

public class OpenCommorose : MonoBehaviour
{
	[SerializeField]
	Visualizer visualizer;

	[SerializeField]
	GetKeypoints gk;

	[SerializeField]
	GetKeypoints.Keypoint left, right, nose;

	public float threshhold = 70f;

	public bool isBordering = false;
	public bool Open = false;

	public GameObject leftHand, rightHand;
	public bool isDebugEnabled = false;
	private DebugManager debugger;
	bool check = false;

	float delta = 0;

	void Start()
	{
		debugger = GameObject.Find("DebugManager").GetComponent<DebugManager>();
		leftHand = GameObject.Find("LeftHand");
		rightHand = GameObject.Find("RightHand");
	}

	void Update()
	{
		isDebugEnabled = debugger.isDebugEnabled;
		if (!isBordering && !isDebugEnabled)
		{
			left = gk.leftWrist;
			right = gk.rightWrist;
			delta = Mathf.Abs(left.coords.y - right.coords.y);
			if (delta > threshhold && !check) check = true;
			if (check) StartCoroutine(nameof(CommoRose));
		}

		if (isDebugEnabled)
		{
			delta = Mathf.Abs(leftHand.transform.localPosition.y) + Mathf.Abs(rightHand.transform.localPosition.y);
			if (delta >= 1.5f && check) check = true;
			if (check) StartCoroutine(nameof(CommoRose));
		}
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
