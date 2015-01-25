using UnityEngine;
using System.Collections;

public class Termite : SoldierAnt {

	public int life = 10;

	public void Damage() {
		life--;

        GameObject damageCounterUI = (GameObject)Instantiate(GetResourceMgr().damageCounter, Camera.main.WorldToViewportPoint(gameObject.transform.position), Quaternion.identity);
        damageCounterUI.guiText.text = "1";
        damageCounterUI.guiText.fontStyle = FontStyle.Bold;

		if (life <= 0) {
			KillAnt();
		}
	}

	protected override void OnCollisionEnter(Collision c) {
		Ant a = c.collider.GetComponent<Ant>();
		if (a != null) {
			a.KillAnt();
		}
	}
}
