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

		string key = "";

		int crouch = 0;

		float def1;

		float def2;

		float predef1;

		float predef2;

		float Rleg;

		float Lleg;

		public int th = 30;

		///<summary>
		///入力されているキーを返すメソッド
		///</summary>
		/// <returns>入力されているキー</returns>
		public string GetInput()
		{
			return key;
		}

		private Vector2 CalculateDelta(Vector2 pre, Vector2 now)
		{
			float dx = pre.x - now.x;
			float dy = pre.y - now.y;

			//Debug.Log(dx);
			return new Vector2(dx, dy);
		}

		void Start()
		{
			gk = GK.GetComponent<GetKeypoints>();
		}

		void Update()
		{
			previous = gk.pose;
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
			// else
			// {
			// 	key = "none";
			// }
		}

		void LateUpdate()
		{
			pose = gk.pose;
			if (pose != null && previous[16].score > 0.7f && previous[15].score > 0.7f)
			{
				//right ankle to nose
				def1 = pose[16].coords.y - pose[0].coords.y;
				//left
				def2 = pose[15].coords.y - pose[0].coords.y;

				if (Mathf.Abs(def1 - predef1) > th || Mathf.Abs(def2 - predef2) > th)
				{
					key = "up";
				}
				//ankle to hip
				Rleg = Mathf.Abs(pose[16].coords.x - pose[12].coords.x);
				Lleg = Mathf.Abs(pose[15].coords.x - pose[11].coords.x);
				// Debug.Log(Rleg);
				if (Rleg > 15)
				{
					key = "right";
				}
				else if (Lleg > 15)
				{
					key = "left";
				}

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

				predef1 = def1;
				predef2 = def2;
			}
		}
	}
}
