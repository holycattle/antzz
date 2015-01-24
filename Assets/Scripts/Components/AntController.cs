using UnityEngine;
using System.Collections;

public class AntController : ExtBehaviour {

	public float moveSpeed = 1f; // units per second
	public float rotateSpeed = 360f; // degrees per second
	public float scanRadius = 1f;
	public float scanArc = 90f;

	ScalarGrid grid;

	void Start() {
		grid = GameMgr.Instance.grid;
	}
	
	void Update() {
		float offsetAngle = Scan();
		Debug.Log(offsetAngle);
		transform.RotateAround(transform.position, Vector3.up, Mathf.Clamp(Mathf.Abs(offsetAngle), 0f, rotateSpeed * Time.deltaTime) * Mathf.Sign(offsetAngle));

		// Move Forward
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}

	float Scan() {
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
				}
			}
		}

		Debug.DrawRay(transform.position, new Vector3(targetDir.x, 0f, targetDir.y).normalized * 5f, Color.blue);
//		Debug.Log("Scanulating:" + targetDir);
//		return Vector2.Angle(forward, targetDir);
		Debug.Log(forward);
		Debug.Log(targetDir);
		return SMath.SignedAngleBetween(forward, targetDir.normalized, Vector3.back);
	}

}
