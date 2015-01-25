using UnityEngine;
using System.Collections;

public class Termite : SoldierAnt {

	int life = 10;

	public void Damage() {
		life--;
		if (life <= 0) {
			KillAnt();
		}
	}

}
