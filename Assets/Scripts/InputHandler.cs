using UnityEngine;
using System.Collections;

public class InputHandler : ExtBehaviour {

	public float heatRadius = 0.5f;

//	ScalarGrid grid;
	Camera mainCamera;

	void Start() {
		mainCamera = Camera.main;
//		grid = GetComponent<ScalarGrid>();
	}

	void Update() {
		if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; i++) {
				HandleTouch(Input.GetTouch(i).position);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				HandleTouch(Input.mousePosition);
			}
		}
	}

	public void HandleTouch(Vector3 screenPoint) {
		Vector3 floorPoint = RaycastToFloor(screenPoint);
//		grid.HeatPoint(floorPoint, heatRadius);
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
