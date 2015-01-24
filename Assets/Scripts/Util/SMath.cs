using UnityEngine;
using System.Collections;

public static class SMath {
	
	public static float FloorMultiple(float f, float m) {
		return Mathf.Floor(f / m) * m;
	}

	public static float CeilMultiple(float f, float m) {
		return Mathf.Ceil(f / m) * m;
	}

	public static float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 normal) {
		// angle in [0,180]
		float angle = Vector3.Angle(a, b);
		float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(a, b)));
		
		// angle in [-179,180]
		float signed_angle = angle * sign;
		
		// angle in [0,360] (not used but included here for completeness)
		//float angle360 =  (signed_angle + 180) % 360;
		
		return signed_angle;
	}
	
	public static float ClockwiseAngleBetween(Vector3 a, Vector3 b, Vector3 normal) {
		return (SignedAngleBetween(a, b, normal) + 180) % 360;
	}
}
