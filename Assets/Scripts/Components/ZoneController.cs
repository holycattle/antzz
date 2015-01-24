using UnityEngine;
using System.Collections;

public class ZoneController : MonoBehaviour {
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.GetComponent<Ant>() != null) {
			c.GetComponent<AntController>().OnEnterAnthillRadius(this);
		}
	}

}
