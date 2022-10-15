using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paon.NPlayer
{
	public class InitializeScript : MonoBehaviour
	{
		void Awake()
		{
			AudioListener.volume = 0;
			StartCoroutine(nameof(DelayAudio));
		}

		// void Update()
		// {
		// 	if (Time.time < 5)
		// 	{
		// 		AudioListener.volume = 0;
		// 	}
		// 	else
		// 	{
		// 		AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
		// 	}
		// }

		IEnumerator DelayAudio()
		{
			yield return new WaitForSeconds(5);
			AudioListener.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
		}
	}
}
