using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour
{
	enum phase
	{
		Positioning,
		HandEstimation,
	};

	[SerializeField]
	private Visualizer _visualizer;

	[SerializeField]
	private GetKeypoints gk;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
