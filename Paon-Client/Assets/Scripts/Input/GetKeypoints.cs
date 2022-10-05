using System.Collections;
using System.Collections.Generic;
using System.IO;
using MediaPipe.Holistic;
using UnityEngine;

namespace Paon.NInput
{
	public class GetKeypoints : MonoBehaviour
	{
		[SerializeField]
		private GameObject HandEstimatior;

		[SerializeField]
		private GameObject PoseEstimator;

		private PoseEstimator _PoseEstimator;

		private Utils.Keypoint[] poseKeypoints;

		private Visualizer _handVisualizer;

		public enum KeyPointType : byte
		{
			pk = 0,
			lhk,
			rhk
		}

		public class Keypoint
		{
			public Vector3 coords;
			public float score;
			public Keypoint(float x, float y, float z, float score)
			{
				this.coords.x = x;
				this.coords.y = y;
				this.coords.z = z;
				this.score = score;
			}
		}

		public Keypoint[] pose = new Keypoint[25];

		public Keypoint leftWrist = new Keypoint(0, 0, 0, 0);

		public Keypoint rightWrist = new Keypoint(0, 0, 0, 0);

		public float closeThreshold = 0.05f;

		public int leftIsClosed = 0;
		public int rightIsClosed = 0;

		Queue<int> leftQueue = new Queue<int>();
		Queue<int> rightQueue = new Queue<int>();

		float distance = 999;

		void Start()
		{
			_PoseEstimator = PoseEstimator.GetComponent<PoseEstimator>();
			_handVisualizer = HandEstimatior.GetComponent<Visualizer>();

			if (PlayerPrefs.HasKey("CloseThreshold")) closeThreshold = PlayerPrefs.GetFloat("CloseThreshold");
		}

		void Update()
		{
			poseKeypoints = _PoseEstimator.GetKeypoints();
			int cnt = 0;
			foreach (Utils.Keypoint key in poseKeypoints)
			{
				pose[cnt] = new Keypoint(key.position.x, key.position.y, 0, key.score);
				// Debug.Log("pose[" + cnt + "]: " + pose[cnt].coords);
				cnt++;
			}

			leftWrist = pose[9];
			rightWrist = pose[10];

			Vector3[] leftTemp = _handVisualizer.GetLeftVert();
			Vector3[] rightTemp = _handVisualizer.GetRightVert();

			float leftScore = leftTemp[21].x;
			float rightScore = rightTemp[21].x;
			Debug.Log("leftScore : " + leftScore);
			Debug.Log("rightScore : " + rightScore);

			leftWrist.coords.z = leftTemp[0].z;
			rightWrist.coords.z = rightTemp[0].z;

			// leftIsClosed = leftCloseOrOpen(leftTemp);
			// rightIsClosed = rightCloseOrOpen(rightTemp);
			// Debug.Log("Left: " + leftIsClosed);
			// Debug.Log("Right: " + rightIsClosed);

			leftQueue.Enqueue(CloseOrOpen(leftTemp));
			rightQueue.Enqueue(CloseOrOpen(rightTemp));
			if (leftQueue.Count >= 10) leftIsClosed = mode(leftQueue);
			if (rightQueue.Count >= 10) rightIsClosed = mode(rightQueue);
		}

		private int CloseOrOpen(Vector3[] finger)
		{
			distance = Vector3.Distance(finger[0], finger[12]);
			if (distance < closeThreshold)
			{
				return 1;
			}
			else return 0;
		}

		private int mode(Queue<int> queue)
		{
			int result = 0;
			int[] judge = new int[2];
			lock (queue)
			{
				int[] stack = queue.ToArray();
				for (int i = 0; i < stack.Length; i++)
				{
					if (stack[i] == 0)
						judge[0]++;
					else judge[1]++;
				}
				if (judge[0] > judge[1]) result = 0;
				else result = 1;

				queue.Clear();
			}

			return result;
		}

		public float GetDistance()
		{
			return distance;
		}
	}
}
