using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NInput
{
	public class RightHandInputProvider : MonoBehaviour
	{
		GameObject GK;

		private GetKeypoints gk;

		private GetKeypoints.Keypoint previous = new GetKeypoints.Keypoint(0, 0, 0, 0);
		private GetKeypoints.Keypoint wrist;

		private GetKeypoints.Keypoint[] hand;

		string key = "";

		private bool isDebugEnabled = false;

		private DebugManager debugger;

		int hold = 0;

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
		public Vector2 GetPosition()
		{
			try
			{
				return wrist.coords;
			}
			catch (System.NullReferenceException)
			{
				return new Vector2(0, 0);
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

		///<summary>
		///手が閉じているか確認するメソッド
		///</summary>
		/// <returns>閉じていたら1</returns>
		public int CheckHold()
		{
			return hold;
		}

		private Vector2 CalculateDelta(Vector2 pre, Vector2 now)
		{
			float dx = pre.x - now.x;
			float dy = pre.y - now.y;
			return new Vector2(dx, dy);
		}

		void Start()
		{
			GK = GameObject.Find("GetKeypoints");
			gk = GK.GetComponent<GetKeypoints>();
			debugger = GameObject.Find("DebugManager").GetComponent<DebugManager>();
		}

		void Update()
		{
			isDebugEnabled = debugger.isDebugEnabled;
			if (gk.rightWrist.score > 0.7f)
			{
				wrist = gk.rightWrist;
			}

			hold = gk.rightIsClosed;

			isDebugEnabled = debugger.isDebugEnabled;
			if (isDebugEnabled)
			{
				if (Input.GetKey(KeyCode.I))
				{
					key = "up";
				}
				else if (Input.GetKey(KeyCode.J))
				{
					key = "left";
				}
				else if (Input.GetKey(KeyCode.K))
				{
					key = "down";
				}
				else if (Input.GetKey(KeyCode.L))
				{
					key = "right";
				}
				else
				{
					key = "none";
				}

				if (Input.GetKey(KeyCode.U))
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
