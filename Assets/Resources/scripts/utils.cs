using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utils : MonoBehaviour {

	public static float ClampAngle(float angle, float min, float max)
	{
		if (min < 0 && max > 0 && (angle > max || angle < min))
		{
			angle -= 360;
			if (angle > max || angle < min)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
				else return max;
			}
		}
		else if(min > 0 && (angle > max || angle < min))
		{
			angle += 360;
			if (angle > max || angle < min)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
				else return max;
			}
		}

		if (angle < min) return min;
		else if (angle > max) return max;
		else return angle;
	}
	public static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}
}
