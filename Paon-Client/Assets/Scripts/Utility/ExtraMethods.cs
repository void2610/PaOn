using UnityEngine;

namespace ExtraMethods
{
	public static class ExtraMethods
	{
		public static float map(this float origin, float inMin, float inMax, float outMin, float outMax, bool trim = false)
		{
			float value = (origin - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
			return trim ? Mathf.Max(Mathf.Min(value, outMax), outMin) : value;
		}
	}

}
