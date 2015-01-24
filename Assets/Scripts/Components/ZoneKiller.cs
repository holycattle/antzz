using UnityEngine;
using System.Collections;

public class ZoneKiller : MonoBehaviour {
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.GetComponent<Ant>() != null) {
			Ant a = c.gameObject.GetComponent<Ant>();

			if (a.hasFood) {
				GetComponentInParent<ZoneParent>().IncreaseScore();
			}

			a.KillAnt();
		}
	}

}
