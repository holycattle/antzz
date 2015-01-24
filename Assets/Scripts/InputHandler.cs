using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : ExtBehaviour {

	public float heatRadius = 0.5f;

	ScalarGrid grid;
	ParticleHandler particles;
	Camera mainCamera;

	List<FingerTouch> fingers;

	void Start() {
		fingers = new List<FingerTouch>();

		mainCamera = Camera.main;
		grid = GameMgr.Instance.grid;
		particles = GameMgr.Instance.particles;
	}

	void Update() {
		for (int i = 0; i < fingers.Count; i++) {
			fingers[i].wasUpdated = false;
		}

		if (Input.touchCount > 0) {
			Touch t;
			for (int i = 0; i < Input.touchCount; i++) {
				t = Input.GetTouch(i);
				HandleTouch(t.position, t.fingerId);
			}
		} else {
			if (Input.GetMouseButton(0)) {
				HandleTouch(Input.mousePosition, 1);
			}
		}

		// Clean out fingers
		for (int i = 0; i < fingers.Count; i++) {
			if (!fingers[i].wasUpdated) {
				Debug.Log("Deleting!");
				fingers.RemoveAt(i);
				i--;
			}
		}
	}

	public void HandleTouch(Vector3 screenPoint, int fingerIndex) {
		bool wasNew = false;
		FingerTouch touch = null;
		for (int i = 0; i < fingers.Count; i++) {
			if (fingers[i].fingerID == fingerIndex) {
				touch = fingers[i];
				break;
			}
		}
		if (touch == null) {
			touch = new FingerTouch();
			touch.fingerID = fingerIndex;
			touch.particleIndex = -1;

			// Look for an available particleIndex
			for (int i = 0; i < particles.numHandlersToSpawn; i++) {
				bool isInUse = false;
				for (int o = 0; o < fingers.Count; o++) {
					if (fingers[o].particleIndex == i) {
						isInUse = true;
						break;
					}
				}
				if (!isInUse) {
					touch.particleIndex = i;
					break;
				}
			}
			if (touch.particleIndex == -1) {
				Debug.LogError("Ran out of available Particle Handlers!");
				return;
			}

			fingers.Add(touch);
			wasNew = true;
		}

		touch.wasUpdated = true;

		Vector3 floorPoint = RaycastToFloor(screenPoint);
		grid.HeatPoint(floorPoint, heatRadius);
		particles.Get(touch.particleIndex).Particlify(floorPoint, !wasNew);
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

	private class FingerTouch {
		public int fingerID;
		public int particleIndex;
		public bool wasUpdated;
	}
}
