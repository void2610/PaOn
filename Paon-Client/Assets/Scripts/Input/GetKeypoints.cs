using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExtraMethods;
using MediaPipe.Holistic;
using UnityEngine;

namespace Paon.NInput
{
	public class GetKeypoints : MonoBehaviour
	{
		[SerializeField]
		private GameObject HandEstimator;

		[SerializeField]
		private GameObject PoseEstimator;

		[SerializeField]
		private GameObject Estimator;

		private PoseEstimator _PoseEstimator;

		private Utils.Keypoint[] poseKeypoints;

		private Visualizer _handVisualizer;

		private Visualizer _visualizer;

		public enum LeftOrRight
		{
			left, right
		};

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

		public Vector3[] keypoints = new Vector3[25];

		public Keypoint leftWrist = new Keypoint(0, 0, 0, 0);

		public Keypoint rightWrist = new Keypoint(0, 0, 0, 0);

		public float closeThreshold = 0.05f;

		public int leftIsClosed = 0;
		public int rightIsClosed = 0;

		float leftScore, rightScore;

		Queue<int> leftQueue = new Queue<int>();
		Queue<int> rightQueue = new Queue<int>();

		public float distance = 999;

		Vector3[] leftTemp, rightTemp;

		public Vector3[] areaThreshold;

		public enum thType
		{
			leftHand,
			rightHand,
			leftLeg,
			rightLeg
		};

		void Start()
		{
			_PoseEstimator = PoseEstimator.GetComponent<PoseEstimator>();
			_handVisualizer = HandEstimator.GetComponent<Visualizer>();
			_visualizer = Estimator.GetComponent<Visualizer>();
			string str = " ";
			if (PlayerPrefs.HasKey("CloseThreshold")) closeThreshold = PlayerPrefs.GetFloat("CloseThreshold");
			if (PlayerPrefs.HasKey("AreaTh")) str = PlayerPrefs.GetString("AreaTh");
			// Debug.Log("AreaTh: " + string.Join(", ", areaThreshold.Select(x => x.ToString)));
			Debug.Log(str);
		}

		void Update()
		{
			// poseKeypoints = _PoseEstimator.GetKeypoints();
			// int cnt = 0;
			// foreach (Utils.Keypoint key in poseKeypoints)
			// {
			// 	pose[cnt] = new Keypoint(key.position.x, key.position.y, 0, key.score);
			// 	// Debug.Log("pose[" + cnt + "]: " + pose[cnt].coords);
			// 	// Debug.Log("pose[" + cnt + "]: " + pose[cnt].coords);
			// }

			keypoints = _visualizer.GetPoseVertices();

			leftWrist = pose[9];
			rightWrist = pose[10];

			leftTemp = _handVisualizer.GetLeftVert();
			rightTemp = _handVisualizer.GetRightVert();

			for (int i = 0; i < leftTemp.Length - 1; i++)
			{
				leftTemp[i] = new Vector3(leftTemp[i].x.map(0, 1, -1, -10),
				leftTemp[i].y.map(0, 1, -1, 3), leftTemp[i].z);
			}

			for (int i = 0; i < rightTemp.Length - 1; i++)
			{
				rightTemp[i] = new Vector3(rightTemp[i].x.map(0, 1, -1, -10),
				rightTemp[i].y.map(0, 1, -1, 3), rightTemp[i].z);
			}


			leftScore = leftTemp[21].x;
			rightScore = rightTemp[21].x;
			// Debug.Log("leftScore : " + leftScore);
			// Debug.Log("rightScore : " + rightScore);

			// leftIsClosed = leftCloseOrOpen(leftTemp);
			// rightIsClosed = rightCloseOrOpen(rightTemp);
			// Debug.Log("Left: " + leftIsClosed);
			// Debug.Log("Right: " + rightIsClosed);
			// leftQueue.Enqueue
			// leftQueue.Enqueue(CloseOrOpen(leftTemp, LeftOrRight.left));
			if (rightScore > 0.7f)
				// rightQueue.Enqueue
				// rightQueue.Enqueue(CloseOrOpen(rightTemp, LeftOrRight.right));
				if (leftQueue.Count >= 15) leftIsClosed = mode(leftQueue);
			if (rightQueue.Count >= 15) rightIsClosed = mode(rightQueue);
		}

		private int CloseOrOpen(Vector3[] finger, LeftOrRight type)
		{
			distance = Vector3.Distance(finger[0], finger[12]);
			float score = 0;
			if (type == LeftOrRight.left) score = leftScore;
			else score = rightScore;

			if (distance < closeThreshold && score > 0.7f)
			{
				return 1;
			}
			else return 0;
		}

		private int leftCloseOrOpen(Vector3[] finger)
		{
			distance = Vector3.Distance(finger[0], finger[12]);
			if (distance < closeThreshold && leftScore > 0.7f)
			{
				return 1;
			}
			else return 0;
		}

		private int rightCloseOrOpen(Vector3[] finger)
		{
			distance = Vector3.Distance(finger[0], finger[12]);
			if (distance < closeThreshold && rightScore > 0.7f)
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

		public float GetRightDistance()
		{
			return distance = Vector3.Distance(rightTemp[0], rightTemp[12]);
		}

		public float GetLeftDistance()
		{
			return distance = Vector3.Distance(leftTemp[0], leftTemp[12]);
		}

		public float GetDistance(Vector3 wrist, Vector3 top)
		{
			return distance = Vector3.Distance(wrist, top);
		}

		public float GetScore(LeftOrRight handedness)
		{
			if (handedness == LeftOrRight.left) return leftScore;
			else return rightScore;
		}
	}
}
