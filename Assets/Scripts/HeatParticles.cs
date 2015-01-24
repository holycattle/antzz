using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class HeatParticles : ExtBehaviour {

	public float particleSizeMultiplier = 3f; // Use this to make the blotch bigger or smaller
	public float minInterpolationDistance = 0.2f;

	ParticleSystem particles;
	InputHandler input;
	ScalarGrid grid;

	void Start() {
		particles = GetComponent<ParticleSystem>();
		input = GameMgr.Instance.inputter;
		grid = GameMgr.Instance.grid;

		// Initialize Particle System variables based on Input and Grid
		particles.startLifetime = 1f / grid.decayRate;
		particles.startSize = input.heatRadius * 2f * particleSizeMultiplier;
	}

	public void Particlify(Vector3 pos, bool interpolate = true) {
		if (interpolate) {
			Vector3 toMove = pos - transform.position;
			while (toMove.magnitude > minInterpolationDistance) {
				transform.position += toMove.normalized * minInterpolationDistance;
				particles.Emit(1);
				toMove = pos - transform.position;
			}
		}

		transform.position = pos;
		particles.Emit(1);
	}

}
