using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
	public class CloseCommoroseScript : MonoBehaviour
	{
		[SerializeField]
		comocmo co;
		SelectEmojiScript2 SES2;

		void Start()
		{
			SES2 = GameObject.Find("RightHand").GetComponent<SelectEmojiScript2>();
		}

		void OnTriggerStay(Collider other)
		{
			Debug.Log(other.gameObject.name);
			if (other.gameObject.name == "LeftHand")
			{
				SES2.isSelecting = false;
				co.Open = SES2.isSelecting;
			}
			else if (other.gameObject.name == "BorderingStart")
				co.isBordering = true;
		}
	}
}
