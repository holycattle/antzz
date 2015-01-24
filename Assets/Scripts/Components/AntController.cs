using UnityEngine;
using System.Collections;

public class AntController : MonoBehaviour {

	public float moveSpeed = 1f;
	
	void Start() {
		
	}
	
	void Update() {
		transform.position += transform.forward * Time.deltaTime * moveSpeed;
	}

}
