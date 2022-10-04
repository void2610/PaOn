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

		public Keypoint[] right = new Keypoint[21];

		public Keypoint[] left = new Keypoint[21];

		public Keypoint leftWrist = new Keypoint(0, 0, 0, 0);

		public Keypoint rightWrist = new Keypoint(0, 0, 0, 0);

		public float holdThreshold = 0.05f;

		public int leftIsClosed = 0;
		public int rightIsClosed = 0;

		Queue<int> leftQueue = new Queue<int>();
		Queue<int> rightQueue = new Queue<int>();

		void Start()
		{
			_PoseEstimator = PoseEstimator.GetComponent<PoseEstimator>();
			_handVisualizer = HandEstimatior.GetComponent<Visualizer>();
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

			leftWrist.coords.z = leftTemp[0].z;
			rightWrist.coords.z = rightTemp[0].z;

			// leftIsClosed = leftCloseOrOpen(leftTemp);
			// rightIsClosed = rightCloseOrOpen(rightTemp);
			// Debug.Log("Left: " + leftIsClosed);
			// Debug.Log("Right: " + rightIsClosed);

			leftQueue.Enqueue(leftCloseOrOpen(leftTemp));
			rightQueue.Enqueue(rightCloseOrOpen(rightTemp));
			if (leftQueue.Count >= 10) leftIsClosed = mode(leftQueue);
			if (rightQueue.Count >= 10) rightIsClosed = mode(rightQueue);
		}

		private int leftCloseOrOpen(Vector3[] finger)
		{
			float distance = Vector3.Distance(finger[0], finger[12]);
			// Debug.Log("Distance: " + distance);
			if (distance < holdThreshold)
				return 1;
			else return 0;
		}

		private int rightCloseOrOpen(Vector3[] finger)
		{
			float distance = Vector3.Distance(finger[0], finger[12]);
			Debug.Log("rightDistance: " + distance);
			if (distance < holdThreshold)
			{
				Debug.Log("right is closed");
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
	}
}
