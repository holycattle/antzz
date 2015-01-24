using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class HeatParticles : ExtBehaviour {

	ParticleSystem particles;
	InputHandler input;
	ScalarGrid grid;

	void Awake() {
		particles = GetComponent<ParticleSystem>();
	}

	public void Particlify(Vector3 pos) {
		byte b = 1;
		particles.Emit(pos, Vector3.zero, input.heatRadius, grid.decayRate, new Color32(b, b, b, 1));
	}

}
