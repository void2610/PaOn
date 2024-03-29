using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NInput
{
	public class MoveInputProvider : MonoBehaviour
	{
		public GameObject GK;

		private GetKeypoints gk;

		private GetKeypoints.Keypoint[] previous;

		private GetKeypoints.Keypoint[] pose;

		public Visualizer _visualizer;

		Vector3[] vertices;

		string key = "";

		public int crouch = 0;

		float def1;

		float def2;

		float predef1;

		float predef2;

		float Rleg;

		float Lleg;

		float prevForward;

		public float forwardThreshold = 15f;

		public float th = 1.0f;

		private bool isDebugEnabled = false;
		private DebugManager debugger;

		private float delta = 0;

		private bool isWalking = false;
		///<summary>
		///入力されているキーを返すメソッド
		///</summary>
		/// <returns>入力されているキー</returns>
		public string GetInput()
		{
			return key;
		}

		Vector2 CalculateDelta(Vector3 pre, Vector3 current)
		{
			float dx = pre.x - current.x;
			float dy = pre.y - current.y;
			return new Vector2(dx, dy);
		}

		IEnumerator JudgeMove()
		{
			if (delta < forwardThreshold)
			{
				yield return new WaitForSeconds(0.5f);
				if (delta < forwardThreshold)
				{
					isWalking = false;
					yield break;
				}
			}
		}


		void Start()
		{
			gk = GK.GetComponent<GetKeypoints>();
			debugger = GameObject.Find("DebugManager").GetComponent<DebugManager>();
			if (PlayerPrefs.HasKey("WalkThreshold")) forwardThreshold = PlayerPrefs.GetFloat("WalkThreshold");
		}

		void Update()
		{
			isDebugEnabled = debugger.isDebugEnabled;
			if (isDebugEnabled)
			{
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					key = "left";
				}
				else if (Input.GetKey(KeyCode.RightArrow))
				{
					key = "right";
				}
				else if (Input.GetKey(KeyCode.UpArrow))
				{
					key = "up";
				}
				else if (Input.GetKey(KeyCode.DownArrow))
				{
					key = "down";
				}
				else if (Input.GetKey(KeyCode.Space))
				{
					key = "space";
				}
				else
				{
					key = "none";
				}
			}
			else
			{
				vertices = _visualizer.GetPoseVertices();
				previous = gk.pose;
				if (pose != null && previous[16].score > 0.7f && previous[15].score > 0.7f)
				{
					Vector2 healCenter = Vector2.Lerp(pose[15].coords, pose[16].coords, 0.5f);
					Vector2 hipCenter = Vector2.Lerp(pose[11].coords, pose[12].coords, 0.5f);

					//right
					def1 = pose[16].coords.y - pose[0].coords.y;

					//left
					def2 = pose[15].coords.y - pose[0].coords.y;

					// Debug.Log("delta1: " + Math.Abs(def1 - predef1));
					// Debug.Log("delta2: " + Math.Abs(def2 - predef2));

					//heel to toe
					Rleg = Mathf.Abs(Mathf.Abs(vertices[30].x) - Mathf.Abs(vertices[32].x));
					Lleg = Mathf.Abs(Mathf.Abs(vertices[29].x) - Mathf.Abs(vertices[31].x));
					// Debug.Log("Rleg: " + Rleg);
					// Debug.Log("Lleg: " + Lleg);

					if (Rleg > 0.07f)
					{
						key = "right";
					}
					else if (Lleg > 0.07f)
					{
						key = "left";
					}
					else
					{
						key = "none";
					}


					float current = Mathf.Abs(pose[16].coords.y - pose[15].coords.y);

					// delta = Mathf.Abs(prevForward - current);
					delta = Mathf.Abs(current);
					if (delta > forwardThreshold && !isWalking)
					{
						isWalking = true;
					}

					if (isWalking && crouch == 0)
					{
						key = "up";
						StartCoroutine(nameof(JudgeMove));
					}

					predef1 = def1;
					predef2 = def2;
					// prevForward = current;
				}
			}
		}

		void LateUpdate()
		{
			if (!isDebugEnabled)
			{
				pose = gk.pose;
				if (pose != null && previous[16].score > 0.7f && previous[15].score > 0.7f)
				{
					Vector2 healCenter = Vector2.Lerp(pose[15].coords, pose[16].coords, 0.5f);
					Vector2 hipCenter = Vector2.Lerp(pose[11].coords, pose[12].coords, 0.5f);

					float legLength = Vector2.Distance(healCenter, hipCenter);
					if (legLength < 50)
					{
						crouch = 1;
						Debug.Log("crouched: " + crouch);
					}
					else
					{
						crouch = 0;
					}
				}
			}

		}

		public float GetDelta()
		{
			return delta;
		}
	}
}
