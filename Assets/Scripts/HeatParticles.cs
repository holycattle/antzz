using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class HeatParticles : ExtBehaviour {

	public float particleSizeMultiplier = 3f; // Use this to make the blotch bigger or smaller

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

	public void Particlify(Vector3 pos) {
		transform.position = pos;
		particles.Emit(1);
	}

}
