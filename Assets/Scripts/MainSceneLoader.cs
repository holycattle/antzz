using UnityEngine;
using System.Collections;

public class MainSceneLoader : MonoBehaviour {

	void Awake() {
		if (Application.loadedLevelName == "_Main") {
			GameObject.Destroy(gameObject);
			return;
		}

		Application.LoadLevel("_Main");
	}

}
