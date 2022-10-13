using System.Collections;
using System.Collections.Generic;
using Paon.NNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace Paon.NPlayer
{
	public class SelectEmojiScript2 : MonoBehaviour
	{
		public Material e1;

		public Material e2;

		public Material e3;

		public Material e4;

		public Material e5;

		private GameObject ApplyButton;

		private GameObject Now;

		private GameObject GC;

		private int select = 1;

		public bool isSelecting = false;

		[SerializeField]
		OpenCommorose oc;

		void Start()
		{
			ApplyButton = GameObject.Find("ApplyButton");
			ApplyButton.SetActive(false);
			Now = GameObject.Find("NowEmoji");
			GC = GameObject.Find("GameClient");
			//select = 1;
			// GC.GetComponent<GameClient>().SendFaceID(select);
		}

		void Update()
		{
			isSelecting = oc.Open;
			if (Input.GetKeyDown(KeyCode.Z))
			{
				isSelecting = true;
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				isSelecting = false;
				oc.Open = isSelecting;
			}
			if (isSelecting)
			{
				ApplyButton.SetActive(true);

				e1.color = new Color32(255, 255, 255, 110);
				e2.color = new Color32(255, 255, 255, 110);
				e3.color = new Color32(255, 255, 255, 110);
				e4.color = new Color32(255, 255, 255, 110);
				e5.color = new Color32(255, 255, 255, 110);
				if (select == 1)
				{
					e1.color = new Color32(255, 255, 255, 255);
				}
				else if (select == 2)
				{
					e2.color = new Color32(255, 255, 255, 255);
				}
				else if (select == 3)
				{
					e3.color = new Color32(255, 255, 255, 255);
				}
				else if (select == 4)
				{
					e4.color = new Color32(255, 255, 255, 255);
				}
				else if (select == 5)
				{
					e5.color = new Color32(255, 255, 255, 255);
				}
			}
			else
			{
				ApplyButton.SetActive(false);
				e1.color = new Color32(255, 255, 255, 0);
				e2.color = new Color32(255, 255, 255, 0);
				e3.color = new Color32(255, 255, 255, 0);
				e4.color = new Color32(255, 255, 255, 0);
				e5.color = new Color32(255, 255, 255, 0);
			}
			if (Resources.Load<Sprite>("Picture/Emoji" + select) != null)
			{
				Now.GetComponent<Image>().sprite =
						Resources.Load<Sprite>("Picture/Emoji" + select);
			}
		}

		void OnTriggerStay(Collider other)
		{
			if (isSelecting)
			{
				if (other.gameObject.name == "Emoji1")
				{
					select = 1;
				}
				else if (other.gameObject.name == "Emoji2")
				{
					select = 2;
				}
				else if (other.gameObject.name == "Emoji3")
				{
					select = 3;
				}
				else if (other.gameObject.name == "Emoji4")
				{
					select = 4;
				}
				else if (other.gameObject.name == "Emoji5")
				{
					select = 5;
				}
				else
				{
					select = 0;
				}
				GC.GetComponent<GameClient>().SendFaceID(select);
			}
		}
	}
}
