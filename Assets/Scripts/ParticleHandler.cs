using UnityEngine;
using System.Collections;

public class ParticleHandler : MonoBehaviour {

	public int numHandlersToSpawn = 4;

	HeatParticles[] particles;

	void Start() {
		GameObject g = transform.GetChild(0).gameObject;

		particles = new HeatParticles[numHandlersToSpawn];
		particles[0] = g.GetComponent<HeatParticles>();
		for (int i = 0; i < numHandlersToSpawn - 1; i++) {
			GameObject ng = Instantiate(g) as GameObject;
			ng.transform.parent = transform;
			particles[i + 1] = ng.GetComponent<HeatParticles>();
		}
	}

	public HeatParticles Get(int i) {
		return particles[Mathf.Clamp(i, 0, particles.Length)];
	}

}
