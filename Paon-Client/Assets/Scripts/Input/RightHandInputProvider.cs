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
		}

		void Update()
		{
			if (gk.rightWrist.score > 0.7f)
			{
				wrist = gk.rightWrist;
			}

			hold = gk.rightIsClosed;

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

			// if (Input.GetKey(KeyCode.U))
			// {
			// 	hold = 1;
			// }
			// else
			// {
			// 	hold = 0;
			// }
			if (wrist != previous)
				previous = wrist;
		}

		// 	void LateUpdate()
		// 	{
		// 		if (gk.rightWrist.score > 0.7f)
		// 			wrist = gk.rightWrist;
		// 		// hand = gk.right;
		// 		// if (hand != null && previous[0] != null)
		// 		// {
		// 		//     //Vector2 delta = CalculateDelta(previous[0].coords, hand[0].coords);
		// 		//     float fd =
		// 		//         (float)Vector2.Distance(hand[4].coords, hand[12].coords);
		// 		//     if (fd < 40)
		// 		//     {
		// 		//         hold = 1;
		// 		//     }
		// 		//     else
		// 		//     {
		// 		//         hold = 0;
		// 		//     }
		// 		// }
		// 		hold = gk.rightIsClosed;
		// 		Debug.Log("right: " + hold);
		// 	}
	}
}
