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

        Termite t = c.gameObject.GetComponent<Termite>();
        if (s != null) {
            GetComponentInParent<ZoneParent>().DecreaseScore(t.penalty);
            t.KillAnt();
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
