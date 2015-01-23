using UnityEngine;
using System.Collections;

public static class SMath {
	
	public static float FloorMultiple(float f, float m) {
		return Mathf.Floor(f / m) * m;
	}

	public static float CeilMultiple(float f, float m) {
		return Mathf.Ceil(f / m) * m;
	}

}
