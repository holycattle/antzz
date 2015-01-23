using UnityEngine;
using System.Collections;

public class SceneLoader : ExtBehaviour 
{
    AsyncOperation loadOp;

    IEnumerator Start() {
        loadOp = Application.LoadLevelAdditiveAsync("MainScene");
        yield return async;
    }

    void Update() {
        if (loadOp.isDone) {
            //load stuff into GameMgr game object
            GameObject goRoot = Util.Find(gameObject, "Root");
            if (goRoot != null) {
                foreach (Transform child in goRoot.transform) {
                    child.parent = goRoot.transform;
                }
            }

            //once loading is done, destroy this script
            Destroy(goRoot);
            Destroy(this);
        }
    }
}
