using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	public float heatRadius = 0.5f;

	Camera mainCamera;

	void Start() {
		mainCamera = Camera.main;
	}

	void Update() {
		if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; i++) {
				HandleTouch(Input.GetTouch(i));
			}
		}
	}

	public void HandleTouch(Touch t) {
		Vector3 floorPoint = RaycastToFloor(t.position);

	}

	public Vector3 RaycastToFloor(Vector3 screenPoint) {
		Ray r = mainCamera.ScreenPointToRay(screenPoint);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity)) {
			return hit.point;
		}
		Debug.LogError("The raycast missed! It shouldn't have missed!");
		return Vector3.one * Mathf.Infinity;
	}
}
