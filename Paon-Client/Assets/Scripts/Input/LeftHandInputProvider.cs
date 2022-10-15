using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NInput
{
	public class LeftHandInputProvider : MonoBehaviour
	{
		[Range(0.0f, 1.0f)]
		public float holdThreshold = 0.1f;
		GameObject GK;

		private GetKeypoints gk;

		private GetKeypoints.Keypoint previous = new GetKeypoints.Keypoint(0, 0, 0, 0);

		private GetKeypoints.Keypoint[] hand;

		private GetKeypoints.Keypoint wrist;
		private Vector3 finger;
		string key = "";

		public bool isDebugEnabled = false;

		int hold = 0;

		private float CalculateDistance(Vector2 start, Vector2 end)
		{
			return (float)Vector2.Distance(start, end);
		}

		private Vector2 CalculateDelta(Vector2 pre, Vector2 now)
		{
			float dx = pre.x - now.x;
			float dy = pre.y - now.y;
			return new Vector2(dx, dy);
		}

		///<summary>
		///入力されているキーを返すメソッド
		///</summary>
		/// <returns>入力されているキー</returns>
		public string GetInput()
		{
			return key;
		}

		///<summary>
		///推定された手の座標を返すメソッド
		///</summary>
		/// <returns>手の座標</returns>
		public Vector3 GetPosition()
		{
			try
			{
				return wrist.coords;
			}
			catch (System.NullReferenceException)
			{
				return new Vector3(0, 0);
			}
		}

		public Vector2 GetDelta()
		{
			try
			{
				return CalculateDelta(previous.coords, wrist.coords);
			}
			catch (System.NullReferenceException)
			{
				return new Vector2(0, 0);
			}
		}

		public int CheckHold()
		{
			return hold;
		}

		void Start()
		{
			GK = GameObject.Find("GetKeypoints");
			gk = GK.GetComponent<GetKeypoints>();
		}

		void Update()
		{
			if (gk.leftWrist.score > 0.7f)
			{
				wrist = gk.leftWrist;
				// Debug.Log("wrist: " + wrist.coords);
				// if (previous != null) Debug.Log("previous: " + previous.coords);
			}

			hold = gk.leftIsClosed;
			// Debug.Log("left: " + hold);

			if (Input.GetKey(KeyCode.Return)) isDebugEnabled = !isDebugEnabled;

			if (isDebugEnabled)
			{
				if (Input.GetKey(KeyCode.W))
				{
					key = "up";
				}
				else if (Input.GetKey(KeyCode.A))
				{
					key = "left";
				}
				else if (Input.GetKey(KeyCode.S))
				{
					key = "down";
				}
				else if (Input.GetKey(KeyCode.D))
				{
					key = "right";
				}
				else
				{
					key = "none";
				}

				if (Input.GetKey(KeyCode.Q))
				{
					hold = 1;
				}
				else
				{
					hold = 0;
				}
			}
			else
			{
				if (wrist != previous)
					previous = wrist;
			}
		}
	}
}
