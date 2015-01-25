using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : ExtBehaviour {

	public float heatRadius = 0.5f;
	public float minInterpolationDistance = 0.2f;

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
			touch.isTouchDown = true;

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

		if (touch.isTouchDown) { 
			Termite t = RaycastTermite(screenPoint);
			if (t != null) {
				t.Damage();
			}
		}

		Vector3? rayHit = RaycastToFloor(screenPoint);
		if (rayHit == null) {
			return;
		}

		Vector3 floorPoint = rayHit.Value;
		HeatParticles touchParticles = particles.Get(touch.particleIndex);

		if (wasNew) {
			touch.pos = floorPoint;
		}

		Vector3 toMove = floorPoint - touch.pos;
		while (toMove.magnitude > minInterpolationDistance) {
			touch.pos += toMove.normalized * minInterpolationDistance;

			grid.HeatPoint(touch.pos, heatRadius);
			touchParticles.Particlify(touch.pos);

			toMove = floorPoint - touch.pos;
		}

		grid.HeatPoint(floorPoint, heatRadius);
		touchParticles.Particlify(floorPoint);

		touch.pos = floorPoint;
		touch.wasUpdated = true;
		touch.isTouchDown = false;
	}

	public Vector3? RaycastToFloor(Vector3 screenPoint) {
		Ray r = mainCamera.ScreenPointToRay(screenPoint);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity)) {
			return hit.point;
		}
		return null;
	}

	public Termite RaycastTermite(Vector3 screenPoint) {
		Ray r = mainCamera.ScreenPointToRay(screenPoint);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Termite"))) {
			return hit.collider.GetComponentInParent<Termite>();
		}
		return null;
	}

	private class FingerTouch {
		public int fingerID;
		public int particleIndex;
		public Vector3 pos;
		public bool wasUpdated;
		public bool isTouchDown;
	}
}
