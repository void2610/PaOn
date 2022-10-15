using System.Collections;
using System.Collections.Generic;
using Paon.NInput;
using Paon.NPlayer;
using UnityEngine;

namespace Paon.NBordering
{
	public class RightHoldHOLDScript : MonoBehaviour
	{
		public AudioClip SE;

		public GameObject NearObject;

		private GameObject Hand;

		private GameObject Player;

		public ObjectHolder oh = new ObjectHolder();

		public RightHandInputProvider rmip = null;

		private LeftHandInputProvider lmip = null;

		private RightHandMove rhm = null;

		private BorderingTimerScript bts = null;

		private Vector3 bodyBase;

		private Vector3 prev = Vector3.zero;

		private string tmp = "none";

		private float dis = 999;

		[SerializeField]
		private OpenCommorose oc;
		public bool isRunning = false;
		private bool isDebugEnabled = false;
		private DebugManager debugger;

		void Start()
		{
			Player = GameObject.Find("PlayerBody");
			Hand = GameObject.Find("RightHand");
			rmip =
					GameObject
							.Find("RightHandInputProvider")
							.GetComponent<RightHandInputProvider>();
			lmip =
					GameObject
							.Find("LeftHandInputProvider")
							.GetComponent<LeftHandInputProvider>();
			rhm = Hand.GetComponent<RightHandMove>();
			bts =
					GameObject
							.Find("BorderingManager")
							.GetComponent<BorderingTimerScript>();
			debugger = GameObject.Find("DebugManager").GetComponent<DebugManager>();
		}

		void Update()
		{
			isDebugEnabled = debugger.isDebugEnabled;
			Vector3 pos = rmip.GetPosition();

			//掴んでいるかどうか
			if (rmip.CheckHold() == 1)
			{
				//新しく物をつかんだときの処理
				if (NearObject != null && oh.NowHoldObject == null)
				{
					oh.HoldObject(NearObject);
					if (oh.NowHoldObject.tag == "BorderingHOLDTag")
					{
						GetComponent<AudioSource>().PlayOneShot(SE);
						bts.StartTimer();
						isRunning = true;
						oc.isBordering = true;
						Player.GetComponent<Rigidbody>().useGravity = false;
						StopCoroutine(nameof(GravityFall));
						bodyBase = Player.transform.position;
					}
				}
			}
			else
			{
				if (oh.NowHoldObject != null)
				{
					//物を離したときの処理
					if (oh.NowHoldObject.tag == "BorderingHOLDTag")
					{
						//もう片方が掴んでいなかったら固定解除
						if (lmip.CheckHold() == 0)
						{
							StartCoroutine(nameof(GravityFall));
						}

						//手の位置を戻す
						if (!isDebugEnabled)
							Hand.transform.localPosition = new Vector3(2f, 0, 3.4f);
					}
				}
				oh.UnHold();
			}

			//ホールドを掴んでいるときの処理
			if (oh.Holding)
			{
				//手動かなくする
				Player.GetComponent<PlayerMove>().canMove = false;
				rhm.canMove = false;
				Hand.transform.position = oh.NowHoldObject.transform.position;

				if (pos.y < prev.y && Player.transform.position.y <= oh.NowHoldObject.transform.position.y + 0.3f)
				{
					Player.transform.Translate(Vector3.up * 0.1f);
				}
			}
			else
			{
				//掴んで無かったら手動かなくしない
				Player.GetComponent<PlayerMove>().canMove = true;
				rhm.canMove = true;
			}

			//近くの物との距離を計算
			if (NearObject != null)
			{
				dis =
						Vector3
								.Distance(this.gameObject.transform.position,
								NearObject.transform.position);
			}

			tmp = rmip.GetInput();

			if (pos != prev) prev = pos;
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.tag == "BorderingHOLDTag")
			{
				if (NearObject != other.gameObject)
				{
					if (NearObject != null)
					{
						if (NearObject.GetComponent<Outline>())
						{
							NearObject.GetComponent<Outline>().OutlineWidth = 0;
						}
					}
				}
				NearObject = other.gameObject;
				if (NearObject.GetComponent<Outline>())
				{
					NearObject.GetComponent<Outline>().OutlineWidth = 8;
					NearObject.GetComponent<Outline>().OutlineColor = Color.red;
				}
			}
		}

		void OnTriggerExit(Collider other)
		{
			//つかめない距離になったらアウトラインを消す
			if (other.CompareTag("BorderingHOLDTag"))
			{
				if (NearObject == other.gameObject)
				{
					if (NearObject.GetComponent<Outline>())
					{
						NearObject.GetComponent<Outline>().OutlineWidth = 0;
					}
					NearObject = null;
				}
			}
		}

		IEnumerator GravityFall()
		{
			yield return new WaitForSeconds(3);
			Player.GetComponent<Rigidbody>().useGravity = true;
		}
	}
}
