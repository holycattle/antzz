using UnityEngine;
using System.Collections;

public class SceneLoader : ExtBehaviour {
	void Awake() {
		Application.LoadLevelAdditive("MainScene");
	}

	void Start() {
		//load stuff into GameMgr game object
		GameObject goRoot = GameObject.Find("Root");
		if (goRoot != null) {
			Debug.Log(goRoot);
			while (goRoot.transform.childCount > 0) {
				goRoot.transform.GetChild(0).parent = gameObject.transform;
			}
			
			Destroy(goRoot);
			
			GetNotifyMgr().PostNotify(NotifyType.LoadGameSceneDone, this);
			
			//once loading is done, destroy this script
			Destroy(this);
		}
	}
}
