using UnityEngine;
using System.Collections;

public class BoundsCollider : MonoBehaviour {

	void OnTriggerExit(Collider c) {
		c.SendMessage("OnExitBounds");
	}

}
