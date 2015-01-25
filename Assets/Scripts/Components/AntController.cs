using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntController : ExtBehaviour {

	private static AntController[] antList;

	public bool followAnt;

	public float moveSpeed = 1f; // units per second
	public float rotateSpeed = 360f; // degrees per second
	public float scanRadius = 1f;
	public float scanArc = 180f;
	public float backScanOffset = 0.5f;

	public float followAngleThreshold = 10f;
	public float antFollowRange = 4f;
	public float antFollowArc = 90f;
	public float antDirectionalFollowThreshold = 135f;

	ScalarGrid grid;
	ZoneController capturedZoneController;

	bool isScanning = false;
	float curOffsetAngle = 0f;

	void Start() {
		grid = GameMgr.Instance.grid;
		capturedZoneController = null;

		// Whenever a new AntController is created, update the list.
		antList = GameObject.FindObjectsOfType<AntController>();
	}
	
	void Update() {
		float moveSpeedMult = 1f;
		if (capturedZoneController != null) {
			Vector3 q = Quaternion.LookRotation(capturedZoneController.transform.position - transform.position).eulerAngles;
			q.x = 0f;
			q.z = 0f;
			transform.rotation = Quaternion.Euler(q);
		} else {
			if (!isScanning) {
				StartCoroutine(UpdateScan());
			}

			float amtToTurn = Mathf.Clamp(Mathf.Abs(curOffsetAngle), 0f, rotateSpeed * Time.deltaTime) * Mathf.Sign(curOffsetAngle);
			transform.RotateAround(transform.position, Vector3.up, amtToTurn);
			curOffsetAngle -= amtToTurn;
		}
		transform.position += transform.forward * Time.deltaTime * moveSpeed * moveSpeedMult;
	}

	IEnumerator UpdateScan() {
		isScanning = true;
		float offsetAngle = Scan();

		if (followAnt) {
			if (Mathf.Abs(offsetAngle) < followAngleThreshold) {
				float newOffset = ScanAnts();
				if (newOffset != 0) {
					offsetAngle = newOffset;
				}
			}
		}
        
		curOffsetAngle = offsetAngle;

		yield return null;

		isScanning = false;
//		yield return null;
	}

	float ScanAnts() {
		int closestIndex = -1;
		float closestDistance = Mathf.Infinity;
		for (int i = 0; i < antList.Length; i++) {
			if (antList[i] == null || antList[i] == this) {
				continue;
			}

			float d = Vector3.Distance(transform.position, antList[i].transform.position);
			if (d < closestDistance) {
				if (d < antFollowRange) {
					if (Vector3.Angle(transform.forward, antList[i].transform.position - transform.position) < antFollowArc) {
						if (Vector3.Angle(transform.forward, antList[i].transform.forward) < antDirectionalFollowThreshold) {
							closestIndex = i;
							closestDistance = d;
						}
					}
				}
			}
		}

		if (closestIndex != -1) {
			return SMath.SignedAngleBetween(transform.forward, antList[closestIndex].transform.position - transform.position, Vector3.up);
		}

		return 0f;
	}

	float Scan() {
		bool wasTargetDirUpdated = false;
		bool hasNonZeroScalarGrid = false;
		bool hasNonZeroForward = false;

		Vector2 targetDir = Vector2.zero;

		Vector2 leftDir = Vector2.zero;
		Vector2 frontDir = Vector2.zero;
		Vector2 rightDir = Vector2.zero;

		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
		pos = pos - forward.normalized * backScanOffset;

		Vector3[] vectors = grid.SquaresInBox(transform.position, scanRadius);
		for (int i = 0; i < vectors.Length; i++) {
			Vector2 vecPos = new Vector2(vectors[i].x, vectors[i].z);

			if (Vector2.Distance(pos, vecPos) < scanRadius) {
				Vector2 dir = vecPos - pos;
				
//				float angle = Vector2.Angle(forward, dir);
				float angle = SMath.SignedAngleBetween(forward, dir, Vector3.forward);
				if (Mathf.Abs(angle) < scanArc / 2) {
					if (Mathf.Abs(angle) < scanArc / 6) {
						// Forward
						frontDir += dir * Mathf.Clamp01(1 - dir.magnitude / scanRadius) * Mathf.Clamp01(1 - vectors[i].y);
						if (vectors[i].y > 0) {
							hasNonZeroForward = true;
						}
					} else if (angle > scanArc / 6) {
						// Right
						rightDir += dir * Mathf.Clamp01(1 - dir.magnitude / scanRadius) * Mathf.Clamp01(1 - vectors[i].y);
					} else {
						// Left
						leftDir += dir * Mathf.Clamp01(1 - dir.magnitude / scanRadius) * Mathf.Clamp01(1 - vectors[i].y);
					}

//					targetDir += dir * Mathf.Clamp01(1 - dir.magnitude / scanRadius) * Mathf.Clamp01(1 - vectors[i].y);

					wasTargetDirUpdated = true;
					if (vectors[i].y > 0) {
						hasNonZeroScalarGrid = true;
					}
				}
			}
		}

		if (!wasTargetDirUpdated || !hasNonZeroScalarGrid || !hasNonZeroForward) {
			return 0f;
		}
		
		if (frontDir.magnitude > rightDir.magnitude) {
			if (frontDir.magnitude > leftDir.magnitude) {
				targetDir = frontDir;
			} else {
				targetDir = leftDir;
			}
		} else {
			if (rightDir.magnitude > leftDir.magnitude) {
				targetDir = rightDir;
			} else {
				targetDir = leftDir;
			}
		}

//		Debug.DrawRay(transform.position, new Vector3(targetDir.x, 0f, targetDir.y).normalized * 5f, Color.blue);
//		Debug.Log("Scanulating:" + targetDir);
//		return Vector2.Angle(forward, targetDir);
//		Debug.Log(forward);
//		Debug.Log(targetDir);
		return SMath.SignedAngleBetween(forward, targetDir.normalized, Vector3.back);
	}

	public void OnExitBounds() {
		Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
		Debug.Log("View:" + v);

		Vector3 offset = Vector3.zero;
		if (v.x <= 0 || v.x >= 1) {
			if (capturedZoneController == null) {
				GetComponent<Ant>().KillAnt();
			}
		} else if (v.y < 0) {
			offset = Vector3.forward * 9f;
			Debug.Log("Exit Bot");
		} else if (v.y > 1) {
			offset = -Vector3.forward * 9f;
		} else {
			Debug.LogError("Shit I didnt do something");
		}

		transform.position += offset;
	}

	public void OnEnterAnthillRadius(ZoneController zone) { // Don't change the name of this function, it's called via a SendMessage()
		capturedZoneController = zone;
	}

}
