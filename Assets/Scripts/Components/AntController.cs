using UnityEngine;
using System.Collections;

public class AntController : ExtBehaviour {

	public float moveSpeed = 1f; // units per second
	public float rotateSpeed = 360f; // degrees per second
	public float scanRadius = 1f;
	public float scanArc = 180f;
	ScalarGrid grid;
	ZoneController capturedZoneController;

	void Start() {
		grid = GameMgr.Instance.grid;
		capturedZoneController = null;
	}
	
	void Update() {
		if (capturedZoneController != null) {
			Vector3 q = Quaternion.LookRotation(capturedZoneController.transform.position - transform.position).eulerAngles;
			q.x = 0f;
			q.z = 0f;
			transform.rotation = Quaternion.Euler(q);
		} else {
			float offsetAngle = Scan();
			transform.RotateAround(transform.position, Vector3.up, Mathf.Clamp(Mathf.Abs(offsetAngle), 0f, rotateSpeed * Time.deltaTime) * Mathf.Sign(offsetAngle));
		}
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}

	float Scan() {
		bool wasTargetDirUpdated = false;
		bool hasNonZeroScalarGrid = false;
		Vector2 targetDir = Vector2.zero;

		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);

		Vector3[] vectors = grid.SquaresInBox(transform.position, scanRadius);
		for (int i = 0; i < vectors.Length; i++) {
			Vector2 vecPos = new Vector2(vectors[i].x, vectors[i].z);

			if (Vector2.Distance(pos, vecPos) < scanRadius) {
				Vector2 dir = vecPos - pos;

				if (Vector2.Angle(forward, dir) < scanArc / 2) {
					targetDir += dir * Mathf.Clamp01(1 - dir.magnitude / scanRadius) * Mathf.Clamp01(1 - vectors[i].y);

					wasTargetDirUpdated = true;
					if (vectors[i].y > 0) {
						hasNonZeroScalarGrid = true;
					}
				}
			}
		}

		if (!wasTargetDirUpdated || !hasNonZeroScalarGrid) {
			return 0f;
		}

//		Debug.DrawRay(transform.position, new Vector3(targetDir.x, 0f, targetDir.y).normalized * 5f, Color.blue);
//		Debug.Log("Scanulating:" + targetDir);
//		return Vector2.Angle(forward, targetDir);
//		Debug.Log(forward);
//		Debug.Log(targetDir);
		return SMath.SignedAngleBetween(forward, targetDir.normalized, Vector3.back);
	}

	public void OnEnterAnthillRadius(ZoneController zone) { // Don't change the name of this function, it's called via a SendMessage()
		capturedZoneController = zone;
	}

}
