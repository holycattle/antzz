using UnityEngine;
using System.Collections;

public class Ant : ExtBehaviour {
	public bool hasFood { get; internal set; }
	public bool isInTrigger;

	public void KillAnt() {
		Destroy(gameObject);
	}

	protected virtual void OnCollisionEnter(Collision c) {
		CollectFood(c.gameObject);
	}

	void CollectFood(GameObject g) {
		Food f = g.GetComponent<Food>();
		if (f == null || f.IsOwned() || hasFood)
			return;

		hasFood = true;

		int layerIndex = LayerMask.NameToLayer("AntWithFood");
		gameObject.layer = layerIndex;
		foreach (Transform t in gameObject.transform)
			t.gameObject.layer = layerIndex;

		f.SetIsOwned(true);        
		Vector3 oldPos = gameObject.transform.position;
		g.transform.position = new Vector3(oldPos.x, oldPos.y + 0.25f, oldPos.z);
		g.transform.parent = gameObject.transform;

        GetComponent<AntController>().followAnt = false;
	}
}
