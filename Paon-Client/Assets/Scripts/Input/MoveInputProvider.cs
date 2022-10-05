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

		public bool isDebugEnabled = false;

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
			if (PlayerPrefs.HasKey("WalkThreshold")) forwardThreshold = PlayerPrefs.GetKey("WalkThreshold");
		}

		void Update()
		{
			vertices = _visualizer.GetPoseVertices();
			previous = gk.pose;

			if (Input.GetKey(KeyCode.Return)) isDebugEnabled = !isDebugEnabled;

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
				else if (Input.GetKey(KeyCode.UpArrow) || crouch == 1)
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

			if (pose != null && previous[16].score > 0.7f && previous[15].score > 0.7f)
			{
				Vector2 healCenter = Vector2.Lerp(pose[15].coords, pose[16].coords, 0.5f);
				Vector2 hipCenter = Vector2.Lerp(pose[11].coords, pose[12].coords, 0.5f);

				//left
				def2 = pose[15].coords.y - pose[0].coords.y;

				// Debug.Log("delta1: " + Math.Abs(def1 - predef1));
				// Debug.Log("delta2: " + Math.Abs(def2 - predef2));
				if (
								Mathf.Abs(def1 - predef1) > th ||
								Mathf.Abs(def2 - predef2) > th
				)
				{
					key = "up";
				}
				else
				{
					key = "none";
				}

				//ankle to hip
				// Rleg = Mathf.Abs(pose[16].coords.x - pose[12].coords.x);
				// Lleg = Mathf.Abs(pose[15].coords.x - pose[11].coords.x);
				Rleg = Mathf.Abs(vertices[30].x - vertices[32].x);
				Lleg = Mathf.Abs(vertices[29].x - vertices[31].x);

				// Debug.Log(Rleg);
				if (Rleg > 0.07)
				{
					key = "right";
				}
				else if (Lleg > 0.07)
				{
					key = "left";
				}
				else
				{
					key = "none";
				}

				predef1 = def1;
				predef2 = def2;

				float current = Mathf.Abs(pose[16].coords.y - pose[15].coords.y);

				delta = Mathf.Abs(prevForward - current);
				if (delta > forwardThreshold && !isWalking)
				{
					isWalking = true;
				}

				if (isWalking)
				{
					key = "up";
					StartCoroutine(nameof(JudgeMove));
				}
			}
		}

		void LateUpdate()
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

		public float GetDelta()
		{
			return delta;
		}
	}
}
