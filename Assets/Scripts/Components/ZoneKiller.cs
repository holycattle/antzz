using UnityEngine;
using System.Collections;

public class ZoneKiller : ExtBehaviour {
	
	void OnTriggerEnter(Collider c) {
        SoldierAnt s = c.gameObject.GetComponent<SoldierAnt>();
        if (s != null) {
            GetComponentInParent<ZoneParent>().DecreaseScore(s.penalty);
            s.KillAnt();
            return;
        }

        Ant a = c.gameObject.GetComponent<Ant>();
		if (a != null) {
			if (a.hasFood) {
				GetComponentInParent<ZoneParent>().IncreaseScore();
			}

			a.KillAnt();
		}
	}

}
