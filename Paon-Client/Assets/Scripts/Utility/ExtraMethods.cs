using UnityEngine;

namespace ExtraMethods
{
	public static class ExtraMethods
	{
		public static float map(this float origin, float inMin, float inMax, float outMin, float outMax)
		{
			return (origin - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}
	}

}
