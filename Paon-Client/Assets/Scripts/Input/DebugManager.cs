using UnityEngine;

public class DebugManager : MonoBehaviour
{
	public bool isDebugEnabled = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return)) isDebugEnabled = !isDebugEnabled;
	}
}
